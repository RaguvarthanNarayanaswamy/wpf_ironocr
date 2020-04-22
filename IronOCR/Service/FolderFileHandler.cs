using System;
using System.Threading.Tasks;

namespace IronOCR.Service
{
    class FolderFileHandler
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

        public static Tuple<Boolean, string> WriteData_TextFile(string outputFilePath, string outputText, string[] array_OutputText, Boolean flag_OutputIsArray = false, Boolean flag_IfFileExists_Replace = true)
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

        public static Tuple<Boolean, string> WaitForFile_AtLocation(string filePath, int waitSeconds)
        {
            for (int i = 0; i < waitSeconds; i++)
            {
                if (System.IO.File.Exists(filePath))
                {
                    break;
                }
                else
                {
                    Task.Delay(1000);
                }
            }
            if (!System.IO.File.Exists(filePath))
            {
                return Tuple.Create(true, "Failed");
            }
            else
            {
                return Tuple.Create(true, "Success");
            }
        }
    }
}
