using System.Windows;
using Poliklininka.Entities;

namespace Poliklininka
{
    /// <summary>
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        private User _user;
        public AdminWindow(User user)
        {
            _user = user;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new New_User(_user).Show();

        }
    }
}
