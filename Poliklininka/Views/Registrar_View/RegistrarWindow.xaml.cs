using Poliklininka.ViewModels.Registrar_Model;
using System.Windows;

namespace Poliklininka.Views.Registrar_View
{

    public partial class RegistrarWindow : Window
    {
        public RegistrarWindow(RegistrarViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}

