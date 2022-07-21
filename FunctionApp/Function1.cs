using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using FtxApi;
using FtxApi.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RunTest;

namespace FunctionApp
{
    public class Function1
    {

        private static Client client;
        private static FtxRestApi api;
        private static FtxWebSocketApi wsApi;
        private static Fng btcFng;
        private static Dictionary<string, CoinBalance> coins;
        private static int price;
        //private static int cycleNumber = 0;
        //private static Boolean cycleActive = false;
        private static decimal cycleAmount;
        private static int buyThreshold = 20;
        private static int sellThreshold = 80;
        //private static int weekTimer = 10000;
        private static SideType cycleType;



        //public static async Task Main(string[] args)
        //{
        //    setUp();
        //    await fngBotAsync();
        //}

        private static void setUp()
        {
            //Console.WriteLine("Setup");
            client = new Client();
            api = new FtxRestApi(client);
            //wsApi = new FtxWebSocketApi("wss://ftx.com/ws/");
        }

        private static async Task fngBotAsync()
        {
            //Console.WriteLine("BotStart");

            await GetDataAsync();
            //printTotals(btcFng, coins, price);
            await Logic();

        }


        private static void printTotals(Fng btcFng, Dictionary<string, CoinBalance> coins, int price)
        {
            Console.WriteLine("print");
            Console.WriteLine(price);
            Console.WriteLine(" ");

            Console.WriteLine(btcFng.value);
            Console.WriteLine(btcFng.valueClass);
            Console.WriteLine(" ");

            Console.WriteLine(coins["BTC"].coin);
            Console.WriteLine(coins["BTC"].availableWithoutBorrow);
            Console.WriteLine(coins["BTC"].usdValue);
            Console.WriteLine(" ");

            Console.WriteLine(coins["USD"].coin);
            Console.WriteLine(coins["USD"].availableWithoutBorrow);
            Console.WriteLine(coins["USD"].usdValue);
            Console.WriteLine(" ");

            //Console.WriteLine(cycleAmount);
            //Console.WriteLine(cycleNumber);
            Console.WriteLine(cycleType);
            Console.WriteLine(" ");
        }

        private static double ConvertUSDToBTC(double usdAmount)
        {
            return usdAmount / price;
        }

        private static async Task Logic()
        {
            //Console.WriteLine("event");
            //if (cycleActive == false)
            //{
            //   // Console.WriteLine("inactive");
            //    cycleNumber = 0;

                if (btcFng.value <= buyThreshold)
                {
                    cycleType = SideType.buy;
                    cycleAmount = (decimal)ConvertUSDToBTC(coins["USD"].free / 4.0); //need to convert to BTC amount
                    //await Cycle();
                }
                else if (btcFng.value >= sellThreshold)
                {
                    cycleType = SideType.sell;
                    cycleAmount = (decimal)(coins["BTC"].free / 10.0);
                    //await Cycle();
                }
                else
                {
                return;
                    //await Task.Delay(btcFng.timeUntilUpdate);
                }

            var call = await FtxApiUtil.placeOrder(api, "BTC/USD", cycleType, cycleAmount);
            //}
            //else
            //{
            //   // Console.WriteLine("active");
            //    await Cycle();
            //}
        }

        private static async Task GetDataAsync()
        {
            //Console.WriteLine("GetData");
            btcFng = await FngApiUtil.readFNG();
            coins = await FtxApiUtil.getCoinBalance(api);
            price = await CoinGeckoUtil.readPrice();
        }

        private static async Task Cycle()
        {
            //cycleActive = true;
            //if (cycleNumber < 4)
            //{
            //    ++cycleNumber;
            //Console.WriteLine("<4");

            try
            {
                
                var call = await FtxApiUtil.placeOrder(api, "BTC/USD", cycleType, cycleAmount);

            } catch (Exception ex)
            {

            }

            //Console.WriteLine(call);
            //Console.WriteLine(" ");
            //await Task.Delay(weekTimer);
            //}
            //else
            //{
            //    if ((btcFng.value <= buyThreshold && cycleType == SideType.buy) || (btcFng.value >= sellThreshold && cycleType == SideType.sell))
            //    {
            //        Console.WriteLine(">4");
            //        if (cycleType == SideType.buy)
            //        {
            //            cycleAmount = (decimal)ConvertUSDToBTC(coins["USD"].free / 4.0);
            //        }
            //        else
            //        {
            //            cycleAmount = (decimal)(coins["BTC"].free / 10.0);
            //        }

            //        var call = await FtxApiUtil.placeOrder(api, "BTC/USD", SideType.buy, cycleAmount);
            //        Console.WriteLine(call);
            //        Console.WriteLine(" ");
            //        //await Task.Delay(weekTimer);
            //    }
            //    else
            //    {
            //        cycleAmount = 0;
            //        //cycleNumber = 0;
            //        //cycleActive = false;
            //        //await Task.Delay(btcFng.timeUntilUpdate);
            //    }
            //}
        }

        [FunctionName("Function1")]
        public async Task Run([TimerTrigger("0 0 6 * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            setUp();
            log.LogInformation("setup done");
            await fngBotAsync();
            log.LogInformation("run done");
        }
    }
}
