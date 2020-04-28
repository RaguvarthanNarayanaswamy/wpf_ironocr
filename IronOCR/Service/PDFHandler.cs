using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IronOCR.Service
{
    class PDFHandler
    {
        public static Tuple<Boolean,string> PDF_NonScanned(string inputFilePath)
        {
            if (System.IO.Path.GetExtension(inputFilePath).ToLower().Trim() == ".pdf")
            {
                try
                {
                    PdfDocument inputDocument = PdfReader.Open(inputFilePath, PdfDocumentOpenMode.Import);
                    return Tuple.Create(false, "Success");
                }
                catch (Exception exp)
                {
                    return Tuple.Create(true, exp.Message);
                }
            }
            else
            {
                return Tuple.Create(false, "Not a PDF");
            }
        }

        public static Tuple<Boolean, string> MergePDF_NonScanned(string outputFilePath, string outputFileName, string[] array_InputFilePath, Boolean flag_OpenPDFAfterMerge)
        {
            try
            {
                string[] files = array_InputFilePath;
                PdfDocument outputDocument = new PdfDocument();
                foreach (string file in files)
                {
                    PdfDocument inputDocument = PdfReader.Open(file, PdfDocumentOpenMode.Import);
                    int count = inputDocument.PageCount;
                    for (int idx = 0; idx < count; idx++)
                    {
                        PdfPage page = inputDocument.Pages[idx];
                        outputDocument.AddPage(page);
                    }
                }
                char[] trimParams = (@" /\").ToCharArray();
                string filename = outputFilePath.Trim(trimParams) + @"\" + System.IO.Path.GetFileNameWithoutExtension(outputFileName) + ".pdf";
                outputDocument.Save(filename);
                if (flag_OpenPDFAfterMerge)
                {
                    Process.Start(filename);
                }
                return Tuple.Create(true, "Success");
            }
            catch (Exception exp)
            {
                return Tuple.Create(false, exp.Message);
            }
        }

        public static Tuple<Boolean, string> SplitPDF_NonScanned(string inputFilePath, string outputFolderPath, string prefixForOutputPDF, Boolean flag_OpenPDFAfterSplit)
        {
            try
            {
                if (System.IO.Path.GetExtension(inputFilePath).ToLower().Trim() == ".pdf")
                {
                    string outputFileName = prefixForOutputPDF.Trim() + System.IO.Path.GetFileNameWithoutExtension(inputFilePath);
                    Tuple<Boolean, string> result_CreateFolder = FolderFileHandler.CreateFolder(outputFolderPath);
                    PdfDocument inputDocument = PdfReader.Open(inputFilePath, PdfDocumentOpenMode.Import);
                    int count = inputDocument.PageCount;
                    List<string> list_OutputFiles = new List<string>();
                    for (int idx = 0; idx < count; idx++)
                    {
                        PdfDocument tempDocument = new PdfDocument();
                        PdfPage page = inputDocument.Pages[idx];
                        tempDocument.AddPage(page);
                        tempDocument.Save(outputFolderPath + @"\" + outputFileName + "_Page" + (idx + 1).ToString() + ".pdf");
                        list_OutputFiles.Add(outputFolderPath + @"\" + outputFileName + "_Page" + (idx + 1).ToString() + ".pdf");
                    }
                    if (flag_OpenPDFAfterSplit)
                    {
                        foreach (string outputFile in list_OutputFiles)
                        { Process.Start(outputFile); }
                    }
                    return Tuple.Create(true, "Success");
                }
                else
                {
                    return Tuple.Create(false, "Input file is not PDF");
                }
            }
            catch (Exception exp)
            {
                return Tuple.Create(false, exp.Message);
            }
        }
        
        public static void SplitPDF(string inputFilePath)
        {

        }
    }
}
