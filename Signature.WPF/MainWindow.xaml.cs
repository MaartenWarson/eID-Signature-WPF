using Signature.Business;
using Signature.Business.Exceptions;
using System;
using System.Windows;
using System.IO;

namespace Signature.WPF
{
    public partial class MainWindow : Window
    {
        private ReadData rd;
        private Integrity integrity;
        private string fullPath;
        private const string FILE_NAME = "/Dummy file.pdf";
        private const string SIGN_LABEL = "Signature";
        private byte[] certificateBytes;
        private string firstnames;
        private string surname;
        private bool signed = false;

        public MainWindow()
        {
            InitializeComponent();

            fullPath = System.IO.Path.GetFullPath("./resources"); // in Signature.WPF/bin/Debug/resources
            rd = new ReadData();

            ShowData();
            ReadPDF();
        }

        private void ShowData()
        {
            try
            {
                firstnames = rd.GetFirstnames();
                lblFirstnames.Content = firstnames;
                surname = rd.GetSurname();
                lblSurname.Content = surname;
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
            if (!signed)
            {
                pdfWebViewer.Navigate(fullPath + FILE_NAME);
            }
            else
            {
                pdfWebViewer.Navigate(fullPath + "/Dummy file (signed).pdf");
            }
        }

        private void btnSign_Click(object sender, RoutedEventArgs e)
        {
            lblConfirmation.Visibility = Visibility.Hidden;

            // Show loading message
            lblLoading.Visibility = Visibility.Visible;

            Sign sign = new Sign();
            byte[] dummyPDFBytes = File.ReadAllBytes(fullPath + FILE_NAME);
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
                signed = sign.SignPhysically(fullPath, firstnames, surname);
                ReadPDF();
                HideLoadingMessage();


                if (signed)
                {                
                    lblConfirmation.Content = $"Digitaal getekend op {DateTime.Now}.";
                    lblConfirmation.Visibility = Visibility.Visible;
                }
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
    }
}
