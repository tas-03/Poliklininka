using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Poliklininka.Infrastructure.EF;
using Poliklininka.Services;
using Poliklininka.ViewModels.Auth_Model;
using System.Windows;

namespace Poliklininka;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private IServiceProvider _serviceProvider;
    protected override void OnStartup(StartupEventArgs e)
    {

        base.OnStartup(e);

        var services = new ServiceCollection();

        var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

        var connectionString = config.GetConnectionString("Default");
        services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));

        services.AddScoped<IAuthService, EFAuthService>();
        services.AddScoped<IPatientService, EFPatientService>();

        services.AddTransient<AuthModel>();
        services.AddTransient<Login_Window>();

        _serviceProvider = services.BuildServiceProvider();

        var loginWindow = _serviceProvider.GetRequiredService<Login_Window>();

        loginWindow.Show();
    }
}
