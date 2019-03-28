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
            if (args.Length != 3)
            {
                Console.WriteLine("Please provide exactly 2 arguments. Example: dotnet run \"word1 word2 word3.... word12\" \"password\" \"address\"");
                return;
            }

            var seed = args[0];
            var password = args[1];
            var address = args[2];

            Mnemonic mnemonic;
            try
            {
                mnemonic = new Mnemonic(seed);
            }
            catch(FormatException)
            {
                Console.WriteLine("ERROR: The encrypted secret is invalid. Make sure you copied correctly from your wallet file.");
                return;
            }

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
                        var extKey = mnemonic.DeriveExtKey(newPassword);
                        var pubKey = extKey.Derive(new KeyPath("m/84'/0'/0'/0/0")).Neuter().PubKey;

                        var firstAddress = pubKey.GetSegwitAddress(Network.Main).ToString();
                        if(firstAddress == address)
                        {
                            Console.WriteLine("Password found: " + newPassword);
                            goto end;
                        }
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
