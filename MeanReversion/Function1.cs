using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using FtxApi;
using System.Threading.Tasks;
using RunTest;
using System.Linq;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace MeanReversion
{
    public class Function1
    {
        private static Client client;
        private static FtxRestApi api;
        private static FtxWebSocketApi wsApi;
        //private static readonly FunctionInstanceCache<string[]> Fic = new FunctionInstanceCache<string[]>(PopulateCache);
        private static Dictionary<string, double> previousValue;


        [FunctionName("Function1")]
        public async Task RunAsync([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            double prevPrice = previousValue.GetValueOrDefault("prevMinPrice");
            Console.WriteLine(previousValue);

            double[] prices = await CoinGeckoUtil.getMeanReversionPrices();
            //foreach(double price in prices)
            //{
            //    Console.WriteLine(price);
            //}

            double minAvg = getMinAverage(prices);
            Console.WriteLine(minAvg);
            double maxAvg = getMaxAverage(prices);
            Console.WriteLine(maxAvg);

            previousValue.Add("prevMinPrice", minAvg);
            previousValue.Add("prevMaxPrice", maxAvg);

        }

        private static double getMinAverage(double[] prices)
        {
            double avg = prices.Sum() / prices.Length;
            return avg;
        }

        private static double getMaxAverage(double[] prices)
        {
            double[] maxPrices = new double[prices.Length / 3];
            int j = 0;
            int n = 3;
            for (int i = 0; i < prices.Length; i += n)
            {

                maxPrices[j] = prices[i];
                j++;
            }

            double avg = maxPrices.Sum() / maxPrices.Length;
            return avg;
        }

        //private static string[] PopulateCache()
        //{
        //    return DOSOMETHING HERE;
        //}
    }
}
