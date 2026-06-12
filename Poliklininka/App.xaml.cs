using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Poliklininka.Infrastructure.EF;
using Poliklininka.Services;
using Poliklininka.ViewModels.Auth_Model;
using System.Windows;
using Poliklininka.Services.Admin;
using Poliklininka.ViewModels.Admin_Model;
using Poliklininka.Views.Admin_View;

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
        services.AddScoped<IAdminAdoService>(_ => new AdminAdoService(connectionString));
        services.AddTransient<AdminViewModel>();
        services.AddTransient<AdminWindow>();

        _serviceProvider = services.BuildServiceProvider();

        using (var scope = _serviceProvider.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            db.Database.Migrate();
        }

        var loginWindow = _serviceProvider.GetRequiredService<Login_Window>();

        loginWindow.Show();
    }
}
