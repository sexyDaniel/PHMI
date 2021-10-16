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

namespace MenuParadigm
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private string currentCatalog = @"C:\Users\Acer\Desktop\Semesters\1 семестр";

        public MainWindow()
        {
            InitializeComponent();
            CurrentDirrectory.Text = currentCatalog;
            CurrentDir.Text = currentCatalog;
            WriteFileAndDirrectories();
        }

        private async void WriteFileAndDirrectories(bool isMore = false) 
        {
            var res = isMore ? await DirectoryWorker.ExecuteLs("ls -l", currentCatalog) : await DirectoryWorker.ExecuteLs("ls", currentCatalog);
            GridList.ItemsSource = res;
            WriteFilesList();
        }

        private void CDButton_Click(object sender, RoutedEventArgs e)
        {
            if (!Directory.Exists(CurrentDirrectory.Text)) 
            {
                MessageBox.Show("Дирректории не существует");
                return;
            }

            currentCatalog = CurrentDirrectory.Text;
            CurrentDir.Text = currentCatalog;
            WriteFileAndDirrectories();
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
            CurrentDir.Text = childDir;
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
            CurrentDir.Text = parent;
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

            if (IsAllFiles.IsChecked ?? false)
            {
                try
                {
                    var command = $"cf {FormatName.Text} *";
                    DirectoryWorker.ExecuteCf(command, currentCatalog);
                    WriteFileAndDirrectories();
                    return;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    WriteFileAndDirrectories();
                }
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

                try
                {
                    var command = $"cf {FormatName.Text} {files}";
                    DirectoryWorker.ExecuteCf(command, currentCatalog);
                    WriteFileAndDirrectories();
                    return;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
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
    }
}
