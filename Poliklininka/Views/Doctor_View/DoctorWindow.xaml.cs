using System.Windows;
using Poliklininka.ViewModels.Doctor_Model;

namespace Poliklininka.Views.Doctor_View
{
    public partial class DoctorWindow : Window
    {
        public DoctorWindow(DoctorViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}