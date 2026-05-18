using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Win32;
using Poliklininka.Entities;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Poliklininka
{
    /// <summary>
    /// Логика взаимодействия для Anketa_Red.xaml
    /// </summary>
    public partial class Anketa_Red : Window
    {
        private User _user;
        //private Patient _patient;
        private byte[]? _photoBytes;
        private readonly DbContextOptions<ApplicationDbContext> _options;
        public Anketa_Red(User user)
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var connectionString = config.GetConnectionString("Default");
            _options = new DbContextOptionsBuilder<ApplicationDbContext>().UseNpgsql(connectionString).Options;
            _user = user;
            InitializeComponent();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using var context = new ApplicationDbContext(_options);

            var patient = context.Patients.FirstOrDefault(e => e.Id == _user.Id);
            if (patient == null) return;
            patient.Phone_number = Phone.Text;
            patient.Address = Adress.Text;
            patient.Photo = _photoBytes;
            context.SaveChanges();
            MessageBox.Show("Данные сохранены!");

            this.Close();
        }

        private void Button_Photo_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "Изображения|*.jpg;*.jpeg;*.png";
            if (dialog.ShowDialog() == true)
            {
                _photoBytes = File.ReadAllBytes(dialog.FileName);
                var image = new BitmapImage();
                image.BeginInit();
                image.StreamSource = new MemoryStream(_photoBytes);
                image.EndInit();
                Avatar.Source = image;
            }
        }


    }
}
