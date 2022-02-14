using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace RunTest
{
    class FngApi
    {
        public static async Task<Fng> readFNG()
        {
            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

            var stringTask = client.GetStringAsync("https://api.alternative.me/fng/");
            String json = await stringTask;

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
