using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace gamr
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 3)
            {
                Console.WriteLine("You need to enter an odd number of items >=3,\ngamr paper rock scissors");
                return;
            }

            if (args.Length % 2 == 0)
            {
                Console.WriteLine("You need to enter an odd number of items >=3,\ngamr paper rock scissors");
                return;
            }
            
            var compData = Comp(args.Length);
            Console.WriteLine($"HMAC: {compData[0]}");
            for (int i = 0; i < args.Length; i++)
            {
                Console.WriteLine($"{i+1} - {args[i]}");
            }
            
            int chose;
            while (true)
            {
                Console.Write("Enter your move: ");
                 chose = Int32.Parse(Console.ReadLine() ?? throw new InvalidOperationException());
                if (chose == 0)
                    return;
                if (chose > args.Length || chose < 0)
                    Console.WriteLine($"Please enter a number between 0 and {args.Length}");
                else
                    break;
            }
            
            Console.WriteLine("Your move: " + args[chose-1] + "\nComp move: "+args[Convert.ToInt32(compData[1])] + "  HEX: "+compData[3]);
            Console.WriteLine( chose switch
                {
                    _ when chose-1 == Convert.ToInt32(compData[1]) => "draw",
                    _ when IsWinner(chose, args[Convert.ToInt32(compData[1])], args) => "Player is win",
                    _ => "Player is lose"
                }
            );
            Console.WriteLine($"HMAC key: {compData[2]}");
        }

        private static string[] Comp(int len)
        {
            Random r = new Random();
            int msg = r.Next(1, len);
            byte[] key = new byte[16];
            var rng = new RNGCryptoServiceProvider();
            rng.GetBytes(key);
            var hash = new HMACSHA256(key);
            return new[] {Convert.ToHexString(hash.ComputeHash(BitConverter.GetBytes(msg))),""+msg, Convert.ToHexString(key), Convert.ToHexString(BitConverter.GetBytes(msg))};
        }

        private static bool IsWinner(int pChose, string cChose, string[] argsArr)
        {
            List<string> winnerElectionsList = new List<string>();
            for (int i = pChose, j = 0; j < argsArr.Length/2;i++, j++)
            {
                if (i >= argsArr.Length)
                    i = 0;
                winnerElectionsList.Add(argsArr[i]);
            }
            return winnerElectionsList.Any(x => x.Equals(cChose));
        }
    }
}