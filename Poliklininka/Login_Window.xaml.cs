using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Windows;

namespace Poliklininka;

/// <summary>
/// Логика взаимодействия для Login_Window.xaml
/// </summary>
public partial class Login_Window : Window
{
    private readonly DbContextOptions<ApplicationDbContext> _options;
    public Login_Window()
    {
        InitializeComponent();
        var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        var connectionString = config.GetConnectionString("Default");
        _options = new DbContextOptionsBuilder<ApplicationDbContext>().UseNpgsql(connectionString).Options;

    }

    private async void Button_Click(object sender, RoutedEventArgs e)
    {
        using var context = new ApplicationDbContext(_options);
        string Login = login.Text;
        string Parol = parol.Text;
        var existing_user = await context.Users.FirstOrDefaultAsync(x => x.Login == Login && x.Password == Parol);
        if (existing_user != null)
        {
            switch (existing_user.Role)
            {
                case "Admin":
                    new AdminWindow(existing_user).Show();
                    this.Close();
                    break;
                case "Patient":
                    new PatientWindow(existing_user).Show();
                    this.Close();
                    break;
                case "Doctor":
                    new DoctorWindow(existing_user).Show();
                    this.Close();
                    break;
            }
        }
        else
        {
            MessageBox.Show("Вы не зарегистрированы! Обратитесь с администратору!");
        }

    }
}