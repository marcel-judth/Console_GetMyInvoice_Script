using Console_GetMyInvoice_Script.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Console_GetMyInvoice_Script
{
    class Program
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            try
            {
                Utile utile = new Utile();

                utile.SaveGetMyInvoices();
            }
            catch (UserInfoException e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
                Thread.Sleep(3000);
                Console.ForegroundColor = ConsoleColor.White;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("An unexpected error happend!");
                Thread.Sleep(3000);
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        private static void ParseArguments(string[] args, out string folder, out string automated)
        {
            folder = "";
            automated = "";
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].ToLower().Equals("-folder") && i + 1 < args.Length)
                    folder = args[i + 1];

                if (args[i].ToLower().Equals("-automated") && i + 1 < args.Length)
                    automated = args[i + 1];
            }

            if (string.IsNullOrEmpty(folder))
                throw new Exception("No -folder argument given!");

        }
    }
}
