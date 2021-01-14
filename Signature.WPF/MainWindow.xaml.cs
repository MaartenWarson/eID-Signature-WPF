using Signature.Business;
using Signature.Business.Exceptions;
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
using System.IO;

namespace Signature.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ReadData rd;
        private Integrity integrity;
        private string fullPathDummyPDF;
        private const string SIGN_LABEL = "Signature";
        private byte[] certificateBytes;

        public MainWindow()
        {
            InitializeComponent();

            fullPathDummyPDF = System.IO.Path.GetFullPath("./resources/Dummy file.pdf"); // in Signature.WPF/bin/Debug/resources
            rd = new ReadData();

            ShowData();
            ReadPDF();
        }

        private void ShowData()
        {
            try
            {
                lblSurname.Content = rd.GetSurname();
                lblFirstnames.Content = rd.GetFirstnames();
                lblGender.Content = rd.GetGender();
                lblLocationOfBirth.Content = rd.GetLocationOfBirth();
                lblDateOfBirth.Content = rd.GetDateOfBirth();
                lblNationality.Content = rd.GetNationality();
            }
            catch(EIDNotFoundException eidnfe)
            {
                ShowMessageBoxEIDNotFound(eidnfe);
            }
        }

        private void ReadPDF()
        {
            pdfWebViewer.Navigate(fullPathDummyPDF);
        }

        private void btnSign_Click(object sender, RoutedEventArgs e)
        {
            lblConfirmation.Visibility = Visibility.Hidden;

            // Show loading message
            lblLoading.Visibility = Visibility.Visible;

            Sign sign = new Sign();
            byte[] dummyPDFBytes = File.ReadAllBytes(fullPathDummyPDF);
            byte[] signedDataBytes = null;
            bool signedSuccessfully = false;

            try
            {
                signedDataBytes = sign.DoSign(dummyPDFBytes, SIGN_LABEL);
                certificateBytes = rd.GetCertificateSignatureFile();

                integrity = new Integrity();
                signedSuccessfully = integrity.Verify(dummyPDFBytes, signedDataBytes, certificateBytes);
            }
            catch(EIDNotFoundException eidnfe)
            {
                ShowMessageBoxEIDNotFound(eidnfe);
            }
            catch(VerificationException ve)
            {
                MessageBox.Show(ve.Message, "Verificatiefout", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch(SignatureCanceledException sce)
            {
                MessageBox.Show(sce.Message, "Digitale handtekening geannuleerd", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            finally
            {
                HideLoadingMessage();
            }
            
            // Success message
            if (signedDataBytes != null && signedSuccessfully)
            {
                HideLoadingMessage();
                lblConfirmation.Content = $"Digitaal getekend op {DateTime.Now}.";
                lblConfirmation.Visibility = Visibility.Visible;

                //SaveSignedFile(signedDataBytes);
            }
        }

        private void HideLoadingMessage()
        {
            lblLoading.Visibility = Visibility.Hidden;
        }

        private void ShowMessageBoxEIDNotFound(EIDNotFoundException eidnfe)
        {
            if (MessageBox.Show(eidnfe.Message, "Geen eID gevonden", MessageBoxButton.OKCancel, MessageBoxImage.Error) == MessageBoxResult.OK)
            {
                ShowData();
            }
            else
            {
                Environment.Exit(0);
            }
        }

        private void SaveSignedFile(byte[] signedDataBytes)
        {
            File.WriteAllBytes("C:/PXL/Test.pdf", signedDataBytes);
        }
    }
}
