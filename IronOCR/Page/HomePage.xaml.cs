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
using IS_PDF = IronOCR.Service.Helper_PDFHandling;
using OCRS = IronOCR.Service.IronOCRHandler;
using FS = IronOCR.Service.Helper_FolderFileHandler;

namespace IronOCR
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();
        }

        private void Sample_Click(object sender, RoutedEventArgs e)
        {
            //Uri uri = new Uri("/IronOCR;component/Page/UploadPage.xaml", UriKind.Relative);
            //this.NavigationService.Navigate(uri);
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDlg = new Microsoft.Win32.OpenFileDialog();
            openFileDlg.Filter = "PDF (*.PDF)|*.PDF|" + "Images (*.PNG;*.JPG;*.GIF;*.BMP)|*.PNG;*.JPG;*.GIF;*.BMP";
            openFileDlg.Multiselect = false;
            openFileDlg.Title = "IronOCR File Browser";
            Nullable<bool> result = openFileDlg.ShowDialog();
            //Boolean result = true;

            if (result == true)
            {
                string fileName = openFileDlg.FileName;
                FileNameTextBox.Text = fileName;

                OCRS.SimpleRead(fileName);
                ////string fileName = @"C:\Users\Raguvarthan\Desktop\Logo1.JPG";
                //string fileExtension = System.IO.Path.GetExtension(fileName);
                //if (fileExtension.ToLower() == ".png" || fileExtension.ToLower() == ".jpg" || fileExtension.ToLower() == ".gif" || fileExtension.ToLower() == ".bmp")
                //{
                //    UploadButton.Visibility = Visibility.Visible;
                //    Panel2.Visibility = Visibility.Hidden;
                //    Panel3.Visibility = Visibility.Visible;
                //    Panel4.Visibility = Visibility.Hidden;
                //    BitmapImage bitmap = new BitmapImage();
                //    bitmap.BeginInit();
                //    bitmap.UriSource = new Uri(fileName);
                //    bitmap.EndInit();
                //    ImageViewer1.Source = bitmap;
                //}
                //else if (fileExtension.ToLower() == ".pdf")
                //{
                //    UploadButton.Visibility = Visibility.Visible;
                //    Panel2.Visibility = Visibility.Visible;
                //    Panel3.Visibility = Visibility.Visible;
                //    Panel4.Visibility = Visibility.Visible;
                //    MessageBox.Show("PDF");
                //}
            }
        }

        private void UploadButton_Click(object sender, RoutedEventArgs e)
        {
            //IS_PDF.MergePDF_NonScanned(@"C:\Users\Raguvarthan\Documents\Symprio Projects\IronOCR\", "test", new string[2] { @"C:\Users\Raguvarthan\Documents\Symprio Projects\IronOCR\Test1.pdf", @"C:\Users\Raguvarthan\Documents\Symprio Projects\IronOCR\Test2.pdf" }, false);

            //IS_PDF.SplitPDF_NonScanned(@"C:\Users\Raguvarthan\Documents\Symprio Projects\IronOCR\test.pdf", @"C:\Users\Raguvarthan\Documents\Symprio Projects\IronOCR", "Testing_", true);
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
