using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
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
using System.Windows.Shapes;

namespace Dadaev_Proekt_
{
    /// <summary>
    /// Логика взаимодействия для bibliotecar.xaml
    /// </summary>
    public partial class bibliotecar : Window
    {
        public bibliotecar()
        {
            InitializeComponent();
            LoadBooks("", "");
            SearchTextBox.TextChanged += SearchTextBox_TextChanged;
            GenreTextBox.TextChanged += GenreTextBox_TextChanged;
        }
        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            LoadBooks(SearchTextBox.Text, GenreTextBox.Text);
        }

        private void GenreTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            LoadBooks(SearchTextBox.Text, GenreTextBox.Text);
        }

        private void LoadBooks(string titleFilter, string genreFilter)
        {
            using (SqlConnection connection = new SqlConnection(info.connectionString))
            {
                try
                {
                    connection.Open();
                    string query = @"
        SELECT B.Title, CONCAT(A.First_Name, ' ', A.Last_Name) AS Author, B.ISBN, B.Number_of_Copies, G.Genre_Name AS Genre
        FROM dbo.AK_Books B
        INNER JOIN dbo.AK_Authors A ON B.Author_ID = A.Author_ID
        INNER JOIN dbo.AK_Genres G ON B.Genre_ID = G.Genre_ID
        WHERE (B.Title LIKE @TitleFilter OR CONCAT(A.First_Name, ' ', A.Last_Name) LIKE @TitleFilter)
        AND (G.Genre_Name LIKE @GenreFilter)";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@TitleFilter", $"%{titleFilter}%");
                    command.Parameters.AddWithValue("@GenreFilter", $"%{genreFilter}%");

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    ListViewBooks.ItemsSource = dataTable.DefaultView;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при загрузке данных: " + ex.Message);
                }
            }
        }
        private void Knigi_giv(object sender, RoutedEventArgs e)
        {
            var readerWindow = new KnigiGivSet();
            readerWindow.Show();
        }

        private void Knigi_set(object sender, RoutedEventArgs e)
        {
            var readerWindow = new KnigiSet();
            readerWindow.Show();
        }

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var readerWindow = new user();
            readerWindow.Show();
        }
    }
}
