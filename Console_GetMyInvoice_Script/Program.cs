using Console_GetMyInvoice_Script.Code;
using System;
using System.Collections.Generic;
using System.Linq;

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
                ParseArguments(args, out var folder);
                utile.SaveGetMyInvoices(folder);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        private static void ParseArguments(string[] args, out string folder)
        {
            folder = "";
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].ToLower().Equals("-folder") && i + 1 < args.Length)
                    folder = args[i + 1];
            }

            if (string.IsNullOrEmpty(folder))
                throw new Exception("No -folder argument given!");

        }
    }
}
