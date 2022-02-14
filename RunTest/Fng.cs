using System;
using System.Collections.Generic;

namespace RunTest
{
    class Fng
    {
        public int value { get; }
        public String valueClass { get; }
        public long timestamp { get; }
        public int timeUntilUpdate { get; }

        public Fng()
        {
            this.value = 0;
            this.valueClass = null;
            this.timestamp = 0;
            this.timeUntilUpdate = 0;
        }

        public Fng(int value, String valueClass, long timestamp, int timeUntilUpdate)
        {
            this.value = value;
            this.valueClass = valueClass;
            this.timestamp = timestamp;
            this.timestamp = timestamp;
            this.timeUntilUpdate = timeUntilUpdate;
        }

        public static implicit operator Fng(Dictionary<string, CoinBalance> v)
        {
            throw new NotImplementedException();
        }
    }
}
