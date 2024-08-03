using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using FtxApi;
using FtxApi.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nancy.Json;
using Newtonsoft.Json;
using NUnit.Framework.Internal;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunTest
{

    class Program
    {

        private static Client client;
        private static FtxRestApi api;
        private static FtxWebSocketApi wsApi;
        private static string connectionString = "DefaultEndpointsProtocol=https;AccountName=;AccountKey=;EndpointSuffix=core.windows.net";
        private static SideType sideType;
        private static Dictionary<string, CoinBalance> coins;
        private static decimal tradeAmount;
        private static BlobServiceClient blobServiceClient;
        private static string localPath = "./data/";
        private static string fileName = "MRPrices.json";
        private static string localFilePath = Path.Combine(localPath, fileName);
        private static BlobContainerClient containerClient;
        private static BlobClient blobClient;

        //Set up on azure and monitor. (wed)


        private static void setUp()
        {
            client = new Client("", "", "MR");
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


        public static async Task Main(string[] args)
        {
            setUp();

            MeanReversionPrices previousPrices = await GetPreviousPrices();
            Console.WriteLine(previousPrices.minPrice);
            Console.WriteLine(previousPrices.maxPrice);

            MeanReversionPrices currentPrices = await GetCurrentPrices();
            Console.WriteLine(currentPrices.minPrice);
            Console.WriteLine(currentPrices.maxPrice);

            await Trade(previousPrices, currentPrices);

            await UploadJson(currentPrices);

            Console.WriteLine("wank");

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
                    Console.WriteLine(previousPrices.maxPrice);
                    Console.WriteLine(previousPrices.minPrice);
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

