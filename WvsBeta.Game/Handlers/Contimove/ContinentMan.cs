using System;
using System.Collections.Generic;
using System.Linq;
using WvsBeta.Common;
using WvsBeta.Common.Extensions;
using WvsBeta.Common.Sessions;

namespace WvsBeta.Game.Handlers.Contimove
{
    public enum Conti : byte
    {
        Dormant = 0xFF,
        Wait = 2,
        Start = 3,
        Move,
        Mobgen,
        Mobdestroy,
        End,
        TargetFieldStart,
        TargetFieldShipmoveStart,
        TargetFieldWait,
        TargetFieldMove,
        TargetFieldEnd,
        TargetFieldShipmoveEnd,
    }

    public class ContinentMan
    {
        public static IDictionary<string, CONTIMOVE> Contimoves { get; } = new Dictionary<string, CONTIMOVE>();
        public long LastUpdateTime { get; private set; }

        public static ContinentMan Instance { get; private set; }

        public static void Init()
        {
            var reader = new ConfigReader(Server.Instance.GetConfigPath("Contimove.img"));
            Instance = new ContinentMan();
            foreach (var node in reader.RootNode)
            {
                new CONTIMOVE(node);
            }

            // Normalizing the info
            long currentTime = MasterThread.CurrentTime;
            var cDate = MasterThread.CurrentDate;
            int currentMinute = cDate.Minute;
            foreach (var contimove in Contimoves.Select(i => i.Value))
            {
                int boardingInterval = contimove.TermMin;
                int sections = currentMinute / boardingInterval;
                int nextDeparture = boardingInterval * (sections + 1) + contimove.DelayMin;
                int nextBoarding = nextDeparture - contimove.WaitMin;
                int nextBoardingIn = nextBoarding - currentMinute;

                contimove.NextBoardingTime = currentTime + nextBoardingIn*60000;
                contimove.State = Conti.Dormant;
                contimove.Event?.ResetEvent();
            }

            MasterThread.RepeatingAction.Start(
                "ContinentMan Update",
                Instance.Update,
                0,
                1 * 1000
            );
        }

        public CONTIMOVE FindContiMove(int fieldId)
        {
            foreach (var contimove in Contimoves.Select(i => i.Value))
            {
                if (contimove.FieldStartStation == fieldId ||
                    contimove.FieldsShip[0] == fieldId)
                {
                    return contimove;
                }
            }

            return null;
        }

        public int GetInfo(int fieldId, int flag)
        {
            var cm = FindContiMove(fieldId);
            if (cm == null)
            {
                return 0;
            }

            // Center::ms_bServerCheckup thing here... meh

            if (flag == 1) return cm.Event?.EventInProgress == true ? 1 : 0;
            if (flag == 0) return (byte)cm.State;

            return -1;
        }

        public void MoveField(int fieldFrom, int fieldTo)
        {
            if (!GameDataProvider.Maps.TryGetValue(fieldFrom, out var field)) return;

            foreach (var character in field.Characters.ToArray())
            {
                // Little hack
                if (character.HP == 0)
                {
                    // Dead
                    character.ChangeMap(field.ReturnMap);
                }
                else
                {
                    character.ChangeMap(fieldTo);
                }
            }
        }

        public void OnAllSummonedMobRemoved(int fieldId)
        {
            // Actual bug in GMS here:
            // This does not reset the 'eventdoing' value, thus re-entering the map
            // will show the ballrog ship again
            // Kassy said they fixed it in EMS by not sending this packet at all

            foreach (var contimove in Contimoves.Select(i => i.Value))
            {
                if (contimove.FieldsShip[0] == fieldId)
                {
                    SendContiPacket(fieldId, Conti.TargetFieldMove, Conti.Mobdestroy);
                }
            }

        }

        public static void SendContiPacket(int fieldId, Conti target, Conti flag)
        {
            if (!GameDataProvider.Maps.TryGetValue(fieldId, out var field)) return;

            var packet = new Packet(ServerMessages.CONTI_TARGET_FIELD);
            packet.WriteByte((byte)target);
            packet.WriteByte((byte)flag);

            field.SendPacket(packet);
        }

        public void SetReactorState(int fieldId, string sName, int state)
        {
            // TODO: Implement
        }

        public void Update(long tCur)
        {
            LastUpdateTime = tCur;

            foreach (var contimove in Contimoves.Select(i => i.Value))
            {
                var curState = contimove.GetState();
                switch (curState)
                {
                    case Conti.End:
                        // Finish moving
                        SendContiPacket(
                            contimove.FieldEndStation,
                            Conti.TargetFieldShipmoveEnd,
                            Conti.End
                        );
                        foreach (var field in contimove.FieldsShip)
                        {
                            MoveField(field, contimove.FieldDestination);
                        }

                        if (contimove.ReactorName?.Length > 0)
                        {
                            SetReactorState(contimove.FieldEndStation, contimove.ReactorName, contimove.StateOnEnd);
                        }
                        break;
                        
                    case Conti.Start:
                        SendContiPacket(
                            contimove.FieldStartStation,
                            Conti.TargetFieldShipmoveStart,
                            Conti.Start
                        );
                        MoveField(contimove.FieldWaitingRoom, contimove.FieldsShip[0]);

                        if (contimove.ReactorName?.Length > 0)
                        {
                            SetReactorState(contimove.FieldStartStation, contimove.ReactorName, contimove.StateOnStart);
                        }
                        break;
                }
            }
        }
    }
}
