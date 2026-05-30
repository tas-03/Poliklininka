using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Poliklininka.Entities;
using Poliklininka.Infrastructure.EF;
using System.Windows;
using System.Windows.Controls;

namespace Poliklininka
{
    /// <summary>
    /// Логика взаимодействия для New_User.xaml
    /// </summary>
    public partial class New_User : Window
    {
        private User _user;
        private readonly DbContextOptions<ApplicationDbContext> _options;
        private string _connectionString = "Host=localhost;Port=8080;Database=polikl;Username=postgres;Password=123";
        public New_User(User user)
        {
            _user = user;
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var connectionString = config.GetConnectionString("Default");
            _options = new DbContextOptionsBuilder<ApplicationDbContext>().UseNpgsql(connectionString).Options;
            InitializeComponent();
        }

        private void Role_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var role = (Role.SelectedItem as ComboBoxItem)?.Content.ToString();

            // сначала скрываем все
            DoctorPanel.Visibility = Visibility.Collapsed;
            Patient.Visibility = Visibility.Collapsed;

            // показываем нужный
            switch (role)
            {
                case "Доктор":
                    DoctorPanel.Visibility = Visibility.Visible;
                    break;
                case "Пациент":
                    Patient.Visibility = Visibility.Visible;
                    break;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using var context = new ApplicationDbContext(_options);
            //тут используем датасет 
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            var role = (Role.SelectedItem as ComboBoxItem)?.Content.ToString();
            var check = new NpgsqlCommand("SELECT COUNT(*) FROM \"Users\" WHERE login=@login", connection);
            check.Parameters.AddWithValue("login", login.Text);
            int count = Convert.ToInt32(check.ExecuteScalar());
            if (count > 0)
            {
                MessageBox.Show("Такой пользователь уже существует!");
                return;
            }
            if (role == "Доктор")
            {
                var cpecial = (Spec.SelectedItem as ComboBoxItem)?.Content.ToString();
                if (cpecial == null)
                {
                    MessageBox.Show("Выберите специализацию!");
                    return;
                }
                var new_doctor = new NpgsqlCommand("INSERT INTO \"Users\" (login, password,full_name, role, discriminator) VALUES (@login,@password,@full_name ,@role, @discriminator) RETURNING id", connection);
                new_doctor.Parameters.AddWithValue("login", login.Text);
                new_doctor.Parameters.AddWithValue("password", password.Text);
                new_doctor.Parameters.AddWithValue("full_name", fio.Text);
                new_doctor.Parameters.AddWithValue("role", "Doctor");
                new_doctor.Parameters.AddWithValue("discriminator", "Doctor");
                var doctorId = Convert.ToInt32(new_doctor.ExecuteScalar());

                var new_doctor2 = new NpgsqlCommand("INSERT INTO doctors (Id ,specialization, office) VALUES (@Id, @specialization, @office)", connection);
                new_doctor2.Parameters.AddWithValue("Id", doctorId);
                new_doctor2.Parameters.AddWithValue("specialization", cpecial);
                new_doctor2.Parameters.AddWithValue("office", Kabinet.Text);
                new_doctor2.ExecuteNonQuery();
                MessageBox.Show("Данные сохранены!");
                this.Close();
            }
            else if (role == "Пациент")
            {
                //маппинг наследования(автоматически добавляется запись и в users и в patients
                var patient = new Patient
                {
                    Login = login.Text,
                    Password = password.Text,
                    Full_Name = fio.Text,
                    Role = role,
                    Discriminator = role,
                    Phone_number = Phone.Text,
                    Insurance_Policy = Polic.Text,
                    Address = Adress.Text
                };
                context.Patients.Add(patient);
                context.SaveChanges();
                MessageBox.Show("Данные сохранены!");
                this.Close();

            }

            var new_admin = new NpgsqlCommand("INSERT INTO \"Users\" (login, password,full_name, role, discriminator) VALUES (@login,@password,@full_name ,@role, @discriminator) ", connection);
            new_admin.Parameters.AddWithValue("login", login.Text);
            new_admin.Parameters.AddWithValue("password", password.Text);
            new_admin.Parameters.AddWithValue("full_name", fio.Text);
            new_admin.Parameters.AddWithValue("role", "Admin");
            new_admin.Parameters.AddWithValue("discriminator", "Admin");
            new_admin.ExecuteNonQuery();
            MessageBox.Show("Данные сохранены!");
            this.Close();
        }
    }
}
