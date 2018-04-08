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
                Console.WriteLine(@"1 - Calculate
2 - Show
0 - Exit");

                string menu = Console.ReadLine();
                switch (menu)
                {
                    case "1":
                        market.DownloadMarketData();
                        market.Calculate();
                        break;
                    case "2":
                        Console.WriteLine(market.ShowItems());
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
