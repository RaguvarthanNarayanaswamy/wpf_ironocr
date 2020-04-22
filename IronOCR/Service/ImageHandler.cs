using IronOcr;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows;
using OCRS = IronOCR.Service.IronOCRHandler;
using FS = IronOCR.Service.FolderFileHandler;

namespace IronOCR.Service
{
    class ImageHandler
    {
        public static Tuple<Boolean, string, Image> DrawRectange_OnImage(Image inputImage, Double ocrAccurancy, int wordLocation_X, int wordLocation_Y, int wordWidth, int wordHeigth, int pageWidth, int pageHeigth)
        {
            using (Graphics graphics = Graphics.FromImage(inputImage))
            {
                Rectangle rectangle = new Rectangle(wordLocation_X, wordLocation_Y, wordWidth, wordHeigth);
                Decimal ratioX = Convert.ToDecimal(wordLocation_X) / Convert.ToDecimal(pageWidth);
                Decimal ratioY = Convert.ToDecimal(wordLocation_Y) / Convert.ToDecimal(pageHeigth);
                rectangle.X = Convert.ToInt32(ratioX * inputImage.Width);
                rectangle.Y = Convert.ToInt32(ratioY * inputImage.Height);
                rectangle.Width = Convert.ToInt32(ratioX * wordWidth);
                rectangle.Height = Convert.ToInt32(ratioY * wordHeigth);

                Color penColor = Color.Red;
                if (ocrAccurancy >= 80)
                {
                    penColor = Color.Green;
                }
                else if (ocrAccurancy >= 60)
                {
                    penColor = Color.Orange;
                }
                else
                {
                    penColor = Color.Red;
                }

                using (Pen pen = new Pen(penColor, 3))
                {
                    graphics.DrawRectangle(pen, rectangle);
                }
            }
            return Tuple.Create(true, "Success", inputImage);
        }
            
        public static Tuple<Boolean, string, Image> ResizeImage(Image inputImage, System.Drawing.Size size, bool preserveAspectRatio = true)
        {
            try
            {
                int newWidth;
                int newHeight;
                if (preserveAspectRatio)
                {
                    int originalWidth = inputImage.Width;
                    int originalHeight = inputImage.Height;
                    float percentWidth = (float)size.Width / (float)originalWidth;
                    float percentHeight = (float)size.Height / (float)originalHeight;
                    float percent = percentHeight < percentWidth ? percentHeight : percentWidth;
                    newWidth = (int)(originalWidth * percent);
                    newHeight = (int)(originalHeight * percent);
                }
                else
                {
                    newWidth = size.Width;
                    newHeight = size.Height;
                }
                Image newImage = new Bitmap(newWidth, newHeight);
                using (Graphics graphicsHandle = Graphics.FromImage(newImage))
                {
                    graphicsHandle.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphicsHandle.DrawImage(inputImage, 0, 0, newWidth, newHeight);
                }
                return Tuple.Create(true, "Success", newImage);
            }
            catch (Exception exp)
            {
                return Tuple.Create(false, exp.Message, inputImage);
            }
        }

        public static Tuple<Boolean, string> SaveImage(string outputFilePath, Image inputImage, Boolean flag_ReplaceFileIfExists = true)
        {
            try
            {
                if (System.IO.File.Exists(outputFilePath))
                {
                    if (flag_ReplaceFileIfExists)
                    {
                        System.IO.File.Delete(outputFilePath);
                    }
                    else
                    {
                        outputFilePath = System.IO.Path.GetDirectoryName(outputFilePath) + @"\" + System.IO.Path.GetFileNameWithoutExtension(outputFilePath) + "_" + DateTime.Now.ToString("yyyymmdd_hhmmss") + "_" + System.IO.Path.GetExtension(outputFilePath);
                    }
                }
                inputImage.Save(outputFilePath);
                return Tuple.Create(true, "Success");
            }
            catch (Exception exp)
            {
                return Tuple.Create(false, exp.Message);
            }
        }

        public static Tuple<Boolean, string, System.Drawing.Size> Image_MeasureDimention(Image inputImage)
        {
            try
            {
                System.Drawing.Size sizeSpec = new System.Drawing.Size();
                sizeSpec.Height = inputImage.Height;
                sizeSpec.Width = inputImage.Width;
                return Tuple.Create(true, "Success", sizeSpec);
            }
            catch (Exception exp)
            {
                return Tuple.Create(false, exp.Message, new System.Drawing.Size());
            }

        }

        public static System.Windows.Media.Imaging.BitmapImage ConvertImage_ToBitmapImage(Image inputImage)
        {
            var bitmap = new System.Windows.Media.Imaging.BitmapImage();
            bitmap.BeginInit();
            System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
            inputImage.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
            memoryStream.Seek(0, System.IO.SeekOrigin.Begin);
            bitmap.StreamSource = memoryStream;
            bitmap.EndInit();
            return bitmap;
        }
    
    }
}
