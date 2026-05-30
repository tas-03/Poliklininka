using Poliklininka.ViewModels.Patient_Model;
using System.Windows;

namespace Poliklininka
{
    /// <summary>
    /// Логика взаимодействия для Anketa_Red.xaml
    /// </summary>
    public partial class Anketa_Red : Window
    {
        public Anketa_Red(Anketa_RedViewModel anketa_RedViewModel)
        {

            InitializeComponent();
            DataContext = anketa_RedViewModel;
            anketa_RedViewModel.OnSaveSuccess += ()=> this.Close();

        }
    }
}
