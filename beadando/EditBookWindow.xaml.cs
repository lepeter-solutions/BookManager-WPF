using System.Windows;
using System.Windows.Controls;

namespace beadando
{
    public partial class EditBookWindow : Window
    {
        public Books Book { get; private set; }

        public EditBookWindow(Books book)
        {
            InitializeComponent();
            foreach(string genre in MainWindow.bookGenres)
            {
                GenreComboBox.Items.Add(genre);
            }
            Book = book;
            TitleTextBox.Text = book.Title;
            AuthorTextBox.Text = book.Author;
            GenreComboBox.SelectedItem = GenreComboBox.Items.Cast<string>().FirstOrDefault(item => item == book.Genre);
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TitleTextBox.Text) || string.IsNullOrWhiteSpace(AuthorTextBox.Text) || GenreComboBox.SelectedItem == null)
            {
                MessageBox.Show("Minden mezőt ki kell tölteni!");
                return;
            }
            Book.Title = TitleTextBox.Text;
            Book.Author = AuthorTextBox.Text;
            Book.Genre = GenreComboBox.SelectedItem.ToString();
            DialogResult = true;
            Close();
        }
    }
}