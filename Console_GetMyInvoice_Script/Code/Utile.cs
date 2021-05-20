using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;

namespace Console_GetMyInvoice_Script.Code
{
    class Utile
    {
        readonly RESTHandler restHandler = new RESTHandler();

        public void SaveGetMyInvoices()
        {
            string settingsFilePath = Environment.CurrentDirectory + "\\settings.csv";

            Console.WriteLine($"loading data...");
            (string folder, var date) = CSVHandler.ReadCSVFile(settingsFilePath);

            if (date.Date == DateTime.Now.AddDays(-1).Date)
                throw new UserInfoException("No new documents available!");

            List<Document> allDocuments = GetDocumentsFromRESTService(date);
            Console.WriteLine($"Documents: {allDocuments.Count} found.");

            Console.WriteLine("Parsing documents...");
            List<DocumentContent> docContents = GetInvoicesUid(allDocuments);

            SaveFileContentAsPDF(docContents, folder);
            CSVHandler.SaveDateToCSVFile(settingsFilePath, folder, DateTime.Now.AddDays(-1).ToString("dd.MM.yyyy"));
        }

        public List<Document> GetDocumentsFromRESTService(DateTime startDate)
        {
            var filter = new
            {
                documentTypeFilter = "INCOMING_INVOICE",
                startDateFilter = startDate.ToString("yyyy-MM-dd"),
                endDateFilter = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd")
            };

            string jsonFilter = JsonSerializer.Serialize(filter);

            var task = restHandler.GetDocumentList(jsonFilter);

            task.Wait();

            return JsonSerializer.Deserialize<DocumentList>(task.Result)?.records?.ToList();
        }


        public List<DocumentContent> GetInvoicesUid(List<Document> records)
        {
            List<DocumentContent> documents = new List<DocumentContent>();
            //includeDocument to get base64 of pdf document
            string jsonFilter = JsonSerializer.Serialize(new
            {
                includeDocument = true,
            });

            foreach (Document record in records)
            {
                //get document details from resthandler
                var task = restHandler.GetDocument(record.documentUid, jsonFilter);
                task.Wait();
                DocumentContent obj = JsonSerializer.Deserialize<DocumentContent>(task.Result);
                documents.Add(obj);
            }


            return documents;
        }

        public void SaveFileContentAsPDF(List<DocumentContent> fileContents, string folderpath)
        {
            foreach (DocumentContent docContent in fileContents)
            {
                byte[] sPDFDecoded = Convert.FromBase64String(docContent.fileContent);
                //Console.WriteLine($"Documents: {folderpath}\\{docContent.meta_data.filename}");

                File.WriteAllBytes(folderpath + @$"\{docContent.meta_data.filename}", sPDFDecoded);
            }
        }
    }


    public class DocumentContent
    {
        public Meta_Data meta_data { get; set; }
        public string fileContent { get; set; }
    }

    public class Meta_Data
    {
        public int documentUid { get; set; }
        public string createdAt { get; set; }
        public int companyUid { get; set; }
        public string documentType { get; set; }
        public string documentNumber { get; set; }
        public string documentDate { get; set; }
        public string documentDueDate { get; set; }
        public float netAmount { get; set; }
        public float vat { get; set; }
        public object[] taxRates { get; set; }
        public float grossAmount { get; set; }
        public string currency { get; set; }
        public bool isArchived { get; set; }
        public bool isLocked { get; set; }
        public int isOcrCompleted { get; set; }
        public object[] tags { get; set; }
        public string note { get; set; }
        public string source { get; set; }
        public string sourceUser { get; set; }
        public string filename { get; set; }
        public string fileSize { get; set; }
        public string paymentStatus { get; set; }
        public string paymentMethod { get; set; }
        public string attachments { get; set; }
        public object supplierUid { get; set; }
        public Paymentdetails paymentDetails { get; set; }
    }

    public class Paymentdetails
    {
        public string purpose_of_usage { get; set; }
        public string iban { get; set; }
        public string bic { get; set; }
        public string account_holder_name { get; set; }
        public string account_number { get; set; }
        public string bank_name { get; set; }
        public string bank_address { get; set; }
        public string sort_code { get; set; }
        public string routing_number { get; set; }
        public string ifsc_code { get; set; }
        public string routing_code { get; set; }
        public object cash_discount_date { get; set; }
        public int cash_discount_value { get; set; }
    }


    public class DocumentList
    {
        public Document[] records { get; set; }
        public string totalCount { get; set; }
        public int maxAmount { get; set; }
        public int offset { get; set; }
    }

    public class Document
    {
        public int documentUid { get; set; }
        public string createdAt { get; set; }
        public int companyUid { get; set; }
        public string documentType { get; set; }
        public string documentNumber { get; set; }
        public string documentDate { get; set; }
        public string documentDueDate { get; set; }
        public float netAmount { get; set; }
        public int vat { get; set; }
        public object[] taxRates { get; set; }
        public float grossAmount { get; set; }
        public string currency { get; set; }
        public bool isArchived { get; set; }
        public bool isLocked { get; set; }
        public int isOcrCompleted { get; set; }
        public object[] tags { get; set; }
        public string note { get; set; }
        public string source { get; set; }
        public string filename { get; set; }
        public string fileSize { get; set; }
        public string paymentStatus { get; set; }
        public string paymentMethod { get; set; }
        public object supplierUid { get; set; }
        public Paymentdetails paymentDetails { get; set; }
    }

}
