using System;
using System.Collections.Generic;
using System.Text;

namespace Console_GetMyInvoice_Script.Code
{
    class CSVHandler
    {
        public static (string, DateTime) ReadCSVFile(string filename)
        {
            CheckIfFileExists(filename);
            string[] lines = System.IO.File.ReadAllLines(filename);


            string[] columns = lines[1].Split(';');

            string folder = columns[0];

            if (string.IsNullOrEmpty(folder))
                throw new UserInfoException("Folder is not defined in CSV-File!");

            if (!System.IO.Directory.Exists(folder))
                throw new UserInfoException("Folder defined in the CSV-File does not exist!");

            var date = DateTime.MinValue;
            string dateString = columns[1];
            if (!string.IsNullOrEmpty(dateString))
                if (!DateTime.TryParse(dateString, out date))
                    throw new UserInfoException("Invalid Date in CSV-File!");

            return (folder, date);
        }

        internal static void SaveDateToCSVFile(string filename, string folder, string date)
        {
            CheckIfFileExists(filename);
            string[] lines = System.IO.File.ReadAllLines(filename);

            lines[1] = $"{folder};{date}";

            System.IO.File.WriteAllLines(filename, lines);
        }

        private static void CheckIfFileExists(string filename)
        {
            if (!System.IO.File.Exists(filename))
                throw new UserInfoException($"{filename} does not exist!");
        }
    }
}
