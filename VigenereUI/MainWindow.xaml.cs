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

namespace VigenereUI
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
            if (Key.Text == string.Empty || File.Text == string.Empty)
            {
                MessageBox.Show("Something gone wrong!", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
                return;
            }

            VigenereAlgorithm alg = new VigenereAlgorithm();

            try
            {
                string result = string.Empty;

                if (Action.SelectionBoxItem.ToString() == "Decrypt")
                {
                    result = alg.Decrypt(System.IO.File.ReadAllText(File.Text), Key.Text, Language.Text);
                }

                if (Action.SelectionBoxItem.ToString() == "Encrypt")
                {
                    result = alg.Encrypt(System.IO.File.ReadAllText(File.Text), Key.Text, Language.Text);
                }

                string newFilename = System.IO.Path.GetRandomFileName();

                System.IO.File.WriteAllText(newFilename, result);

                MessageBox.Show($"Check {newFilename}", "OK", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
            catch
            {
                MessageBox.Show("Something gone wrong!", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
            }
        }
    }
}
