using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronOCR.Service
{
    class Helper_FolderFileHandler
    {
        public static string GetDocumentsPath()
        {
            string folderpath_Documents;
            folderpath_Documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            return folderpath_Documents;
        }

        public static Tuple<Boolean,String> CreateFolder(string folderPath)
        {
            if (! System.IO.Directory.Exists(folderPath))
            {
                try
                {
                    System.IO.Directory.CreateDirectory(folderPath);
                    return Tuple.Create(true, "Success");
                }
                catch (Exception exp)
                {
                    return Tuple.Create(true, exp.Message);
                }
            }
            else
            {
                return Tuple.Create(true, "Input folder already exists");
            }
        }

        public static Tuple<Boolean, string> WriteData_TextFile(string outputFilePath, string outputText, string[] array_OutputText, Boolean flag_OutputIsArray, Boolean flag_IfFileExists_Replace)
        {
            try
            {
                if (System.IO.File.Exists(outputFilePath))
                {
                    if(flag_IfFileExists_Replace)
                    {
                        System.IO.File.Delete(outputFilePath);
                        if (flag_OutputIsArray)
                        {
                            System.IO.File.WriteAllLines(outputFilePath, array_OutputText);
                        }
                        else
                        {
                            System.IO.File.WriteAllText(outputFilePath, outputText);
                        }

                    }
                    else
                    {
                        if (flag_OutputIsArray)
                        {
                            System.IO.File.AppendAllLines(outputFilePath, array_OutputText);
                        }
                        else
                        {
                            System.IO.File.AppendAllText(outputFilePath, System.Environment.NewLine + outputText);
                        }
                    }
                }
                else
                {
                    if (flag_OutputIsArray)
                    {
                        System.IO.File.WriteAllLines(outputFilePath, array_OutputText);
                    }
                    else
                    {
                        System.IO.File.WriteAllText(outputFilePath, outputText);
                    }
                }
                return Tuple.Create(true, "Success");
            }
            catch (Exception exp)
            {
                return Tuple.Create(false, exp.Message);
            }
        }
    }
}
