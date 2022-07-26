using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunTest
{
    public class CoinBalance
    {
        public string coin { get; }
        public double total { get; }
        public double free { get; }
        public double availableWithoutBorrow { get; }
        public double usdValue { get; }
        public double spotBorrow { get; }

        public CoinBalance()
        {
            this.coin = "";
            this.total = 0.0;
            this.free = 0.0;
            this.availableWithoutBorrow = 0.0;
            this.usdValue = 0.0;
            this.spotBorrow = 0.0;
        }

        public CoinBalance(String coin, double total, double free, double availableWithoutBorrow, double usdValue, double spotBorrow)
        {
            this.coin = coin;
            this.total = total;
            this.free = free;
            this.availableWithoutBorrow = availableWithoutBorrow;
            this.usdValue = usdValue;
            this.spotBorrow = spotBorrow;
        }


    }
}
