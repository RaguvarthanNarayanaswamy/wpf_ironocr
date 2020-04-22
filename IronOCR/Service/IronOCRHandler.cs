using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using IronOcr;
using IHS = IronOCR.Service.ImageHandler; 

namespace IronOCR.Service
{
    class IronOCRHandler
    {
        public static Tuple<Boolean, string, OcrResult> Read_File(string inputFilePath)
        {
            try
            {
                var Ocr = new AutoOcr();
                OcrResult result = Ocr.Read(inputFilePath);
                return Tuple.Create(true, "Success", result);
            }
            catch (Exception exp)
            {
                return Tuple.Create(false, exp.Message, new OcrResult());
            }

        }

        public static Tuple<Boolean, string, OcrResult> Read_Image(System.Drawing.Image inputImage)
        {
            try
            {
                var Ocr = new AutoOcr();
                OcrResult result = Ocr.Read(inputImage);
                return Tuple.Create(true, "Success", result);
            }
            catch (Exception exp)
            {
                return Tuple.Create(false, exp.Message, new OcrResult());
            }

        }

        public static Tuple<Boolean, string, OcrResult> AdvancedRead_File(string inputFilePath)
        {
            try
            {
                var Ocr = GetIronOCR_English();
                OcrResult result = Ocr.Read(inputFilePath);
                return Tuple.Create(true, "Success", result);
            }
            catch (Exception exp)
            {
                return Tuple.Create(false, exp.Message, new OcrResult());
            }
        }
        
        public static Tuple<Boolean, string, OcrResult> AdvancedRead_Image(System.Drawing.Image inputImage)
        {
            try
            {
                var Ocr = GetIronOCR_English();
                OcrResult result = Ocr.Read(inputImage);
                return Tuple.Create(true, "Success", result);
            }
            catch (Exception exp)
            {
                return Tuple.Create(false, exp.Message, new OcrResult());
            }
        }
               
        public static AdvancedOcr GetIronOCR_English()
        {
            var ocrObject = new AdvancedOcr()
            {
                Language = IronOcr.Languages.English.OcrLanguagePack,
                ColorSpace = AdvancedOcr.OcrColorSpace.GrayScale,
                EnhanceResolution = true,
                EnhanceContrast = true,
                CleanBackgroundNoise = true,
                ColorDepth = 4,
                RotateAndStraighten = false,
                DetectWhiteTextOnDarkBackgrounds = false,
                ReadBarCodes = true,
                Strategy = AdvancedOcr.OcrStrategy.Fast,
                InputImageType = AdvancedOcr.InputTypes.Document
            };

            return ocrObject;
        }

        public static Tuple<Boolean, string, Image, double> ProcessImage_MarkIdentifiedText(OcrResult.OcrPage inputPage)
        {
            Image outputImage = inputPage.Image;
            int int_CountOfProcessedWord = 0;
            double sum_OverallConfidence = 0;
            double overallConfidence = 0;

            try
            {
                using (Graphics graphics = Graphics.FromImage(outputImage))
                {
                    int page_width_px = inputPage.Width;
                    int page_height_px = inputPage.Height;

                    graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    // pages -> paragraphs
                    foreach (var paragraph in inputPage.Paragraphs)
                    {
                        int paragraph_number = paragraph.ParagraphNumber;
                        String paragraph_text = paragraph.Text;
                        Image paragraph_image = paragraph.Image;
                        int paragraph_x_location = paragraph.X;
                        int paragraph_y_location = paragraph.Y;
                        int paragraph_width = paragraph.Width;
                        int paragraph_height = paragraph.Height;
                        double paragraph_ocr_accuracy = paragraph.Confidence;
                        string paragraph_font_name = paragraph.FontName;
                        double paragraph_font_size = paragraph.FontSize;
                        OcrResult.TextFlow paragrapth_text_direction = paragraph.TextDirection;
                        double paragrapth_rotation_degrees = paragraph.TextOrientation;

                        // pages -> paragraphs -> lines
                        foreach (var line in paragraph.Lines)
                        {
                            int line_number = line.LineNumber;
                            String line_text = line.Text;
                            Image line_image = line.Image;
                            int line_x_location = line.X;
                            int line_y_location = line.Y;
                            int line_width = line.Width;
                            int line_height = line.Height;
                            double line_ocr_accuracy = line.Confidence;
                            double line_skew = line.BaselineAngle;
                            double line_offset = line.BaselineOffset;

                            // pages -> paragraphs -> lines -> words
                            foreach (var word in line.Words)
                            {
                                int word_number = word.WordNumber;
                                String word_text = word.Text;
                                Image word_image = word.Image;
                                int word_x_location = word.X;
                                int word_y_location = word.Y;
                                int word_width = word.Width;
                                int word_height = word.Height;
                                double word_ocr_accuracy = word.Confidence;
                                String word_font_name = word.FontName;
                                double word_font_size = word.FontSize;
                                bool word_is_bold = word.FontIsBold;
                                bool word_is_fixed_width_font = word.FontIsFixedWidth;
                                bool word_is_italic = word.FontIsItalic;
                                bool word_is_serif_font = word.FontIsSerif;
                                bool word_is_underlined = word.FontIsUnderlined;

                                int_CountOfProcessedWord++;
                                sum_OverallConfidence = sum_OverallConfidence + word_ocr_accuracy;

                                outputImage = IHS.DrawRectange_OnImage(outputImage, word_ocr_accuracy, word_x_location, word_y_location, word_width, word_height, inputPage.Width, inputPage.Height).Item3;
                            }
                        }
                    }
                    overallConfidence = sum_OverallConfidence / int_CountOfProcessedWord;
                    return Tuple.Create(true, "Success", outputImage, overallConfidence);
                }                
            }
            catch(Exception exp)
            {
                return Tuple.Create(false, exp.Message, outputImage, overallConfidence);
            }
        }
        
    }
}
