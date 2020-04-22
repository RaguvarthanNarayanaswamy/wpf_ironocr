using Ghostscript.NET;
using Ghostscript.NET.Rasterizer;
using System;
using System.Collections.Generic;
using System.Drawing;
using FHS = IronOCR.Service.FolderFileHandler;

namespace IronOCR.Service
{
    class GhostScriptHandler
    {
        public static Tuple<Boolean, string, int, List<Image>> ConvertPDF_ToImage(string inputFilePath, string outputFolderPath = "", Boolean flag_SaveImages = false)
        {
            try
            {
                var lastInstalledVersion = GhostscriptVersionInfo.GetLastInstalledVersion(
                    GhostscriptLicense.GPL | GhostscriptLicense.AFPL,
                    GhostscriptLicense.GPL);
                var rasterizer = new GhostscriptRasterizer();
                rasterizer.Open(inputFilePath, lastInstalledVersion, false);

                List<Image> listImage = new List<Image>();
                if (rasterizer.PageCount > 0)
                {
                    for (int idx = 0; idx < rasterizer.PageCount; idx++)
                    {
                        Image img = rasterizer.GetPage(96, 96, idx + 1);
                        listImage.Add(img);
                        if (flag_SaveImages)
                        {
                            FHS.CreateFolder(outputFolderPath);
                            img.Save(outputFolderPath + @"\" + System.IO.Path.GetFileNameWithoutExtension(inputFilePath) + "_Page_" + (idx + 1).ToString() + ".png");
                        }
                    }
                    return Tuple.Create(true, "Success", rasterizer.PageCount, listImage);
                }
                else
                {
                    return Tuple.Create(false, "No page to process", 0, listImage);
                }
            }
            catch (Exception exp)
            {
                return Tuple.Create(false, exp.Message, 0, new List<Image>());
            }
        }

        public static Tuple<Boolean, string, int> GetPDF_PageCount(string inputFilePath)
        {
            try
            {
                var lastInstalledVersion = GhostscriptVersionInfo.GetLastInstalledVersion(
                    GhostscriptLicense.GPL | GhostscriptLicense.AFPL,
                    GhostscriptLicense.GPL);
                var rasterizer = new GhostscriptRasterizer();
                rasterizer.Open(inputFilePath, lastInstalledVersion, false);
                return Tuple.Create(true, "Success", rasterizer.PageCount);

            }
            catch (Exception exp)
            {
                return Tuple.Create(false, exp.Message, 0);
            }
        }
    }
}
