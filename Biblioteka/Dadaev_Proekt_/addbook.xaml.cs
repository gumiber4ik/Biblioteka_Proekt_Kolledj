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
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;

namespace Dadaev_Proekt_
{
    public partial class addbook : Window
    {
        public addbook()
        {
            InitializeComponent();
            LoadComboBoxes();
            LoadBooks("");
            SearchTextBox.TextChanged += SearchTextBox_TextChanged;
        }
        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                LoadBooks(textBox.Text);  // Загрузка и фильтрация данных
            }
        }

        private void SearchTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (SearchTextBox.Text == "Введите название книги для поиска...")
            {
                SearchTextBox.Text = "";
                SearchTextBox.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        private void SearchTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchTextBox.Text))
            {
                SearchTextBox.Text = "Введите название книги для поиска...";
                SearchTextBox.Foreground = new SolidColorBrush(Colors.Gray);
            }
        }
        private void LoadBooks(string filter)
        {
            using (SqlConnection connection = new SqlConnection(info.connectionString))
            {
                try
                {
                    connection.Open();
                    string query = @"
            SELECT B.Title, CONCAT(A.First_Name, ' ', A.Last_Name) AS Author, B.ISBN, B.Number_of_Copies
            FROM dbo.AK_Books B
            INNER JOIN dbo.AK_Authors A ON B.Author_ID = A.Author_ID
            WHERE B.Title LIKE @Filter OR CONCAT(A.First_Name, ' ', A.Last_Name) LIKE @Filter";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Filter", $"%{filter}%");

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
        private void LoadComboBoxes()
        {
            LoadAuthors();
            LoadPublishers();
            LoadGenres();
        }

        private void LoadAuthors()
        {
            using (SqlConnection connection = new SqlConnection(info.connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT Author_ID, First_Name + ' ' + Last_Name AS FullName FROM AK_Authors ORDER BY First_Name, Last_Name";
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    authorComboBox.Items.Clear();

                    while (reader.Read())
                    {
                        authorComboBox.Items.Add(new { Author_ID = reader["Author_ID"], FullName = reader["FullName"].ToString() });
                    }

                    authorComboBox.DisplayMemberPath = "FullName";
                    authorComboBox.SelectedValuePath = "Author_ID";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка загрузки авторов: " + ex.Message);
                }
            }
        }

        private void LoadPublishers()
        {
            using (SqlConnection connection = new SqlConnection(info.connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT Publisher_ID, Publisher_Name FROM AK_Publishers ORDER BY Publisher_Name";
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    publisherComboBox.Items.Clear();

                    while (reader.Read())
                    {
                        publisherComboBox.Items.Add(new { Publisher_ID = reader["Publisher_ID"], PublisherName = reader["Publisher_Name"].ToString() });
                    }

                    publisherComboBox.DisplayMemberPath = "PublisherName";
                    publisherComboBox.SelectedValuePath = "Publisher_ID";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка загрузки издателей: " + ex.Message);
                }
            }
        }

        private void LoadGenres()
        {
            using (SqlConnection connection = new SqlConnection(info.connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT Genre_ID, Genre_Name FROM AK_Genres ORDER BY Genre_Name";
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    genreComboBox.Items.Clear();

                    while (reader.Read())
                    {
                        genreComboBox.Items.Add(new { Genre_ID = reader["Genre_ID"], GenreName = reader["Genre_Name"].ToString() });
                    }

                    genreComboBox.DisplayMemberPath = "GenreName";
                    genreComboBox.SelectedValuePath = "Genre_ID";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка загрузки жанров: " + ex.Message);
                }
            }
        }


        private void AddBook_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(info.connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "INSERT INTO AK_Books (Title, Author_ID, Genre_ID, Number_of_Copies) VALUES (@Title, @AuthorID, @GenreID, @Copies)";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Title", titleTextBox.Text);
                    command.Parameters.AddWithValue("@AuthorID", Convert.ToInt32(authorComboBox.SelectedValue));
                    command.Parameters.AddWithValue("@GenreID", Convert.ToInt32(genreComboBox.SelectedValue));
                    command.Parameters.AddWithValue("@Copies", int.Parse(copiesTextBox.Text));

                    command.ExecuteNonQuery();
                    MessageBox.Show("Книга добавлена успешно.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при добавлении книги: " + ex.Message);
                }
            }
        }




        private void DeleteBook_Click(object sender, RoutedEventArgs e)
        {
            string title = titleTextBox.Text; // Предположим, что пользователь вводит название книги для удаления

            using (SqlConnection connection = new SqlConnection(info.connectionString))
            {
                connection.Open();

                // Проверяем, существует ли книга с таким названием
                string checkQuery = "SELECT ISBN FROM AK_Books WHERE Title = @Title";
                SqlCommand checkCommand = new SqlCommand(checkQuery, connection);
                checkCommand.Parameters.AddWithValue("@Title", title);
                object result = checkCommand.ExecuteScalar();

                if (result != null)
                {
                    int bookId = Convert.ToInt32(result); // Получаем ISBN книги для удаления

                    // Удаление книги по полученному ISBN
                    string deleteQuery = "DELETE FROM AK_Books WHERE ISBN = @BookId";
                    SqlCommand deleteCommand = new SqlCommand(deleteQuery, connection);
                    deleteCommand.Parameters.AddWithValue("@BookId", bookId);

                    int rowsAffected = deleteCommand.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Книга успешно удалена.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Не удалось удалить книгу.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Книга с таким названием не найдена.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

    }
}
