using Poliklininka.ViewModels.Patient_Model;
using System.Windows;

namespace Poliklininka;


public partial class PatientWindow : Window
{
    public PatientWindow(PatientViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }


}
