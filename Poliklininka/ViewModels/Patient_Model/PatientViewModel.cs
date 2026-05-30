using Poliklininka.Core;
using Poliklininka.Entities;
using Poliklininka.Helpers;
using Poliklininka.Infrastructure.EF;
using Poliklininka.Services;
using System.IO;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Poliklininka.ViewModels.Patient_Model;

public class PatientViewModel : BaseViewModel
{

    private readonly User _user;
    private  Patient? _patient;
    private readonly IPatientService _patientService;

    private string _fullName = string.Empty;
    public string FullName
    {
        get => _fullName;
        set => SetProperty(ref _fullName, value);
    }
    private string _phoneNumber = string.Empty;
    public string PhoneNumber
    {
        get => _phoneNumber;
        set => SetProperty(ref _phoneNumber, value);


    }
    private string _address = string.Empty;
    public string Address
    {
        get => _address;
        set => SetProperty(ref _address, value);
    }
    private string _insurancePolicy = string.Empty;
    public string InsurancePolicy
    {
        get => _insurancePolicy;
        set => SetProperty(ref _insurancePolicy, value);
    }
    private ImageSource? _photo;
    public ImageSource? Photo
    {
        get => _photo;
        set => SetProperty(ref _photo, value);
    }

    public PatientViewModel(User user, IPatientService patientService)
    {
        _user = user;
        _patientService = patientService;
        _=LoadData();
        OpenRedAnketaCommand = new RelayCommand(
            _ =>
            {
                var viewModel = new Anketa_RedViewModel(_user, _patient!, _patientService);
                var window = new Anketa_Red(viewModel);
                viewModel.OnSaveSuccess += () => _ = LoadData();
                window.Show();
            }
            );
    }
    private async Task LoadData()
    {
        var patient = await _patientService.GetPatientByUserIdAsync(_user.Id);
        if (patient != null)
        {
            _patient = patient;
            FullName = _user.Full_Name;
            PhoneNumber = patient.Phone_number;
            Address = patient.Address;
            InsurancePolicy = patient.Insurance_Policy;
            Photo = PhotoCoverter.ConvertToImageSourse(patient.Photo);
        }
    }

   public ICommand OpenRedAnketaCommand { get; }

     

}
