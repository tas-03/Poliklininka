using Microsoft.Win32;
using Poliklininka.Core;
using Poliklininka.Entities;
using Poliklininka.Helpers;
using Poliklininka.Services;
using System.IO;
using System.Windows.Input;
using System.Windows.Media;

namespace Poliklininka.ViewModels.Patient_Model;

public class Anketa_RedViewModel : BaseViewModel
{
    private readonly IPatientService _patientService;
    private User _user;
    private Patient _patient;
    public event Action? OnSaveSuccess;

    private ImageSource? _photo;
    public ImageSource? Photo
    {
        get=>  _photo;
        set => SetProperty(ref _photo, value);
    }
    private string _address = string.Empty;
    public string Address
    {
        get => _address;
        set => SetProperty(ref _address, value);
    }
    private string _phonenumber = string.Empty;
    public string Phonenumber
    {
        get => _phonenumber;
        set => SetProperty(ref _phonenumber, value);
    }
    public Anketa_RedViewModel(User user, Patient patient, IPatientService patientService)
    {
        _patientService= patientService;
        _user= user;
        _patient= patient;
        Photo = PhotoCoverter.ConvertToImageSourse(_patient.Photo);
        Address = _patient.Address;
        Phonenumber = _patient.Phone_number;
        SaveUpdateAnketa = new RelayCommand(
            async _ => await SaveChanges());
        NewPhotoCommand = new RelayCommand(
            _ => NewPhoto());
    }

    public  async Task SaveChanges()
    {
        _patient.Address = Address;
        _patient.Phone_number = Phonenumber;
        _patient.Photo = PhotoCoverter.ConvertToByteArray(Photo);
        await _patientService.UpdatePatientByUserIdAsync(_patient);
        OnSaveSuccess?.Invoke();
    }
    public ICommand SaveUpdateAnketa { get; }
   public ICommand NewPhotoCommand { get; }
    

    public void NewPhoto()
    {
        var dialog = new OpenFileDialog();
        dialog.Filter = "Изображения|*.jpg;*.jpeg;*.png";

        if (dialog.ShowDialog() == true)
        {
            var bytes = File.ReadAllBytes(dialog.FileName);
            _patient.Photo = bytes;
            Photo = PhotoCoverter.ConvertToImageSourse(bytes);
        }
    }
}
