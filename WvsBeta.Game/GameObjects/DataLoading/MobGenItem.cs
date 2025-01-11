using System;
using WvsBeta.Common;

namespace WvsBeta.Game
{
    public class MobGenItem
    {
        public int ID { get; private set; }
        public int RegenInterval { get; set; }
        public long RegenAfter { get; set; }
        public short Foothold { get; private set; }
        public bool FacesLeft { get; private set; }
        public int MobCount { get; set; }

        private readonly Life _life;
        private bool _initializedYAxis;
        private short _y, _cy;
        public short X { get; private set; }

        public short Y
        {
            get
            {
                if (_initializedYAxis) return _y;

                MobData md;
                if (GameDataProvider.Mobs.TryGetValue(ID, out md) == false)
                {
                    Console.WriteLine($"Invalid mob template ID({ID})");
                    return -1;
                }

                // Flying mobs use CY value
                //if (md.Flies)
                    _y = _cy;
                _initializedYAxis = true;
                return _y;
            }
        }

        public MobGenItem(Life life, long? currentTime)
        {
            _life = life;
            Reset();
        }

        public void Reset()
        {
            ID = _life.ID;
            Foothold = (short)_life.Foothold;
            FacesLeft = _life.FacesLeft;
            X = _life.X;
            _initializedYAxis = false;
            _y = _life.Y;
            _cy = _life.Cy;
            MobCount = 0;
            RegenInterval = _life.RespawnTime * 1000;

            //if (currentTime == null) currentTime = MasterThread.CurrentTime;
            //var regenInterval = RegenInterval = life.RespawnTime * 1000;
            //if (regenInterval >= 0)
            //{
            //    var T1 = regenInterval / 10;
            //    var T2 = 6 * regenInterval / 10;

            //    RegenAfter = currentTime.Value;
            //    if (T2 != 0)
            //        RegenAfter += T1 + Rand32.Next() % T2;
            //    //else
            //    //    RegenAfter += Rand32.Next();
            //}
            //else
            //{
            //    RegenAfter = 0;
            //}
        }
    }
}
