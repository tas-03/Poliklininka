using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Poliklininka.Entities;
using Poliklininka.Infrastructure.EF;
using Poliklininka.ViewModels.Patient_Model;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Poliklininka;


public partial class PatientWindow : Window
{
    public PatientWindow(PatientViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }


}
