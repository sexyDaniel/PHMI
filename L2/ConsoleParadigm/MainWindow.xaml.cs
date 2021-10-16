using Paradigms.Application;
using System.IO;
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
using System.Collections.ObjectModel;
using System;

namespace ConsoleParadigm
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string BaseCatalog = @"C:\Users\Acer\Desktop\Semesters\1 семестр";
        private List<string> history;
        private int currentCommand = -1;
        public MainWindow()
        {
            InitializeComponent();
            history = new List<string>();
            CurrentCatalog.Text = BaseCatalog;
            HistoryTextBox.Text = BaseCatalog + ">";
        }

        private async void ExecuteCommand(string command) 
        {
            if (command == "")
            {
                return;
            }

            try
            {
                var arr = command.Split(' ', 2);
                switch (arr[0])
                {
                    case "cd":
                        {
                            var res = DirectoryWorker.ExecuteCd(command, CurrentCatalog.Text);
                            if (res == null)
                            {
                                HistoryTextBox.Text += "Ошибка. Неверная команда или каталог\r\n";
                                HistoryTextBox.Text += $"{CurrentCatalog.Text }>";
                                break;
                            }
                            CurrentCatalog.Text = res;
                            HistoryTextBox.Text += $"{CurrentCatalog.Text }>";
                            break;
                        }
                    case "ls":
                        {
                            var response = await DirectoryWorker.ExecuteLs(command, CurrentCatalog.Text);
                            if (response == null)
                            {
                                HistoryTextBox.Text += "Ошибка. Неверная команда\r\n";
                                HistoryTextBox.Text += $"{CurrentCatalog.Text }>";
                                break;
                            }
                            CatalogList.ItemsSource = response;
                            HistoryTextBox.Text += $"{CurrentCatalog.Text }>";
                            break;
                        }
                    case "cf":
                        {
                            DirectoryWorker.ExecuteCf(command, CurrentCatalog.Text);
                            HistoryTextBox.Text += $"{CurrentCatalog.Text }>";
                            break;
                        }
                    case "help":
                        {
                            HistoryTextBox.Text += "cd <каталог> - переход в заданный каталог\ncd ../ - переход в родительский каталог\ncd ./<дирректория> - переход в следующую дирректорию\nls [-l] - вывод файлов и папок/[-l] с доп информацией\n cf <тип> <файл1> [<файл2>...] или [*] - смена форматов файлов, если * - всех файлов";
                            break;
                        }
                    default:
                        {
                            HistoryTextBox.Text += "Ошибка. Неверная команда\r\n";
                            HistoryTextBox.Text += $"{CurrentCatalog.Text }>";
                            break;
                        }
                }
            }
            catch(Exception ex) 
            {
                HistoryTextBox.Text += ex.Message+ "\r\n";
                HistoryTextBox.Text += $"{CurrentCatalog.Text }>";
            }
            
            CommandTextBox.Text = "";
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e) 
        {
            if (e.Key == Key.Enter) 
            {
                HistoryTextBox.Text += CommandTextBox.Text + "\r\n";
                currentCommand++;
                history.Add(CommandTextBox.Text);
                ExecuteCommand(CommandTextBox.Text);
            }
            if (e.Key == Key.F1) 
            {
                if (history.Count - 1 == currentCommand) 
                {
                    return;
                }
                currentCommand++;
                CommandTextBox.Text = history[currentCommand];
            }
            if (e.Key == Key.F2)
            {
                if (currentCommand==0)
                {
                    return;
                }
                currentCommand--;
                CommandTextBox.Text = history[currentCommand];
            }
        }
    }
}
