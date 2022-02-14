using FtxApi;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RunTest
{
    class FtxApi
    {
        private static Client client;
        private static FtxRestApi api;
        private static FtxWebSocketApi wsApi;



        private static void setUp()
        {
            client = new Client();
            api = new FtxRestApi(client);
            wsApi = new FtxWebSocketApi("wss://ftx.com/ws/");
        }

        public static async Task<Dictionary<string, CoinBalance>> getCoinBalance()
        {
            setUp();

            var json = api.GetBalancesAsync().Result;

            var jo = JObject.Parse(json);

            int numOfCoins = jo["result"].Count;

            Dictionary<string, CoinBalance> coins = new Dictionary<string, CoinBalance>();

            for (int i = 0;  i < numOfCoins; i++)
            {
                string coin = jo["result"][i]["coin"].ToString();
                double total = (double)jo["result"][i]["total"];
                double free = (double)jo["result"][i]["free"];
                double availableWithoutBorrow = (double)jo["result"][i]["availableWithoutBorrow"];
                double usdValue = (double)jo["result"][i]["usdValue"];
                double spotBorrow = (double)jo["result"][i]["spotBorrow"];

                CoinBalance balance = new CoinBalance(coin, total, free, availableWithoutBorrow, usdValue, spotBorrow);

                coins.Add(balance.coin, balance);
            }

            return coins;
        }


    }
}
