using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace Dadaev_Proekt_
{
    public partial class KnigiSet : Window
    {
        public KnigiSet()
        {
            InitializeComponent();
            LoadStudents();
        }

        Dictionary<string, int> userNamesToIds = new Dictionary<string, int>();

        private void LoadStudents()
        {
            using (var connection = new SqlConnection(info.connectionString))
            {
                connection.Open();
                var query = "SELECT User_ID, First_Name, Last_Name FROM AK_Users WHERE Role_ID = 3"; // Подразумевается, что ID роли студентов равен 3
                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string fullName = reader["First_Name"].ToString() + " " + reader["Last_Name"].ToString();
                            int userId = (int)reader["User_ID"];
                            userNamesToIds[fullName] = userId; // Сохраняем ID, связанный с полным именем
                            cbStudents.Items.Add(fullName);
                        }
                    }
                }
            }
        }

        private void CbStudents_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbStudents.SelectedItem != null)
            {
                string fullName = cbStudents.SelectedItem.ToString();
                if (userNamesToIds.TryGetValue(fullName, out int userId))
                {
                    LoadBooksForStudent(userId);
                }
            }
        }

        private void LoadBooksForStudent(int userId)
        {
            using (var connection = new SqlConnection(info.connectionString))
            {
                connection.Open();
                var query = @"
                SELECT B.Title, L.Loan_ID, L.Return_Date, 
                       CASE 
                           WHEN DATEDIFF(day, L.Return_Date, GETDATE()) > 0 THEN DATEDIFF(day, L.Return_Date, GETDATE()) * 57 
                           ELSE 0 
                       END AS Fine
                FROM AK_Loans L
                JOIN AK_Books B ON L.Book_ID = B.ISBN
                WHERE L.User_ID = @UserId AND L.Returned = 0";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    using (var adapter = new SqlDataAdapter(command))
                    {
                        var dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        lvBooks.ItemsSource = dataTable.DefaultView;
                    }
                }
            }
        }

        private void ReturnBook_Click(object sender, RoutedEventArgs e)
        {
            if (lvBooks.SelectedItem != null)
            {
                var selectedLoan = (DataRowView)lvBooks.SelectedItem;
                int loanId = (int)selectedLoan["Loan_ID"];
                int fine = (int)selectedLoan["Fine"];

                using (var connection = new SqlConnection(info.connectionString))
                {
                    connection.Open();
                    // Обновляем статус возврата книги
                    var updateLoan = "UPDATE AK_Loans SET Returned = 1 WHERE Loan_ID = @LoanId";
                    using (var updateCommand = new SqlCommand(updateLoan, connection))
                    {
                        updateCommand.Parameters.AddWithValue("@LoanId", loanId);
                        updateCommand.ExecuteNonQuery();
                    }

                    // Добавляем информацию о штрафе, если есть
                    if (fine > 0)
                    {
                        var insertFine = "INSERT INTO AK_Fines (Loan_ID, Fine_Amount, Reason) VALUES (@LoanId, @Fine, 'Просроченный возврат')";
                        using (var insertCommand = new SqlCommand(insertFine, connection))
                        {
                            insertCommand.Parameters.AddWithValue("@LoanId", loanId);
                            insertCommand.Parameters.AddWithValue("@Fine", fine);
                            insertCommand.ExecuteNonQuery();
                        }
                    }
                }

                MessageBox.Show($"Книга возвращена. Штраф: {fine} руб.", "Возврат завершён", MessageBoxButton.OK, MessageBoxImage.Information);

                // Перезагрузка списка книг для выбранного студента
                string fullName = cbStudents.SelectedItem.ToString();
                if (userNamesToIds.TryGetValue(fullName, out int userId))
                {
                    LoadBooksForStudent(userId);
                }
            }
            else
            {
                MessageBox.Show("Выберите книгу для возврата.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
