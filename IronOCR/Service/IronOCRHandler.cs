using System;
using System.Windows;
using IronOcr;

namespace IronOCR.Service
{
    class IronOCRHandler
    {
        public static void SimpleRead(string inputFilePath)
        {
            var Ocr = new AutoOcr();
            var Result = Ocr.Read(inputFilePath);
            //MessageBox.Show(Result.Text);
            //Console.WriteLine(Result.Text);
            Helper_FolderFileHandler.WriteData_TextFile(@"C:\Users\Raguvarthan\Documents\Symprio Projects\IronOCR\Test\1.txt",Result.Text,null,false,false);
        }
    }
}
