using Poliklininka.Core;

namespace Poliklininka.Models.Admin;

public class AdminUserDto : BaseViewModel
{
    private int _id;
    public int Id
    {
        get => _id;
        set => SetProperty(ref _id, value);
    }

    private string _login = string.Empty;
    public string Login
    {
        get => _login;
        set => SetProperty(ref _login, value);
    }

    private string _password = string.Empty;
    public string Password
    {
        get => _password;
        set => SetProperty(ref _password, value);
    }

    private string _fullName = string.Empty;
    public string FullName
    {
        get => _fullName;
        set => SetProperty(ref _fullName, value);
    }

    private string _role = "Patient";
    public string Role
    {
        get => _role;
        set => SetProperty(ref _role, value);
    }

    private string _discriminator = "Patient";
    public string Discriminator
    {
        get => _discriminator;
        set => SetProperty(ref _discriminator, value);
    }

    // Поля пациента
    private string _phoneNumber = string.Empty;
    public string PhoneNumber
    {
        get => _phoneNumber;
        set => SetProperty(ref _phoneNumber, value);
    }

    private string _insurancePolicy = string.Empty;
    public string InsurancePolicy
    {
        get => _insurancePolicy;
        set => SetProperty(ref _insurancePolicy, value);
    }

    private string _address = string.Empty;
    public string Address
    {
        get => _address;
        set => SetProperty(ref _address, value);
    }

    // Поля врача
    private string _specialization = string.Empty;
    public string Specialization
    {
        get => _specialization;
        set => SetProperty(ref _specialization, value);
    }

    private string _office = string.Empty;
    public string Office
    {
        get => _office;
        set => SetProperty(ref _office, value);
    }
}