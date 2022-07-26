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
using System.Threading.Tasks;

namespace RunTest
{

    class Program
    {

        private static Client client;
        private static FtxRestApi api;
        private static FtxWebSocketApi wsApi;
        private static string connectionString = "DefaultEndpointsProtocol=https;AccountName=meanreversion;AccountKey=Tu4fK/4UxxDOmDYH15L7mDtVGa42ZXBEBb+6j91RckgDT14SMSsCnYg8mqnf8+hgLQHsg22QpAip+AStmyjYJw==;EndpointSuffix=core.windows.net";
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
            Console.WriteLine("\nDownloading blob to\n\t{0}\n", localFilePath);

            // Download the blob's contents and save it to a file
            await blobClient.DownloadToAsync(localFilePath);

            MeanReversionPrices previousPrices = LoadJson(localFilePath);

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

            // Write text to the file
            await File.WriteAllTextAsync(localFilePath, json);

            Console.WriteLine("Uploading to Blob storage as blob:\n\t {0}\n", blobClient.Uri);

            // Upload data from the local file
            await blobClient.UploadAsync(localFilePath, true);
        }

        public static MeanReversionPrices LoadJson(string filePath)
        {
            using (StreamReader r = new StreamReader(filePath))
            {
                string json = r.ReadToEnd();
                MeanReversionPrices items = JsonConvert.DeserializeObject<MeanReversionPrices>(json);
                return items;
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
















    //// Program.cs
    //class Program
    //{
    //    static void Main(string[] args)
    //    {
    //        // Setup Host
    //        var host = CreateDefaultBuilder().Build();

    //        // Invoke Worker
    //        using IServiceScope serviceScope = host.Services.CreateScope();
    //        IServiceProvider provider = serviceScope.ServiceProvider;
    //        var workerInstance = provider.GetRequiredService<Worker>();
    //        workerInstance.DoWork();

    //        host.Run();
    //    }

    //    static IHostBuilder CreateDefaultBuilder()
    //    {
    //        return Host.CreateDefaultBuilder()
    //            .ConfigureAppConfiguration(app =>
    //            {
    //                app.AddJsonFile("appsettings.json");
    //            })
    //            .ConfigureServices(services =>
    //            {
    //                services.AddSingleton<Worker>();
    //            });
    //    }
    //}

    //// Worker.cs
    //internal class Worker
    //{
    //    private readonly IConfiguration configuration;

    //    public Worker(IConfiguration configuration)
    //    {
    //        this.configuration = configuration;
    //    }

    //    public void DoWork()
    //    {
    //        var keyValuePairs = configuration.AsEnumerable().ToList();
    //        Console.ForegroundColor = ConsoleColor.Green;
    //        Console.WriteLine("==============================================");
    //        Console.WriteLine("Configurations...");
    //        Console.WriteLine("==============================================");
    //        foreach (var pair in keyValuePairs)
    //        {
    //            Console.WriteLine($"{pair.Key} - {pair.Value}");
    //        }
    //        Console.WriteLine("==============================================");
    //        Console.ResetColor();
    //    }
    //}
}

