using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Paradigms.Application;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Paradigms.Application.Commands;

namespace MenuParadigm
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private string currentCatalog = Directory.GetCurrentDirectory();

        public MainWindow()
        {
            InitializeComponent();
            CurrentDirrectory.Text = currentCatalog;
            WriteFileAndDirrectories();
        }

        private async void WriteFileAndDirrectories(bool isMore = false) 
        {
            var command = new LsCommand();
            var res = isMore ? await command.Execute("-l", currentCatalog) :await command.Execute("", currentCatalog);
            GridList.ItemsSource = res.Result;
            WriteFilesList();
        }

        private void CDButton_Click(object sender, RoutedEventArgs e)
        {
            ExecuteCd();
        }

        private void PrevButton_Click(object sender, RoutedEventArgs e) 
        {
            if (GridList.SelectedItem == null)
            {
                MessageBox.Show("Выберите дирректорию изи списка");
                return;
            }

            var childDir = currentCatalog + $"\\{(GridList.SelectedItem as Info).Name}";
            if (!Directory.Exists(childDir)) 
            {
                MessageBox.Show("Такой дирректории не существует");
                return;
            }

            currentCatalog = childDir;
            CurrentDirrectory.Text = childDir;
            WriteFileAndDirrectories();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e) 
        {
            var parent = Directory.GetParent(currentCatalog)?.FullName;
            if (parent == null) 
            {
                MessageBox.Show("У дирректории нет родительской");
                return;
            }
            currentCatalog = parent;
            CurrentDirrectory.Text = parent;
            WriteFileAndDirrectories();
        }

        private void IsAllFilesButton_click(object sender, RoutedEventArgs e) 
        {
            if (IsAllFiles.IsChecked ?? false)
            {
                FilesList.IsEnabled = false;
            }
            else 
            {
                FilesList.IsEnabled = true;
            }
        }

        private void ChangeFileButton_Click(object sender, RoutedEventArgs e) 
        {
            if (string.IsNullOrEmpty(FormatName.Text)) 
            {
                MessageBox.Show("Введите расширение");
                return;
            }

            var command = new CfCommand();

            if (IsAllFiles.IsChecked ?? false)
            {
                var res = command.Execute($"{FormatName.Text} *",currentCatalog).Result;
                if (res.Errors.Count > 0) 
                {
                    MessageBox.Show(string.Join('\n', res.Errors));
                }
                
                WriteFileAndDirrectories();
            }
            else 
            {
                if (FilesList.SelectedItems.Count == 0) 
                {
                    MessageBox.Show("Выберите хотябы один файл");
                    return;
                }

                var files = "";
                foreach (var f in FilesList.SelectedItems) 
                {
                    files += (f as string) + " ";
                }
                var res = command.Execute($"{ FormatName.Text } { files}",currentCatalog).Result;
                WriteFileAndDirrectories();
                if (res.Errors.Count > 0)
                {
                    MessageBox.Show(string.Join('\n', res.Errors));
                }
            }

        }

        private void MoreInfoButton_Click(object sender, RoutedEventArgs e) 
        {
            WriteFileAndDirrectories(true);
        }

        private void WriteFilesList() 
        {
            var dirInfo = new DirectoryInfo(currentCatalog);
            var files = dirInfo.GetFiles().Select(fi=>fi.Name);
            FilesList.ItemsSource = files;
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ExecuteCd();
            }
        }

        private void ExecuteCd() 
        {
            var command = new CdCommand();
            var res = command.Execute(CurrentDirrectory.Text, currentCatalog).Result;
            if (res.Errors.Count > 0)
            {
                MessageBox.Show(string.Join('\n', res.Errors));
                CurrentDirrectory.Text = currentCatalog;
                return;
            }

            currentCatalog = CurrentDirrectory.Text;
            WriteFileAndDirrectories();
        }

        private void MouseDown_Click(object sender, MouseEventArgs e) 
        {
            if (GridList.SelectedItem == null)
            {
                MessageBox.Show("Выберите дирректорию изи списка");
                return;
            }

            var childDir = currentCatalog + $"\\{(GridList.SelectedItem as Info).Name}";
            if (!Directory.Exists(childDir))
            {
                MessageBox.Show("Такой дирректории не существует");
                return;
            }

            currentCatalog = childDir;
            CurrentDirrectory.Text = childDir;
            WriteFileAndDirrectories();
        }
    }
}
