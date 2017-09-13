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
using Microsoft.Win32;
using VigenereCipher;

namespace CasiskiUI
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private OpenFileDialog dialog;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            dialog = new OpenFileDialog();
            dialog.ShowDialog();
            File.Text = dialog.FileName;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (File.Text == string.Empty)
            {
                MessageBox.Show("Something gone wrong!", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
                return;
            }
            try
            {
                KasiskiExamination kasiski = new KasiskiExamination();

                string allContent = System.IO.File.ReadAllText(File.Text);
                string language = Language.Text;
                Output.Text += $"Key Length = {kasiski.FindKeyLength(Utils.TrimText(allContent, language))}";

                string newFilename = System.IO.Path.GetRandomFileName();

                KasiskiExaminationResult result = kasiski.DecryptVigenereCipher(allContent, language);

                Output.Text += " " + result.Keyword;

                System.IO.File.WriteAllText(newFilename, result.Plaintext);
                MessageBox.Show($"Check {newFilename}", "OK", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
            catch
            {
                MessageBox.Show("Something gone wrong!", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
            }
        }
    }
}
