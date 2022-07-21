using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using FtxApi;
using FtxApi.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
        //private static readonly IDictionary<string, double> previousValue = new ConcurrentDictionary<string, double>
        //{
        //    { "prevMinPrice", 0.0 },
        //    { "prevMaxPrice", 0.0 }

        //};

        public static async Task Main(string[] args)
        {
            // Create a BlobServiceClient object which will be used to create a container client
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

            //Create a unique name for the container
            string containerName = "meanreversion" + Guid.NewGuid().ToString();

            // Create the container and return a container client object
            BlobContainerClient containerClient = await blobServiceClient.CreateBlobContainerAsync(containerName);

            // Create a local file in the ./data/ directory for uploading and downloading
            string localPath = "./data/";
            string fileName = "quickstart" + Guid.NewGuid().ToString() + ".txt";
            string localFilePath = Path.Combine(localPath, fileName);

            // Write text to the file
            await File.WriteAllTextAsync(localFilePath, "Hello, World!");

            // Get a reference to a blob
            BlobClient blobClient = containerClient.GetBlobClient(fileName);

            Console.WriteLine("Uploading to Blob storage as blob:\n\t {0}\n", blobClient.Uri);

            // Upload data from the local file
            await blobClient.UploadAsync(localFilePath, true);

            Console.WriteLine("Listing blobs...");

            // List all blobs in the container
            await foreach (BlobItem blobItem in containerClient.GetBlobsAsync())
            {
                Console.WriteLine("\t" + blobItem.Name);
            }

            // Download the blob to a local file
            // Append the string "DOWNLOADED" before the .txt extension 
            // so you can compare the files in the data directory
            string downloadFilePath = localFilePath.Replace(".txt", "DOWNLOADED.txt");

            Console.WriteLine("\nDownloading blob to\n\t{0}\n", downloadFilePath);

            // Download the blob's contents and save it to a file
            await blobClient.DownloadToAsync(downloadFilePath);

            Console.WriteLine("wank");
            //try
            //{
            //    double prevPrice = previousValue.GetValueOrDefault("prevMinPrice");
            //    Console.WriteLine(previousValue["prevMinPrice"]);
            //    Console.WriteLine(previousValue["prevMaxPrice"]);
            //}
            //catch { }


            //double[] prices = await CoinGeckoUtil.getMeanReversionPrices();
            ////foreach(double price in prices)
            ////{
            ////    Console.WriteLine(price);
            ////}

            //double minAvg = getMinAverage(prices);
            //Console.WriteLine(minAvg);
            //double maxAvg = getMaxAverage(prices);
            //Console.WriteLine(maxAvg);

            //previousValue["prevMinPrice"] = minAvg;
            //previousValue["prevMaxPrice"] = maxAvg;
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

