using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using System.Windows;
using System.Data.SqlClient;

namespace Dadaev_Proekt_
{
    public partial class user : Window
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int RoleID { get; set; } // Добавляем свойство для Role_ID

        public user()
        {
            InitializeComponent();
            LoadUserData();
        }

        private void LoadUserData()
        {
            using (SqlConnection connection = new SqlConnection(info.connectionString))
            {
                connection.Open();
                string query = "SELECT First_Name, Last_Name, Login, Role_ID FROM AK_Users WHERE User_ID = @UserID"; // Добавляем получение Role_ID
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserID", info.ID);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        FirstName = reader["First_Name"]?.ToString();
                        LastName = reader["Last_Name"]?.ToString();
                        RoleID = Convert.ToInt32(reader["Role_ID"]); // Присваиваем Role_ID

                        if (RoleID == 3) // Проверяем, является ли пользователь студентом
                        {
                            BooksLabel.Visibility = Visibility.Visible;
                            GoToBooksButton.Visibility = Visibility.Visible;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Пользователь не найден.");
                    }
                }
            }

            DataContext = this;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var readerWindow = new userbooklist();
            readerWindow.Show();
        }
    }
}