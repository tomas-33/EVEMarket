namespace TH.EveMarket.Console
{
    using System;
    using TH.EveMarket.Library;

    public class Program
    {
        static void Main(string[] args)
        {
            Market market = new Market();

            bool exit = false;
            while (!exit)
            {
                Console.WriteLine(@"1 - Load and calculate
2 - Show
3 - Test EveOnlineApi
4 - Test
0 - Exit");

                string menu = Console.ReadLine();
                switch (menu)
                {
                    case "1":
                        market.LoadData();
                        market.DownloadMarketData();
                        market.Calculate();
                        break;
                    case "2":
                        Console.WriteLine(market.ShowItems());
                        break;
                    case "3":
                        market.TestEveOnlineApi();
                        break;
                    case "4":
                        market.Test();
                        break;
                    case "0":
                        exit = true;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
