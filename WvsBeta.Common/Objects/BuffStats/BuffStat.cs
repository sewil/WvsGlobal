using System;
using System.Diagnostics;
using WvsBeta.Common.Enums;
using WvsBeta.Common.Sessions;

namespace WvsBeta.Common.Objects.Stats
{
    public class BuffStat
    {
        /// <summary>
        /// Number. Most of the time, this is the X or Y value of the skill/buff
        /// </summary>
        public short N { get; set; }
        /// <summary>
        /// Reference ID. For Item IDs, use a negative number
        /// </summary>
        public int R { get; set; }
        /// <summary>
        /// Expire Time. Extended version of T (full time in millis).
        /// </summary>
        public long TM { get; set; }
        public BuffValueTypes Flag { get; set; }

        public bool IsSet(long? time = null)
        {
            if (N == 0) return false;
            if (time == null) time = MasterThread.CurrentTime;
            return TM > time;
        }

        public BuffValueTypes GetState(long? time = null)
        {
            return IsSet(time) ? Flag : 0;
        }

        public bool HasReferenceId(int referenceId, long? currenTime = null)
        {
            return IsSet(currenTime) && R == referenceId;
        }

        public BuffStat(BuffValueTypes flag)
        {
            Flag = flag;
            N = 0;
            R = 0;
            TM = 0;
        }

        public BuffValueTypes Reset()
        {
            if (R == 0 && N == 0 && TM == 0) return 0;

            Trace.WriteLine($"Removing buff {Flag} {N} {R} {TM}");
            N = 0;
            R = 0;
            TM = 0;
            return Flag;
        }

        public virtual bool TryReset(long currentTime, ref BuffValueTypes flag)
        {
            if (N == 0 || TM >= currentTime) return false;

            flag |= Reset();
            return true;
        }

        public void TryResetByReference(int reference, ref BuffValueTypes flag)
        {
            if (N == 0 || R != reference) return;
            flag |= Reset();
        }

        public virtual BuffValueTypes Set(int referenceId, short nValue, long expireTime)
        {
            // Ignore 0 N-values
            if (nValue == 0) return 0;
            R = referenceId;
            N = nValue;
            TM = expireTime;
            return Flag;
        }

        public void EncodeForRemote(ref BuffValueTypes flag, long currentTime, Action<BuffStat> func, BuffValueTypes specificFlag = BuffValueTypes.ALL)
        {
            if (!IsSet(currentTime) || !specificFlag.HasFlag(Flag)) return;

            flag |= Flag;
            func?.Invoke(this);
        }

        public void EncodeForLocal(Packet pw, ref BuffValueTypes flag, long currentTime, BuffValueTypes specificFlag = BuffValueTypes.ALL)
        {
            if (!IsSet(currentTime) || !specificFlag.HasFlag(Flag)) return;

            flag |= Flag;
            pw.WriteShort(N);
            pw.WriteInt(R);
            pw.WriteShort((short)((TM - currentTime) / 100)); // If its not divided, it will not flash.
        }

        public virtual bool EncodeForCC(Packet pr, ref BuffValueTypes flag, long currentTime)
        {
            if (!IsSet(currentTime)) return false;

            flag |= Flag;
            pr.WriteShort(N);
            pr.WriteInt(R);
            pr.WriteLong(TM);
            return true;
        }

        public virtual bool DecodeForCC(Packet pr, BuffValueTypes flag)
        {
            if (!flag.HasFlag(Flag))
            {
                Reset();
                return false;
            }
            else
            {
                N = pr.ReadShort();
                R = pr.ReadInt();
                TM = pr.ReadLong();
                return true;
            }
        }
    }
}
