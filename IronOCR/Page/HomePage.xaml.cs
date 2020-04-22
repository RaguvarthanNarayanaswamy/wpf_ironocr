using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using IronOcr;
using PDFSH = IronOCR.Service.PDFHandler;
using OCRS = IronOCR.Service.IronOCRHandler;
using FSH = IronOCR.Service.FolderFileHandler;
using IHS = IronOCR.Service.ImageHandler;
using GSHS = IronOCR.Service.GhostScriptHandler;
using PHS = IronOCR.Service.PageHandler;
using System.Drawing;

namespace IronOCR
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        List<System.Drawing.Image> list_PDFImages = new List<System.Drawing.Image>();
        List<System.Drawing.Image> list_ProcessedImages = new List<System.Drawing.Image>();
        List<string> list_ProcessedText = new List<string>();
        int imageIdx = 0;
        Boolean flagUploadClicked = false;

        public HomePage()
        {
            InitializeComponent();
            var screenSize = PHS.GetScreenSize();

            double wordArea_Width = screenSize.Item1 - 5;
            double wordArea_Height = screenSize.Item2 - 10;

            rowDef1.Height = new GridLength(50);
            rowDef2.Height = new GridLength(60);
            rowDef3.Height = new GridLength(wordArea_Height - 200);
            colDef1.Width = new GridLength(wordArea_Width);

            FileNameTextBox.Width = (wordArea_Width/5)*3;

            ProgressBar1.Width = ((wordArea_Width / 5) * 2) - 355;
            ImageViewer1.Width = (wordArea_Width / 2) - 15;
            //ImageViewer2.Width = (wordArea_Width / 2) - 15;
            ProcessedDataTextBox.Width = (wordArea_Width / 2) - 15;
            Panel2_1.Width = (wordArea_Width / 2) - 15;
            Panel2_1_1.Width = ((wordArea_Width / 2) - 15) / 2;
            Panel2_1_2.Width = ((wordArea_Width / 2) - 15) / 2;

            //Button Enable/Diable
            FileNameTextBox.IsEnabled = false; 
            UploadButton.IsEnabled = false;
            ClearButton.IsEnabled = false;
        }
        
        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDlg = new Microsoft.Win32.OpenFileDialog();
            openFileDlg.Filter = "Images (*.PNG;*.JPG;*.GIF;*.BMP)|*.PNG;*.JPG;*.GIF;*.BMP|"+ "PDF (*.PDF)|*.PDF";
            openFileDlg.Multiselect = false;
            openFileDlg.Title = "IronOCR File Browser";
            Nullable<bool> result = openFileDlg.ShowDialog();

            if (result == true)
            {
                string inputFilePath = openFileDlg.FileName;
                FileNameTextBox.Text = inputFilePath;
                string fileExtension = System.IO.Path.GetExtension(inputFilePath);
                if (fileExtension.ToLower() == ".png" || fileExtension.ToLower() == ".jpg" || fileExtension.ToLower() == ".gif" || fileExtension.ToLower() == ".bmp")
                {
                    //UI elements 
                    Panel2.Visibility = Visibility.Visible;
                    Panel3.Visibility = Visibility.Visible;  
                    ZoomInButton.Visibility = Visibility.Visible;
                    ZoomOutButton.Visibility = Visibility.Visible;
                    LeftButton.Visibility = Visibility.Hidden;
                    RightButton.Visibility = Visibility.Hidden;
                    HomeButton.Visibility = Visibility.Hidden;
                    UploadButton.IsEnabled = true;
                    ClearButton.IsEnabled = true;

                    System.Drawing.Image inputImage = System.Drawing.Image.FromFile(inputFilePath);
                    list_PDFImages.Add(inputImage);
                    ImageViewer1.Source = IHS.ConvertImage_ToBitmapImage(inputImage);
                }
                else if (fileExtension.ToLower() == ".pdf")
                {
                    //UI elements 
                    Panel2.Visibility = Visibility.Visible;
                    Panel3.Visibility = Visibility.Visible;
                    ZoomInButton.Visibility = Visibility.Visible;
                    ZoomOutButton.Visibility = Visibility.Visible;
                    LeftButton.Visibility = Visibility.Visible;
                    RightButton.Visibility = Visibility.Visible;
                    HomeButton.Visibility = Visibility.Visible;
                    UploadButton.IsEnabled = true;
                    ClearButton.IsEnabled = true;

                    var resultPDFtoImage = GSHS.ConvertPDF_ToImage(inputFilePath);
                    if (resultPDFtoImage.Item1 && resultPDFtoImage.Item3 > 0)
                    {
                        ImageViewer1.Source = IHS.ConvertImage_ToBitmapImage(resultPDFtoImage.Item4[0]);
                        list_PDFImages = resultPDFtoImage.Item4;
                    }
                }
                BrowseButton.IsEnabled = false;
            }
        }

        private void UploadButton_Click(object sender, RoutedEventArgs e)
        {
            ProgressBar1.Visibility = Visibility.Visible;
            flagUploadClicked = true;
            if (list_PDFImages.Count > 0)
            {
                flagUploadClicked = true;
                foreach (System.Drawing.Image image in list_PDFImages)
                {
                    var ocrReadResult = OCRS.AdvancedRead_Image(image);
                    if (ocrReadResult.Item1)
                    {
                        var resultMarkImage = OCRS.ProcessImage_MarkIdentifiedText(ocrReadResult.Item3.Pages[0]);
                        if(resultMarkImage.Item1)
                        {
                            System.Drawing.Image markedImage = resultMarkImage.Item3;
                            list_ProcessedImages.Add(markedImage);
                            list_ProcessedText.Add(ocrReadResult.Item3.Pages[0].Text);
                        }
                        else
                        {
                            list_ProcessedImages.Add(image);
                            list_ProcessedText.Add(String.Empty);
                        }
                    }
                    else
                    {
                        list_ProcessedImages.Add(image);
                        list_ProcessedText.Add(String.Empty);
                    }
                }
                //Completed processing
                if (list_ProcessedImages.Count > 0)
                {
                    ImageViewer1.Source = IHS.ConvertImage_ToBitmapImage(list_ProcessedImages[0]);
                    imageIdx = 0;
                }
            }
            UploadButton.IsEnabled = false;
            ProgressBar1.Visibility = Visibility.Hidden;
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            flagUploadClicked = false;
            FileNameTextBox.Text = String.Empty;
            BrowseButton.IsEnabled = true;
        }

        protected void OnWindowSizeChanged(object sender, SizeChangedEventArgs e)
        {
            MessageBox.Show("Changes");
            double newWindowHeight = e.NewSize.Height;
            double newWindowWidth = e.NewSize.Width;
            double prevWindowHeight = e.PreviousSize.Height;
            double prevWindowWidth = e.PreviousSize.Width;
        }

        private void ZoomInButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ZoomOutButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LeftButton_Click(object sender, RoutedEventArgs e)
        {
            if (flagUploadClicked)
            {
                if (list_ProcessedImages.Count > 0)
                {
                    if (imageIdx > 0)
                    {
                        System.Drawing.Image currentImage = list_ProcessedImages[imageIdx - 1];
                        ImageViewer1.Source = IHS.ConvertImage_ToBitmapImage(currentImage);
                        imageIdx--;
                    }
                }
            }
            else
            {
                if (list_PDFImages.Count > 0)
                {
                    if (imageIdx > 0)
                    {
                        System.Drawing.Image currentImage = list_PDFImages[imageIdx - 1];
                        ImageViewer1.Source = IHS.ConvertImage_ToBitmapImage(currentImage);
                        imageIdx--;
                    }
                }
            }
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            if (flagUploadClicked)
            {
                if (list_ProcessedImages.Count > 0)
                {
                    System.Drawing.Image currentImage = list_ProcessedImages[0];
                    ImageViewer1.Source = IHS.ConvertImage_ToBitmapImage(currentImage);
                    imageIdx = 0;
                }
            }
            else
            {
                if (list_ProcessedImages.Count > 0)
                {
                    System.Drawing.Image currentImage = list_ProcessedImages[0];
                    ImageViewer1.Source = IHS.ConvertImage_ToBitmapImage(currentImage);
                    imageIdx = 0;
                }
            }
        }

        private void RightButton_Click(object sender, RoutedEventArgs e)
        {
            if (flagUploadClicked)
            {
                if (list_PDFImages.Count > 0)
                {
                    if (imageIdx + 1 < list_PDFImages.Count)
                    {
                        System.Drawing.Image currentImage = list_PDFImages[imageIdx + 1];
                        ImageViewer1.Source = IHS.ConvertImage_ToBitmapImage(currentImage);
                        imageIdx++;
                    }
                }
            }
            else
            { 
                if (list_PDFImages.Count > 0)
                {
                    if (imageIdx + 1 < list_PDFImages.Count)
                    {
                        System.Drawing.Image currentImage = list_PDFImages[imageIdx + 1];
                        ImageViewer1.Source = IHS.ConvertImage_ToBitmapImage(currentImage);
                        imageIdx++;
                    }
                }
            }
        }

    }
}
