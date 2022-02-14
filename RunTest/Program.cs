using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RunTest
{
    class Program
    {

        public static async Task Main(string[] args)
        {

            Fng btcFng = await FngApi.readFNG();
            Dictionary<string, CoinBalance> coins = await FtxApi.getCoinBalance();

            Console.WriteLine(btcFng.value);
            Console.WriteLine(btcFng.valueClass);
            Console.WriteLine(btcFng.timestamp);
            Console.WriteLine(btcFng.timeUntilUpdate);

            Console.WriteLine(coins["BTC"].coin);
            Console.WriteLine(coins["BTC"].total);
            Console.WriteLine(coins["BTC"].free);
            Console.WriteLine(coins["BTC"].availableWithoutBorrow);
            Console.WriteLine(coins["BTC"].usdValue);
            Console.WriteLine(coins["BTC"].spotBorrow);

            Console.WriteLine(coins["USD"].coin);
            Console.WriteLine(coins["USD"].total);
            Console.WriteLine(coins["USD"].free);
            Console.WriteLine(coins["USD"].availableWithoutBorrow);
            Console.WriteLine(coins["USD"].usdValue);
            Console.WriteLine(coins["USD"].spotBorrow);



        }
    }


}
