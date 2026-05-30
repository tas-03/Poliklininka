using Poliklininka.ViewModels.Auth_Model;
using System.Windows;

namespace Poliklininka;

/// <summary>
/// Логика взаимодействия для Login_Window.xaml
/// </summary>
public partial class Login_Window : Window
{


    public Login_Window(AuthModel authModel)
    {
        InitializeComponent();
        DataContext = authModel;
        authModel.OnLoginSuccess += () => this.Close();
    }


}