using System;
using System.Linq;
using WvsBeta.Common;
using WvsBeta.Common.Extensions;

namespace WvsBeta.Game.Handlers.Contimove
{
    public class CONTIMOVE
    {
        public string Name { get; set; }
        public int FieldWaitingRoom { get; set; }
        private int[] _fieldsShip;
        public int[] FieldsShip => _fieldsShip;
        public int FieldStartStation { get; set; }
        public int FieldEndStation { get; set; }
        public int FieldDestination { get; set; }
        public Conti State { get; set; }
        public IContiEvent Event { get; set;  }
        public int WaitMin { get; set; }
        public int TravelMin { get; set; }
        public string ReactorName { get; set; }
        public int StateOnStart { get; set; }
        public int StateOnEnd { get; set; }
        public long NextBoardingTime { get; set; }
        public int TermMin => WaitMin + TravelMin;
        public int DepartureMin { get; private set; }
        public long DepartingTime { get; private set; }
        public long ArrivalTime { get; private set; }
        public int DelayMin { get; set; }

        public CONTIMOVE(ConfigReader.Node node)
        {
            Name = node.Name;
            ContinentMan.Contimoves[Name] = this;
            _fieldsShip = new int[0];
            foreach (var subNode in node)
            {
                switch (subNode.Name)
                {
                    case "FieldStartStation":
                        FieldStartStation = subNode.GetInt();
                        break;
                    case "FieldEndStation":
                        FieldEndStation = subNode.GetInt();
                        break;
                    case "FieldWaitingRoom":
                        FieldWaitingRoom = subNode.GetInt();
                        break;
                    case "FieldShip":
                        foreach (var shipNode in subNode)
                        {
                            if (int.TryParse(shipNode.Name, out int idx))
                            {
                                if (idx >= _fieldsShip.Length)
                                    Array.Resize(ref _fieldsShip, idx + 1);
                                _fieldsShip[idx] = shipNode.GetInt();
                            }
                        }
                        break;
                    case "FieldDestination":
                        FieldDestination = subNode.GetInt();
                        break;
                    case "WaitMin":
                        WaitMin = subNode.GetInt();
                        break;
                    case "TravelMin":
                        TravelMin = subNode.GetInt();
                        break;
                    case "MobGenEvent":
                        Event = new MobGenEvent(
                            this,
                            subNode["mobGenItem"].GetInt(),
                            new Pos(subNode["spawnX"].GetShort(), subNode["spawnY"].GetShort()),
                            subNode["spawnChance"].GetInt(),
                            subNode["spawnWithinMin"].GetInt(),
                            subNode["spawnMinOffset"].GetInt()
                        );
                        break;
                    case "DelayMin":
                        DelayMin = subNode.GetInt();
                        break;
                    case "DepartureMin":
                        DepartureMin = subNode.GetInt();
                        break;
                    default: break;
                }
            }
        }

        public Conti GetState()
        {
            long systemTime = MasterThread.CurrentTime;
            switch (State)
            {
                case Conti.Dormant:
                    if (systemTime - NextBoardingTime >= 0)
                    {
                        DepartingTime = TimeAddMins(NextBoardingTime, WaitMin);
                        Program.MainForm.LogDebug($"[{Name}:WAIT] DEPARTING AT {DepartingTime.DateFromMillis().TimeOfDay}");
                        State = Conti.Wait;
                        ArrivalTime = TimeAddMins(NextBoardingTime, TermMin);
                        NextBoardingTime = TimeAddMins(NextBoardingTime, DepartureMin);
                    }
                    break;

                case Conti.Wait:
                    if (systemTime - DepartingTime >= 0)
                    {
                        Program.MainForm.LogDebug($"[{Name}:MOVE] ARRIVAL AT {ArrivalTime.DateFromMillis().TimeOfDay}");
                        State = Conti.Move;
                        // not an error
                        return Conti.Start;
                    }
                    break;

                case Conti.Move:
                    {
                        if (Event?.HandleMove(systemTime) == true) { break; }

                        // Check if we have to end the boat trip
                        if (systemTime - ArrivalTime >= 0)
                        {
                            Program.MainForm.LogDebug($"[{Name}:DORMANT] ARRIVED, NEXT BOARDING AT {NextBoardingTime.DateFromMillis().TimeOfDay}");
                            Event?.ResetEvent();
                            State = Conti.Dormant;
                            return Conti.End;
                        }
                        break;
                    }
            }

            return State;
        }

        public static long TimeAddMins(long time, int mins)
        {
            return time + (mins * 60000);
        }
    }
}
