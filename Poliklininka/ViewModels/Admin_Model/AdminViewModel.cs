using Poliklininka.Core;
using Poliklininka.Models.Admin;
using Poliklininka.Services;
using Poliklininka.Services.Admin;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Poliklininka.ViewModels.Admin_Model;

public class AdminViewModel : BaseViewModel
{
    private readonly IAdminAdoService _adminService;

    public ObservableCollection<string> Roles { get; } = new()
    {
        "Patient",
        "Doctor",
        "Admin",
        "Registrar"
    };

    private ObservableCollection<AdminUserDto> _users = new();
    public ObservableCollection<AdminUserDto> Users
    {
        get => _users;
        set => SetProperty(ref _users, value);
    }

    private AdminUserDto? _selectedUser;
    public AdminUserDto? SelectedUser
    {
        get => _selectedUser;
        set => SetProperty(ref _selectedUser, value);
    }

    public ICommand RefreshCommand { get; }
    public ICommand NewUserCommand { get; }
    public ICommand AddUserCommand { get; }
    public ICommand UpdateUserCommand { get; }
    public ICommand DeleteUserCommand { get; }

    public AdminViewModel(IAdminAdoService adminService)
    {
        _adminService = adminService;

        RefreshCommand = new RelayCommand(_ => _ = LoadUsersAsync());
        NewUserCommand = new RelayCommand(_ => NewUser());
        AddUserCommand = new RelayCommand(_ => _ = AddUserAsync());
        UpdateUserCommand = new RelayCommand(_ => _ = UpdateUserAsync());
        DeleteUserCommand = new RelayCommand(_ => _ = DeleteUserAsync());

        _ = LoadUsersAsync();
    }

    private async Task LoadUsersAsync()
    {
        try
        {
            var users = await _adminService.GetUsersAsync();
            Users = new ObservableCollection<AdminUserDto>(users);
            SelectedUser = Users.FirstOrDefault();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                ex.Message,
                "Ошибка загрузки пользователей",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }

    private void NewUser()
    {
        SelectedUser = new AdminUserDto
        {
            Role = "Patient",
            Discriminator = "Patient"
        };
    }

    private async Task AddUserAsync()
    {
        if (SelectedUser == null)
        {
            MessageBox.Show("Заполните данные пользователя.");
            return;
        }

        if (!ValidateUser(SelectedUser))
        {
            return;
        }

        try
        {
            await _adminService.AddUserAsync(SelectedUser);
            await LoadUsersAsync();

            MessageBox.Show(
                "Пользователь успешно добавлен.",
                "Готово",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                ex.Message,
                "Ошибка добавления пользователя",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }

    private async Task UpdateUserAsync()
    {
        if (SelectedUser == null || SelectedUser.Id == 0)
        {
            MessageBox.Show("Выберите пользователя из списка.");
            return;
        }

        if (!ValidateUser(SelectedUser))
        {
            return;
        }

        try
        {
            await _adminService.UpdateUserAsync(SelectedUser);
            await LoadUsersAsync();

            MessageBox.Show(
                "Пользователь успешно обновлен.",
                "Готово",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                ex.Message,
                "Ошибка редактирования пользователя",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }

    private async Task DeleteUserAsync()
    {
        if (SelectedUser == null || SelectedUser.Id == 0)
        {
            MessageBox.Show("Выберите пользователя из списка.");
            return;
        }

        var result = MessageBox.Show(
            "Удалить выбранного пользователя?",
            "Подтверждение",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (result != MessageBoxResult.Yes)
        {
            return;
        }

        try
        {
            await _adminService.DeleteUserAsync(SelectedUser.Id);
            await LoadUsersAsync();

            MessageBox.Show(
                "Пользователь удален.",
                "Готово",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                ex.Message,
                "Ошибка удаления пользователя",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }

    private bool ValidateUser(AdminUserDto user)
    {
        if (string.IsNullOrWhiteSpace(user.FullName))
        {
            MessageBox.Show("Введите ФИО пользователя.");
            return false;
        }

        if (string.IsNullOrWhiteSpace(user.Login))
        {
            MessageBox.Show("Введите логин пользователя.");
            return false;
        }

        if (string.IsNullOrWhiteSpace(user.Password))
        {
            MessageBox.Show("Введите пароль пользователя.");
            return false;
        }

        if (string.IsNullOrWhiteSpace(user.Role))
        {
            MessageBox.Show("Выберите роль пользователя.");
            return false;
        }

        if (user.Role == "Patient")
        {
            if (string.IsNullOrWhiteSpace(user.InsurancePolicy))
            {
                MessageBox.Show("Для пациента нужно указать полис ОМС.");
                return false;
            }
        }

        if (user.Role == "Doctor")
        {
            if (string.IsNullOrWhiteSpace(user.Specialization))
            {
                MessageBox.Show("Для врача нужно указать специализацию.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(user.Office))
            {
                MessageBox.Show("Для врача нужно указать кабинет.");
                return false;
            }
        }

        return true;
    }
}