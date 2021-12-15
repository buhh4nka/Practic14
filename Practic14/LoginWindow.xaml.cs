using System;
using System.Windows;

namespace Practic14
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
       
        public LoginWindow()
        {
            InitializeComponent();
            password.Focus();
        }

        private void enter_Click(object sender, RoutedEventArgs e)
        {
            if (password.Password == "123") this.Close();
            else
            {
                MessageBox.Show("Неверный пароль", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                password.Focus();
            }
        }

       
    }
}
