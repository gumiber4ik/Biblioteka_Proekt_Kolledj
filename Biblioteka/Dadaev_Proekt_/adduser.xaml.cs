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

namespace Dadaev_Proekt_
{
    /// <summary>
    /// Логика взаимодействия для adduser.xaml
    /// </summary>
    public partial class adduser : Window
    {
        public adduser()
        {
            InitializeComponent();
        }
        private bool ValidateLoginAndPassword(string login, string password, string confirmPassword)
        {
            // Проверяем длину логина и пароля
            if (login.Length < 8 || login.Length > 50)
            {
                MessageBox.Show("Логин должен содержать от 8 до 50 символов.", "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (password.Length < 8 || password.Length > 100)
            {
                MessageBox.Show("Пароль должен содержать от 8 до 100 символов.", "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            // Проверяем, совпадают ли пароль и подтверждение пароля
            if (password != confirmPassword)
            {
                MessageBox.Show("Пароль и подтверждение пароля не совпадают.", "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true; // Все проверки пройдены успешно
        }

        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            string firstName = firstNameTextBox.Text;
            string lastName = lastNameTextBox.Text;
            string login = loginTextBox.Text;
            string password = passwordBox.Password;
            string confirmPassword = confirmPasswordBox.Password;
            ComboBoxItem selectedRoleItem = roleComboBox.SelectedItem as ComboBoxItem;
            int role = Convert.ToInt32(selectedRoleItem.Tag);

            if (!ValidateLoginAndPassword(login, password, confirmPassword) || roleComboBox.SelectedItem == null)
            {
                MessageBox.Show("Ошибка ввода данных. Проверьте правильность заполнения полей.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if (InsertUserIntoDatabase(firstName, lastName, login, password, role))
                {
                    MessageBox.Show("Пользователь успешно добавлен.", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                    ClearForm();
                }
                else
                {
                    MessageBox.Show("Не удалось добавить пользователя. Проверьте данные и повторите попытку.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private bool InsertUserIntoDatabase(string firstName, string lastName, string login, string password, int role)
        {
            using (SqlConnection connection = new SqlConnection(info.connectionString))
            {
                try
                {
                    connection.Open();
                    string query = @"INSERT INTO AK_Users (First_Name, Last_Name, Login, Password, Role_ID) 
                             VALUES (@FirstName, @LastName, @Login, @Password, @Role)";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@FirstName", firstName);
                    command.Parameters.AddWithValue("@LastName", lastName);
                    command.Parameters.AddWithValue("@Login", login);
                    command.Parameters.AddWithValue("@Password", password);
                    command.Parameters.AddWithValue("@Role", role);
                    command.ExecuteNonQuery();
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при добавлении пользователя: " + ex.Message);
                    return false;
                }
            }
        }

        private void ClearForm()
        {
            firstNameTextBox.Clear();
            lastNameTextBox.Clear();
            loginTextBox.Clear();
            passwordBox.Clear();
            confirmPasswordBox.Clear();
            roleComboBox.SelectedIndex = -1;  // Сброс выбранной роли
        }

        private void SearchUser_Click(object sender, RoutedEventArgs e)
        {
            string searchText = searchTextBox.Text;
            using (SqlConnection connection = new SqlConnection(info.connectionString))
            {
                connection.Open();
                string query = "SELECT User_ID, First_Name, Last_Name, Login, Role_ID FROM AK_Users WHERE First_Name + ' ' + Last_Name = @SearchText";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@SearchText", searchText);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        firstNameTextBox.Text = reader["First_Name"].ToString();
                        lastNameTextBox.Text = reader["Last_Name"].ToString();
                        loginTextBox.Text = reader["Login"].ToString();
                        // Установка выбранной роли в ComboBox
                        int roleID = Convert.ToInt32(reader["Role_ID"]);
                        roleComboBox.SelectedValue = roleID.ToString();  // Убедитесь, что Tag строки ComboBoxItem является строкой

                        // Визуализация защищённого пароля (подумайте о безопасности)
                        passwordBox.Password = ""; // Не показываем пароль
                        confirmPasswordBox.Password = "";
                    }
                    else
                    {
                        MessageBox.Show("Пользователь не найден.", "Поиск", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
        }

        private void EditUser_Click(object sender, RoutedEventArgs e)
        {
            // Пример обновления данных пользователя
            using (SqlConnection connection = new SqlConnection(info.connectionString))
            {
                connection.Open();
                string query = @"UPDATE AK_Users SET First_Name=@FirstName, Last_Name=@LastName, Login=@Login, Password=@Password, Role_ID=@Role WHERE First_Name=@FirstName AND Last_Name=@LastName";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@FirstName", firstNameTextBox.Text);
                command.Parameters.AddWithValue("@LastName", lastNameTextBox.Text);
                command.Parameters.AddWithValue("@Login", loginTextBox.Text);
                command.Parameters.AddWithValue("@Password", passwordBox.Password); // Простая замена в примере
                command.Parameters.AddWithValue("@Role", ((ComboBoxItem)roleComboBox.SelectedItem).Tag);
                int result = command.ExecuteNonQuery();
                if (result > 0)
                {
                    MessageBox.Show("Пользователь обновлен.", "Обновление", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Не удалось обновить пользователя.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void DeleteUser_Click(object sender, RoutedEventArgs e)
        {
            // Пример удаления пользователя
            using (SqlConnection connection = new SqlConnection(info.connectionString))
            {
                connection.Open();
                string query = "DELETE FROM AK_Users WHERE First_Name=@FirstName AND Last_Name=@LastName";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@FirstName", firstNameTextBox.Text);
                command.Parameters.AddWithValue("@LastName", lastNameTextBox.Text);
                int result = command.ExecuteNonQuery();
                if (result > 0)
                {
                    MessageBox.Show("Пользователь удален.", "Удаление", MessageBoxButton.OK, MessageBoxImage.Information);
                    ClearForm();
                }
                else
                {
                    MessageBox.Show("Не удалось удалить пользователя.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        }
}
