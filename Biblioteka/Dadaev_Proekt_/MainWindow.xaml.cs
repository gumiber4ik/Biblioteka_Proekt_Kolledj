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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Dadaev_Proekt_
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void Aut(object sender, RoutedEventArgs e)
        {
            string login = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            info user = AuthenticateUser(login, password);
            
            if (user != null)
            {
                info.ID = user.UserId;
                MessageBox.Show($"Добро пожаловать, {user.FirstName}! Ваша роль: {user.RoleName}", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                // Проверка роли пользователя и открытие соответствующего окна
                if (user.RoleName == "Админ")
                {
                    var readerWindow = new admin();
                    readerWindow.Show();
                }
                else if (user.RoleName == "Библиотекарь")
                {
                    
                    var readerWindow = new bibliotecar();
                    readerWindow.Show();
                }
                else if (user.RoleName == "Студент")
                {
                    
                    var readerWindow = new student();
                    readerWindow.Show();
                }
                else
                {
                    MessageBox.Show("Неизвестная роль. Пожалуйста, свяжитесь с администратором.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                }

                // Закрываем окно авторизации
                this.Close();
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private info AuthenticateUser(string login, string password)
        {
            info authenticatedUser = null;

            using (SqlConnection connection = new SqlConnection(info.connectionString))
            {
                connection.Open();
                string query = @"SELECT U.User_ID, U.First_Name, U.Last_Name, R.Role_Name 
FROM dbo.AK_Users U
JOIN dbo.AK_Role R ON U.Role_ID = R.K_Role_ID
WHERE U.Login = @Login AND U.Password = @Password;
";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Login", login);
                    command.Parameters.AddWithValue("@Password", password);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            authenticatedUser = new info
                            {
                               
                                UserId = reader.GetInt32(0),
                                FirstName = reader.GetString(1),
                                LastName = reader.GetString(2),
                                RoleName = reader.GetString(3)
                            };
                        }
                    }
                }
            }

            return authenticatedUser;
        }

    }
}
