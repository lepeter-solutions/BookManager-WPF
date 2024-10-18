using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.Json;
using Microsoft.Win32;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace beadando
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // add options to combobox

        public List<Books> BookList = new List<Books>();
        static public List<string> bookGenres = new List<string>
        {
            "Összes",
            "Regény",
            "Dráma",
            "Krimi",
            "Sci-fi",
            "Fantasy",
            "Kaland",
            "Horror",
            "Romantikus",
            "Történelmi",
            "Életrajzi",
            "Ismeretterjesztő",
            "Képregény"
        };
        public MainWindow()
        {
            InitializeComponent();
            
            foreach (string genre in bookGenres)
            {
                comboBox.Items.Add(genre);
                
            }
            comboBox.SelectedIndex = 0;
            //create default books

            Books book1 = new Books("Harry Potter és a bölcsek köve", "J.K. Rowling", "Fantasy");
            BookList.Add(book1);

            Books book2 = new Books("A Gyűrűk Ura", "J.R.R. Tolkien", "Fantasy");
            BookList.Add(book2);
            
            Books book3 = new Books("Az alkimista", "Paulo Coelho", "Regény");
            BookList.Add(book3);
            refreshOutput();

        }

        private void OutputField_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (outputField.SelectedItem != null)
            {
                string selectedItem = outputField.SelectedItem.ToString();
                MessageBoxResult result = MessageBox.Show("Biztosan törölni szeretnéd?", "Törlés", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    foreach (Books book in BookList)
                    {
                        if (selectedItem == $"{book.Author} - {book.Title} - {book.Genre}")
                        {
                            BookList.Remove(book);
                            refreshOutput();
                            break;
                        }
                    }
                }
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            refreshOutput();
        }

        public void refreshOutput()
        {

            outputField.Items.Clear();
            if (comboBox.SelectedItem.ToString() == "Összes")
            {
                foreach (Books book in BookList)
                {
                    outputField.Items.Add($"{book.Author} - {book.Title} - {book.Genre}");
                }
            }
            else
            {
                foreach (Books book in BookList)
                {
                    if (book.Genre == comboBox.SelectedItem.ToString())
                    {
                        outputField.Items.Add($"{book.Author} - {book.Title}");
                    }
                }
            }
           
            
        }

        private void sendButton_Click(object sender, RoutedEventArgs e)
        {
            if (inputFieldTitle.Text == "" || inputFieldAuthor.Text == "" || comboBox.Text == "")
            {
                MessageBox.Show("Minden mezőt ki kell tölteni!");
                return;
            }
            createBook();
        }
        private void createBook()
        {
            string title = inputFieldTitle.Text;
            string author = inputFieldAuthor.Text;
            string genre = comboBox.Text;
            Books book = new Books(title, author, genre);
            BookList.Add(book);
            refreshOutput();
            inputFieldTitle.Text = "";
            inputFieldAuthor.Text = "";
        }

        
            private void OutputField_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
            {
                if (outputField.SelectedItem != null)
                {
                    string selectedItem = outputField.SelectedItem.ToString();
                    Books selectedBook = null;

                    foreach (Books book in BookList)
                    {
                        if (selectedItem == $"{book.Author} - {book.Title} - {book.Genre}")
                        {
                            selectedBook = book;
                            break;
                        }
                    }

                    if (selectedBook != null)
                    {
                        EditBookWindow editWindow = new EditBookWindow(selectedBook);
                        if (editWindow.ShowDialog() == true)
                        {
                            refreshOutput();
                        }
                    }
                }
            }
        


        private void import_Click(object sender, RoutedEventArgs e)
        {
            BookList.Clear();
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
            openFileDialog.Title = "Open Book List";
            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    string json = File.ReadAllText(openFileDialog.FileName);
                    List<Books> importedBooks = JsonSerializer.Deserialize<List<Books>>(json);
                    if (importedBooks != null)
                    {
                        BookList.AddRange(importedBooks);
                        refreshOutput();
                        MessageBox.Show("Book list imported successfully!", "Import", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Failed to import book list. The file may be corrupted or in an incorrect format.", "Import Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while importing the book list: {ex.Message}", "Import Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void export_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
            saveFileDialog.Title = "Save Book List";
            if (saveFileDialog.ShowDialog() == true)
            {
                string json = JsonSerializer.Serialize(BookList, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(saveFileDialog.FileName, json);
                MessageBox.Show("Book list exported successfully!", "Export", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}