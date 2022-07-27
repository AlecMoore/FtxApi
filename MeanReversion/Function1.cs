using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using FtxApi;
using System.Threading.Tasks;
using RunTest;
using System.Linq;
using System.Collections.Generic;
using FtxApi.Enums;
using Azure.Storage.Blobs;
using System.IO;
using Nancy.Json;
using Newtonsoft.Json;
using System.Text;

namespace MeanReversion
{
    public class Function1
    {
        private static Client client;
        private static FtxRestApi api;
        private static string connectionString = "DefaultEndpointsProtocol=https;AccountName=meanreversion;AccountKey=Tu4fK/4UxxDOmDYH15L7mDtVGa42ZXBEBb+6j91RckgDT14SMSsCnYg8mqnf8+hgLQHsg22QpAip+AStmyjYJw==;EndpointSuffix=core.windows.net";
        private static SideType sideType;
        private static Dictionary<string, CoinBalance> coins;
        private static decimal tradeAmount;
        private static BlobServiceClient blobServiceClient;
        private static string fileName = "MRPrices.json";
        private static BlobContainerClient containerClient;
        private static BlobClient blobClient;


        [FunctionName("Function1")]
        public async Task RunAsync([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            setUp();

            MeanReversionPrices previousPrices = await GetPreviousPrices();
            log.LogInformation(previousPrices.minPrice.ToString());
            log.LogInformation(previousPrices.maxPrice.ToString());

            MeanReversionPrices currentPrices = await GetCurrentPrices();
            log.LogInformation(currentPrices.minPrice.ToString());
            log.LogInformation(currentPrices.maxPrice.ToString());

            await Trade(previousPrices, currentPrices);

            await UploadJson(currentPrices);

            log.LogInformation("fin");
        }

        private static void setUp()
        {
            client = new Client("1WY1Vh0nx_GOwnTeGpVVVDJqhtwLm9tvy3MTzJ4G", "91l7qazicFZvWZH7jFh-dGxEPc6ZhbbmwmHpcSWd", "MR");
            api = new FtxRestApi(client);

            // Create a BlobServiceClient object which will be used to create a container client
            blobServiceClient = new BlobServiceClient(connectionString);

            //Create a unique name for the container
            string containerName = "meanreversion";

            // Create the container and return a container client object
            containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            // Get a reference to a blob
            blobClient = containerClient.GetBlobClient(fileName);
        }

        public static async Task<MeanReversionPrices> GetPreviousPrices()
        {

            MeanReversionPrices previousPrices = new MeanReversionPrices();
            if (await blobClient.ExistsAsync())
            {
                var response = await blobClient.DownloadAsync();
                using (var streamReader = new StreamReader(response.Value.Content))
                {
                    string json = streamReader.ReadToEnd();
                    previousPrices = JsonConvert.DeserializeObject<MeanReversionPrices>(json);
                }
            }

            return previousPrices;
        }

        public static async Task<MeanReversionPrices> GetCurrentPrices()
        {
            double[] prices = await CoinGeckoUtil.getMeanReversionPrices();

            MeanReversionPrices currentPrices = new MeanReversionPrices()
            {
                minPrice = 0.0,
                maxPrice = 0.0
            };

            currentPrices.minPrice = getMinAverage(prices);

            currentPrices.maxPrice = getMaxAverage(prices);

            return currentPrices;
        }

        public static async Task Trade(MeanReversionPrices previousPrices, MeanReversionPrices currentPrices)
        {
            coins = await FtxApiUtil.getCoinBalance(api);

            if (currentPrices.minPrice > currentPrices.maxPrice && previousPrices.minPrice < previousPrices.maxPrice)
            {
                sideType = SideType.sell;
                tradeAmount = (decimal)(coins["BTC"].free);
                await FtxApiUtil.placeOrder(api, "BTC/USD", sideType, tradeAmount);
            }
            else if (currentPrices.minPrice < currentPrices.maxPrice && previousPrices.minPrice > previousPrices.maxPrice)
            {
                sideType = SideType.buy;
                tradeAmount = (decimal)(coins["USD"].free / currentPrices.minPrice);
                await FtxApiUtil.placeOrder(api, "BTC/USD", sideType, tradeAmount);
            }
        }

        public static async Task UploadJson(MeanReversionPrices currentPrices)
        {
            var json = new JavaScriptSerializer().Serialize(currentPrices);
            Console.WriteLine(json);

            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                await blobClient.UploadAsync(ms, true);
            }
        }

        private static double getMinAverage(double[] prices)
        {
            double avg = prices.Sum() / prices.Length;
            return avg;
        }

        private static double getMaxAverage(double[] prices)
        {
            double[] maxPrices = prices.Where((value, index) => index % 3 == 0).ToArray();

            double avg = maxPrices.Sum() / maxPrices.Length;
            return avg;
        }

    }
}
