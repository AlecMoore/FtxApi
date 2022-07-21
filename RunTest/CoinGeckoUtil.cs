using Nancy.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RunTest
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Root
    {
        public List<List<double>> prices { get; set; }
        public List<List<double>> market_caps { get; set; }
        public List<List<double>> total_volumes { get; set; }
    }

    public class CoinGeckoUtil
    {
        public static async Task<int> readPrice()
        {
            HttpClient client = new HttpClient();

            String json = await client.GetStringAsync("https://api.coingecko.com/api/v3/simple/price?ids=bitcoin&vs_currencies=usd");

            var jo = JObject.Parse(json);

            int price = (int)jo["bitcoin"]["usd"];

            return price;
        }

        public static async Task<double[]> getMeanReversionPrices()
        {
            HttpClient client = new HttpClient();

            String json = await client.GetStringAsync("https://api.coingecko.com/api/v3/coins/bitcoin/market_chart?vs_currency=usd&days=1");

            var jo = JObject.Parse(json);

            JavaScriptSerializer js = new JavaScriptSerializer();
            Root root = JsonConvert.DeserializeObject<Root>(json);

            List<List<double>> jsPrices = root.prices;
            double[] prices = new double[jsPrices.Count];
            for (int i = 0; i < jsPrices.Count; i++)
            {
                prices[i] = jsPrices[i][1];
            }

            return prices;
        }

    }
}
