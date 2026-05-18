using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Poliklininka.Entities;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Poliklininka;


public partial class PatientWindow : Window
{
    private User _user;
   // private Patient? _patient;
    private readonly DbContextOptions<ApplicationDbContext> _options;
    public PatientWindow(User user)
    {
        var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        var connectionString = config.GetConnectionString("Default");
        _options = new DbContextOptionsBuilder<ApplicationDbContext>().UseNpgsql(connectionString).Options;
        InitializeComponent();
         _user = user;
        LoadAnketa();
    }
    private void Tab_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (TabАнкета.IsSelected)
            LoadAnketa();
        //else if (TabМедкарта.IsSelected)
        //    LoadMedCard();
        //else if (TabИстория.IsSelected)
        //    LoadHistory();
        //else if (TabЗаписи.IsSelected)
        //    LoadЗаписи();
    }

    public void LoadAnketa()
    {
        using var context = new ApplicationDbContext(_options);
        var patient = context.Patients.FirstOrDefault(e=>e.Id == _user.Id);
        if (patient == null) return;
        FIO_1.Text = _user.Full_Name;
        polis.Text=patient.Insurance_Policy;
        phone.Text=patient.Phone_number;
        adress.Text = patient.Address;
        if (patient.Photo != null)
        {
            var image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = new MemoryStream(patient.Photo);
            image.EndInit();
            Photo_Prof.Source = image;
        }
        else
        {
            Photo_Prof.Source = new BitmapImage(new Uri("Assets/No_Ava.jpg", UriKind.Relative));
        }
    }


    private void Button_Click(object sender, RoutedEventArgs e)
    {
  
        var editWindow = new Anketa_Red(_user);
        editWindow.Closed += (s, e) => LoadAnketa();  // ← когда закроется — перезагрузит
        editWindow.Show();
    }
}
