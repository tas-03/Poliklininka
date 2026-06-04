using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Newtonsoft.Json.Linq;
using Poliklininka.Core;
using Poliklininka.Entities;
using Poliklininka.Helpers;
using Poliklininka.Infrastructure.EF;
using Poliklininka.Services;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Poliklininka.ViewModels.Patient_Model;

public class PatientViewModel : BaseViewModel
{

    private readonly User _user;
    private  Patient? _patient;
    private readonly IPatientService _patientService;

    #region Анкета
    
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
    #endregion

    #region Мед.данные
    private string _groupBlood;
    public string GroupBlood
    {
        get => _groupBlood;
        set => SetProperty(ref _groupBlood, value);
    }
    private string _bloodFator;
    public string BloodFator
    {
        get => _bloodFator;
        set => SetProperty(ref _bloodFator, value);
    }

    private DateOnly _burthDate;
    public DateOnly BurthDate
    {
        get => _burthDate;
        set => SetProperty(ref _burthDate, value);
    }
    private string _disability;
    public string Disability
    {
        get => _disability;
        set => SetProperty(ref _disability, value);
    }

    private List<Allergy> _allergies;
    public List<Allergy> Allergies
    {
        get => _allergies;
        set =>SetProperty(ref _allergies, value);
       
    }
    private List<ChronicDiseases> _chronicDiseases;
    public List<ChronicDiseases> ChronicDiseases
    {
        get => _chronicDiseases;
        set => SetProperty(ref _chronicDiseases, value);
    }
    #endregion

    #region История посещений

    private DateOnly _date;
    public DateOnly Date
    {
        get => _date;
        set => SetProperty(ref _date, value);
    }


    private string _doctorName;
    public string DoctorName
    {
        get => _doctorName;
        set => SetProperty(ref _doctorName, value);
    }


    private string _medServiceName;
    public string MedServiceName
    {
        get => _medServiceName;
        set => SetProperty(ref _medServiceName, value);
    }


    private string _visitResults;
    public string VisitResults
    {
        get => _visitResults;
        set => SetProperty(ref _visitResults, value);
    }


    private List<AnalysisHistory> _analyses;
    public List<AnalysisHistory> Analyses
    {
        get => _analyses;
        set => SetProperty(ref _analyses, value);
    }
    private ObservableCollection<VisitHistory> _visitHistories;
    public ObservableCollection<VisitHistory> VisitHistories
    {
        get => _visitHistories;
        set 
            {
            SetProperty(ref _visitHistories, value);
            MessageBox.Show($"VisitHistories установлен: {value?.Count}");
        }
            
    }

    private List<RecipeHistory> _recipes;
   public List<RecipeHistory> Recipes
    {
        get => _recipes;
        set => SetProperty(ref _recipes, value);
    }

    private VisitHistory _selectedVisit;
    public VisitHistory SelectedVisit
    {
        get => _selectedVisit;
        set 
        { 
            SetProperty(ref _selectedVisit, value);
            LoadVisitDetails(value);
        }
    }
    #endregion

    public PatientViewModel(User user, IPatientService patientService)
    {
        _user = user;
        _patientService = patientService;
        _ = LoadData();
        OpenRedAnketaCommand = new RelayCommand(
            _ =>
            {
                if (_patient == null)
                {
                    MessageBox.Show("Данные пациента ещё не загружены.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                var viewModel = new Anketa_RedViewModel(_user, _patient, _patientService);
                var window = new Anketa_Red(viewModel);
                viewModel.OnSaveSuccess += () => _ = LoadData();
                window.Show();
            });
    }

    public void LoadVisitDetails(VisitHistory visitHistory)
    {
        if(visitHistory != null)
        {

            Date= SelectedVisit.VisitDate;
            DoctorName = SelectedVisit.Appointment.Doctor.Full_Name;
            MedServiceName = SelectedVisit.MedService.ServiceName;
            VisitResults = SelectedVisit.VisitResults;
            Analyses = SelectedVisit.AnalysisHistories.ToList();
            Recipes = SelectedVisit.RecipeHistories.ToList();

        }   
    }
    private async Task LoadData()
    {
       
            var patient_medcard = await _patientService.GetMedCardByUserIdAsync(_user.Id);
       
            if (patient_medcard == null)
            {
                MessageBox.Show("Пациент не найден в базе данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // подгружаем историю посещений
            var visitHistory = await _patientService.GetVisitHistoryByUserIdAsync(patient_medcard.Id);
      
        if (visitHistory != null )
        {
            VisitHistories = new ObservableCollection<VisitHistory>(visitHistory);
        }

        _patient = patient_medcard;
            FullName = _user.Full_Name;
            PhoneNumber = patient_medcard.Phone_number;
            Address = patient_medcard.Address;
            InsurancePolicy = patient_medcard.Insurance_Policy;
            Photo = PhotoCoverter.ConvertToImageSourse(patient_medcard.Photo);
   

        if (patient_medcard.MedCard != null)
            {
                Disability = (patient_medcard.MedCard.Disability ?? false) ? "Есть" : "Нет";
                GroupBlood = patient_medcard.MedCard.BloodGroup?.Name ?? "Не указана";
                Allergies = patient_medcard.MedCard.AllergyPatient.Select(a => a.Allergy).ToList();
                BurthDate = patient_medcard.MedCard.DateOfBirth;
                ChronicDiseases= patient_medcard.MedCard.HronicDiseasesPatient.Select(a => a.ChronicDiseases).ToList();
                BloodFator = patient_medcard.MedCard.BloodGroup?.RhFactor ?? "";
            }
    }

 

   public ICommand OpenRedAnketaCommand { get; }


   
}
