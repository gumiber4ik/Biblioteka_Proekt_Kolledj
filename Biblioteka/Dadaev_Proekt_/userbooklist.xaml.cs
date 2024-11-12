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
    /// Логика взаимодействия для userbooklist.xaml
    /// </summary>
    public partial class userbooklist : Window
    {
        public userbooklist()
        {
            InitializeComponent();
            ShowAllBooks_Click(null, null); // По умолчанию показываем все книги
        }

        private void LoadBooksForStudent(int userId, string additionalCondition)
        {
            using (SqlConnection connection = new SqlConnection(info.connectionString))
            {
                connection.Open();
                string query = $@"
            SELECT 
                B.Title, 
                A.First_Name + ' ' + A.Last_Name AS Author, 
                B.ISBN, 
                L.Issue_Date, 
                L.Return_Date
            FROM dbo.AK_Loans L
            JOIN dbo.AK_Books B ON L.Book_ID = B.ISBN
            JOIN dbo.AK_Authors A ON B.Author_ID = A.Author_ID
            WHERE L.User_ID = @UserID {additionalCondition}
            ORDER BY L.Return_Date DESC, L.Issue_Date DESC;";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserID", userId);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                ListViewBooks.ItemsSource = dataTable.DefaultView;
            }
        }

        private void ShowAllBooks_Click(object sender, RoutedEventArgs e)
        {
            LoadBooksForStudent(info.ID, ""); // Пустая строка для условия, показать все книги
        }

        private void ShowDueBooks_Click(object sender, RoutedEventArgs e)
        {
            LoadBooksForStudent(info.ID, "AND L.Returned = 0 AND L.Return_Date < GETDATE()"); // Только невозвращенные книги, которые просрочены
        }
    }

}
