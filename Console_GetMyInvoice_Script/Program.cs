using Console_GetMyInvoice_Script.Code;
using System;

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
                utile.SaveGetMyInvoices(@".\");
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }
    }
}
