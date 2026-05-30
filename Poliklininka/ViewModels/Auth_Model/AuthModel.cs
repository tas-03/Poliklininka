using Poliklininka.Core;
using Poliklininka.Entities;
using Poliklininka.Services;
using Poliklininka.ViewModels.Patient_Model;
using System.Windows;
using System.Windows.Input;

namespace Poliklininka.ViewModels.Auth_Model;

public class AuthModel : BaseViewModel
{
    private readonly IAuthService _authService;
    private readonly IPatientService _patientService;
    public event Action? OnLoginSuccess;

    private string _parol = string.Empty;
    public string Parol
    {
        get => _parol;
        set => SetProperty(ref _parol, value);
    }
    private string _login = string.Empty;
    public string Login
    {
        get => _login;
        set => SetProperty(ref _login, value);
    }

    public AuthModel(IAuthService authService, IPatientService patientService)
    {
        _authService = authService;
        _patientService = patientService;
        AuthCommand = new RelayCommand(
           async _ => {
              var user = await authService.LoginAsync(Login, Parol);
               if (user != null) { 
               OpenWindow(user);
               }
               else
               {
                   MessageBox.Show("Вы не зарегистрированы! Обратитесь с администратору!");
               }

               },
            _ => !string.IsNullOrWhiteSpace(Login) && !string.IsNullOrWhiteSpace(Parol));

    }
   public ICommand AuthCommand { get; }

 
    public void OpenWindow(User user)
    {
        switch (user.Role)
        {
            case "Patient":
                var viewModel = new PatientViewModel(user, _patientService);
                var PatientWindow = new PatientWindow(viewModel);
                PatientWindow.Show();
                OnLoginSuccess?.Invoke();
                break;
        }
    }
}
