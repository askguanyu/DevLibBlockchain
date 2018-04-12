using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DevLib.Blockchain;

namespace DevLib.Blockchain.DebugConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start...");

            var myWalletAddress = "myWalletAddress";
            var random = new Random();
            var myCoin = new BlockChain();

            Task.Run(() => 
            {
                Console.WriteLine("Starting transactions...");

                while (true)
                {
                    myCoin.CreateTransaction(new Transaction(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), random.Next(1, int.MaxValue)));

                    Thread.Sleep(1000);
                }
            });


            Task.Run(() =>
            {
                Console.WriteLine("Starting the miner...");

                while (true)
                {
                    myCoin.MinePendingTransactions(myWalletAddress);
                    Console.WriteLine($"Balance of my wallet is {myCoin.GetBalanceOfAddress(myWalletAddress)}");

                    Thread.Sleep(2000);
                }
            });

            Console.WriteLine("Done!");
            Console.ReadLine();
        }
    }
}
