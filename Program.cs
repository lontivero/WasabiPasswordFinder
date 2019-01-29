using NBitcoin;
using System;
using System.Diagnostics;
using System.Linq;
using System.Security;

namespace WasabiPasswordFinder
{
    internal class Program
    {
        private static char[] chars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789!@$?_- \"#$%&()=".ToArray(); 
        private static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Please provide exactly 2 arguments. Example: dotnet run \"6PYSeErf23ArQL7xXUWPKa3VBin6cuDaieSdABvVyTA51dS4Mxrtg1CpGN\" \"password\"");
                return;
            }

            BitcoinEncryptedSecretNoEC encryptedSecret;
            try
            {
                encryptedSecret = new BitcoinEncryptedSecretNoEC(args[0]);
            }
            catch(FormatException)
            {
                Console.WriteLine("ERROR: The encrypted secret is invalid. Make sure you copied correctly from your wallet file.");
                return;
            }
            var password = args[1];

            Console.WriteLine($"WARNING: This tool will display you password if it finds it. Also, the process status display your wong password chars.");
            Console.WriteLine($"         You can cancel this by CTRL+C combination anytime.");

            var sw = new Stopwatch();
            sw.Start();
            var pwChar = password.ToCharArray();
            for(var i=0; i < pwChar.Length; i++)
            {
                var original = pwChar[i]; 
                Console.WriteLine($"Trying position {i} for char {original}");
                foreach(var c in chars)
                {
                    pwChar[i] = c;
                    var newPassword = new string(pwChar); 
                    try
                    {
                        encryptedSecret.GetKey(newPassword);
                        Console.WriteLine("Password found: " + newPassword);
                        goto end;
                    }
                    catch (SecurityException)
                    {
                    }
                }
                pwChar[i] = original; 
            }
            end:
            sw.Stop();
            Console.WriteLine($"Completed in {sw.Elapsed}");
        }
    }
}
