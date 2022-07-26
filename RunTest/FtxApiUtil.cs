using FtxApi;
using FtxApi.Enums;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RunTest
{
    public class FtxApiUtil
    {

        public static async Task<Dictionary<string, CoinBalance>> getCoinBalance(FtxRestApi api)
        {

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

        public static async Task<dynamic> placeOrder(FtxRestApi api, string instrument, SideType side, Decimal amount)
        {
            return await api.PlaceOrderAsync(instrument, side, 0, OrderType.market, amount, false);
        }

    }
}
