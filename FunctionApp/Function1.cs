using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using FtxApi;
using FtxApi.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;
using RunTest;

namespace FunctionApp
{
    public class Function1
    {

        private static Client client;
        private static FtxRestApi api;
        private static Fng btcFng;
        private static Dictionary<string, CoinBalance> coins;
        private static int price;
        private static decimal cycleAmount;
        private static int buyThreshold = 20;
        private static int sellThreshold = 80;
        private static SideType cycleType;


        private static void setUp()
        {

            client = new Client("", "", "BTC");
            api = new FtxRestApi(client);

        }

        private static async Task fngBotAsync()
        {

            await GetDataAsync();
            await Logic();

        }

        private static double ConvertUSDToBTC(double usdAmount)
        {
            return usdAmount / price;
        }

        private static async Task Logic()
        {

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
                    
                }
                else
                {
                return;    
                }

            var call = await FtxApiUtil.placeOrder(api, "BTC/USD", cycleType, cycleAmount);
        }

        private static async Task GetDataAsync()
        {
            //Console.WriteLine("GetData");
            btcFng = await FngApiUtil.ReadFNG();
            coins = await FtxApiUtil.getCoinBalance(api);
            price = await CoinGeckoUtil.readPrice();
        }

        private static async Task Cycle()
        {
            try
            {
                
                var call = await FtxApiUtil.placeOrder(api, "BTC/USD", cycleType, cycleAmount);

            } catch (Exception ex)
            {

            }
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
