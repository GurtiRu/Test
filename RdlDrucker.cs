using System;
using System.Data;
using System.Windows.Controls;
using Syncfusion.ReportWriter;
using System.IO;
using System.Drawing.Printing;
using Syncfusion.Windows.PdfViewer;
using System.Windows.Documents;
using System.Collections.Generic;
using System.Printing;
using Syncfusion.Windows.Reports;

namespace ggPrint
{
    public class RdlDrucker
    {
        public RdlDrucker()
        {

        }
        public void Drucken()
        {
            try
            {
                // string reportPath = Report;
                DataSet dsDaten = new DataSet();
                dsDaten.ReadXml(XmlDatei);
                dsDaten.AcceptChanges();

                ReportWriter reportWriter = new ReportWriter(Report);
                reportWriter.ReportProcessingMode = ProcessingMode.Local;
                reportWriter.DataSources.Clear();
                var rds = new ReportDataSource { Name = "DataSet1", Value = dsDaten.Tables[0] };
                reportWriter.DataSources.Add(rds);
                var gds = reportWriter.GetDataSources();


                MemoryStream stream = new MemoryStream();
                reportWriter.Save(stream, WriterFormat.PDF);
                PdfDocumentView pdfViewer = new PdfDocumentView();
                pdfViewer.Load(stream);
                var doc = pdfViewer.PrintDocument as IDocumentPaginatorSource;
                var docToPrint = doc.DocumentPaginator;

                PrintDialog printDialog = new PrintDialog();
                printDialog.PrintQueue = FindQueue();
                printDialog.PrintDocument(docToPrint, Report);
            }
            catch (Exception Ex)
            {
                Fehler = Ex.ToString();
            }

        }

        private PrintQueue FindQueue()
        {
            if(Druckername == null)
            {
                var pd = new PrintDocument();
                Druckername = pd.PrinterSettings.PrinterName;

            }
            PrintQueue p1 = null;
            //List<string> printersList = new List<string>();
            //List<string> serversList = new List<string>();
            var server = new PrintServer();
            var queues = server.GetPrintQueues(new[] { EnumeratedPrintQueueTypes.Local });
            foreach (var queue in queues)
            {

                if (queue.FullName.ToLower() == Druckername.ToLower())
                {
                    p1 = queue;
                    //if (!serversList.Contains(queue.HostingPrintServer.Name))
                    //{
                    //    serversList.Add(queue.HostingPrintServer.Name);
                    //}
                    //printersList.Add(queue.FullName);
                }
            }
            //            server = new PrintServer(serversList[0].ToString());

            //            PrintQueue printer1 = server.GetPrintQueue(printersList[0].ToString());
            //          return printer1;
            if (p1 == null)
            {
                queues = server.GetPrintQueues(new[] { EnumeratedPrintQueueTypes.Connections });
                var q = from q in queues
                        select 
                foreach (var queue in queues)
                {

                    if (queue.FullName.ToLower() == Druckername.ToLower())
                    {
                        p1 = queue;
                    }
                }
            }


            return p1;
        }

        public string Report { get; set; }
        public string XmlDatei { get; set; }
        public string Fehler { get; private set; }
        public string Druckername { get; set; }
        public int Copies { get; set; }



    }
}