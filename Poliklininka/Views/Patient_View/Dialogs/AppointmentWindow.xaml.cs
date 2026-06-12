using Poliklininka.ViewModels.Patient_Model;
using System.Windows;

namespace Poliklininka.Views.Patient_View.Dialogs;

public partial class AppointmentWindow : Window
{
    public AppointmentWindow(AppointmentWindowViewModel viewModel)
    {
        InitializeComponent();

        DataContext = viewModel;

        viewModel.RequestClose += Close;
    }
}