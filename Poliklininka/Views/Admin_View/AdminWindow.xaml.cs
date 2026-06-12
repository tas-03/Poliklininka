using Poliklininka.ViewModels.Admin_Model;
using System.Windows;

namespace Poliklininka.Views.Admin_View;

public partial class AdminWindow : Window
{
    public AdminWindow(AdminViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}