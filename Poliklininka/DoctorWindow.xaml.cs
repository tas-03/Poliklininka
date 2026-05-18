using System.Windows;
using Poliklininka.Entities;

namespace Poliklininka;

/// <summary>
/// Логика взаимодействия для DoctorWindow.xaml
/// </summary>
public partial class DoctorWindow : Window
{
    public DoctorWindow(User user)
    {
        InitializeComponent();
    }
}
