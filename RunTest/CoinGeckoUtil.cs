using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RunTest
{
    class CoinGeckoUtil
    {
        public static async Task<int> readPrice()
        {
            HttpClient client = new HttpClient();

            String json = await client.GetStringAsync("https://api.coingecko.com/api/v3/simple/price?ids=bitcoin&vs_currencies=usd");

            var jo = JObject.Parse(json);

            int price = (int)jo["bitcoin"]["usd"];

            return price;
        }

    }
}
