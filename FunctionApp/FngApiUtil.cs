using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace RunTest
{
    class FngApiUtil
    {
        public static async Task<Fng> readFNG()
        {
            HttpClient client = new HttpClient();

            String json = await client.GetStringAsync("https://api.alternative.me/fng/");

            var jo = JObject.Parse(json);

            int value = (int)jo["data"][0]["value"];
            string valueClass = jo["data"][0]["value_classification"].ToString();
            long timestamp = (long)jo["data"][0]["timestamp"];
            int timeUntilUpdate = (int)jo["data"][0]["time_until_update"];

            Fng data = new Fng(value, valueClass, timestamp, timeUntilUpdate);

            return data;
        }

    }
}
