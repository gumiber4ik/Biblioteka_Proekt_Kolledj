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
using System.Windows;

namespace Dadaev_Proekt_
{
    /// <summary>
    /// Логика взаимодействия для KnigiGivSet.xaml
    /// </summary>
    public partial class KnigiGivSet : Window
    {
        public KnigiGivSet()
        {
            InitializeComponent();
            LoadStudents();
            LoadBooks();
        }
        private void LoadStudents()
        {
            using (SqlConnection connection = new SqlConnection(info.connectionString)) // Используем connectionString из класса info
            {
                connection.Open();
                string query = "SELECT User_ID, First_Name + ' ' + Last_Name AS FullName FROM AK_Users WHERE Role_ID = (SELECT Role_ID FROM AK_Role WHERE Role_Name = 'Студент')";
                SqlCommand command = new SqlCommand(query, connection);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string fullName = reader["FullName"].ToString();
                        cbStudents.Items.Add(fullName);
                    }
                }
            }
        }
        private void LoadBooks()
        {
            using (SqlConnection connection = new SqlConnection(info.connectionString)) // Используем connectionString из класса info
            {
                connection.Open();
                string query = "SELECT ISBN, Title FROM AK_Books";
                SqlCommand command = new SqlCommand(query, connection);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string title = reader["Title"].ToString();
                        cbBooks.Items.Add(title);
                    }
                }
            }
        }


        private void IssueBook_Click(object sender, RoutedEventArgs e)
        {
            var selectedStudentName = cbStudents.SelectedItem as string;
            var selectedBookTitle = cbBooks.SelectedItem as string;
            var returnDate = dpReturnDate.SelectedDate;

            if (string.IsNullOrEmpty(selectedStudentName) || string.IsNullOrEmpty(selectedBookTitle) || !returnDate.HasValue)
            {
                MessageBox.Show("Пожалуйста, выберите студента, книгу и дату возврата.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Запросы к базе данных для получения ID студента и книги
            int? studentId = GetUserIdByName(selectedStudentName);
            int? bookId = GetBookIdByTitle(selectedBookTitle);

            if (studentId == null || bookId == null)
            {
                MessageBox.Show("Не удалось найти выбранного студента или книгу в базе данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Вызываем метод для добавления записи о выдаче книги
            IssueBookToStudent(studentId.Value, bookId.Value, DateTime.Now, returnDate.Value);
            var readerWindow = new KnigiGivSet();
            readerWindow.Hide();
        }

        private int? GetUserIdByName(string fullName)
        {
            using (var connection = new SqlConnection(info.connectionString))
            {
                connection.Open();
                var query = "SELECT User_ID FROM AK_Users WHERE First_Name + ' ' + Last_Name = @FullName";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FullName", fullName);
                    var result = command.ExecuteScalar();
                    return result != null ? (int?)result : null;
                }
            }
        }

        private int? GetBookIdByTitle(string title)
        {
            using (var connection = new SqlConnection(info.connectionString))
            {
                connection.Open();
                var query = "SELECT ISBN FROM AK_Books WHERE Title = @Title";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Title", title);
                    var result = command.ExecuteScalar();
                    return result != null ? (int?)result : null;
                }
            }
        }

        private void IssueBookToStudent(int userId, int bookId, DateTime issueDate, DateTime returnDate)
        {
            using (var connection = new SqlConnection(info.connectionString))
            {
                connection.Open();

                // Добавляем запись о выдаче книги
                var insertQuery = "INSERT INTO AK_Loans (User_ID, Book_ID, Issue_Date, Return_Date, Returned) VALUES (@UserId, @BookId, @IssueDate, @ReturnDate, 0)";
                using (var insertCommand = new SqlCommand(insertQuery, connection))
                {
                    insertCommand.Parameters.AddWithValue("@UserId", userId);
                    insertCommand.Parameters.AddWithValue("@BookId", bookId);
                    insertCommand.Parameters.AddWithValue("@IssueDate", issueDate);
                    insertCommand.Parameters.AddWithValue("@ReturnDate", returnDate);
                    insertCommand.ExecuteNonQuery();
                }

                // Обновляем количество копий книги
                var updateQuery = "UPDATE AK_Books SET Number_of_Copies = Number_of_Copies - 1 WHERE ISBN = @BookId AND Number_of_Copies > 0";
                using (var updateCommand = new SqlCommand(updateQuery, connection))
                {
                    updateCommand.Parameters.AddWithValue("@BookId", bookId);
                    int rowsAffected = updateCommand.ExecuteNonQuery();

                    // Проверяем, было ли обновлено количество копий
                    if (rowsAffected == 0)
                    {
                        MessageBox.Show("Не удалось обновить количество копий. Возможно, книги уже нет в наличии.", "Ошибка выдачи", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
            }
            MessageBox.Show("Книга успешно выдана и количество копий обновлено.", "Выдача книги", MessageBoxButton.OK, MessageBoxImage.Information);
        }





    }
}
