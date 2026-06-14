using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Poliklininka.Core;
using Poliklininka.Entities;
using Poliklininka.Infrastructure.NHibernate.Models;

namespace Poliklininka.ViewModels.Doctor_Model;

public class DoctorViewModel : BaseViewModel
{
    private readonly Poliklininka.Services.Hibernate.IDoctorHibernateService _doctorHibernateService;
    private readonly User _currentUser;

    private int _lastCreatedVisitHistoryId;

    public DoctorViewModel(
        User currentUser,
        Poliklininka.Services.Hibernate.IDoctorHibernateService doctorHibernateService)
    {
        _currentUser = currentUser;
        _doctorHibernateService = doctorHibernateService;

        DoctorId = currentUser.Id;
        DoctorName = currentUser.Full_Name;

        Appointments = new ObservableCollection<HibernateAppointment>();
        VisitHistories = new ObservableCollection<HibernateVisitHistory>();

        BloodGroups = new ObservableCollection<HibernateBloodGroup>();
        AllAllergies = new ObservableCollection<HibernateAllergy>();
        PatientAllergies = new ObservableCollection<HibernateAllergy>();

        AllChronicDiseases = new ObservableCollection<HibernateChronicDisease>();
        PatientChronicDiseases = new ObservableCollection<HibernateChronicDisease>();

        Analyses = new ObservableCollection<HibernateAnalysis>();
        Recipes = new ObservableCollection<HibernateRecipe>();

        AnalysisHistories = new ObservableCollection<HibernateAnalysisHistory>();
        RecipeHistories = new ObservableCollection<HibernateRecipeHistory>();

        LoadAppointmentsCommand = new RelayCommand(async _ => await LoadAppointmentsAsync());

        SaveMedCardCommand = new RelayCommand(
            async _ => await SaveMedCardAsync(),
            _ => SelectedAppointment != null);

        AddAllergyCommand = new RelayCommand(
            async _ => await AddAllergyAsync(),
            _ => SelectedAppointment != null && SelectedAllergy != null);

        AddChronicDiseaseCommand = new RelayCommand(
            async _ => await AddChronicDiseaseAsync(),
            _ => SelectedAppointment != null && SelectedChronicDisease != null);

        CreateVisitHistoryCommand = new RelayCommand(
            async _ => await CreateVisitHistoryAsync(),
            _ => SelectedAppointment != null && !string.IsNullOrWhiteSpace(VisitResults));

        AddAnalysisCommand = new RelayCommand(
            async _ => await AddAnalysisAsync(),
            _ => CurrentVisitHistoryId != null && SelectedAnalysis != null);

        AddRecipeCommand = new RelayCommand(
            async _ => await AddRecipeAsync(),
            _ => CurrentVisitHistoryId != null
                 && SelectedRecipe != null
                 && !string.IsNullOrWhiteSpace(RecipeDosage)
                 && !string.IsNullOrWhiteSpace(RecipeDuration));

        DeleteAnalysisCommand = new RelayCommand(
            async _ => await DeleteAnalysisAsync(),
            _ => SelectedAnalysisHistory != null);

        DeleteRecipeCommand = new RelayCommand(
            async _ => await DeleteRecipeAsync(),
            _ => SelectedRecipeHistory != null);

        _ = InitializeAsync();
    }

    public int DoctorId { get; }

    private string _doctorName = string.Empty;
    public string DoctorName
    {
        get => _doctorName;
        set => SetProperty(ref _doctorName, value);
    }

    public ObservableCollection<HibernateAppointment> Appointments { get; }

    public ObservableCollection<HibernateVisitHistory> VisitHistories { get; }

    public ObservableCollection<HibernateBloodGroup> BloodGroups { get; }

    public ObservableCollection<HibernateAllergy> AllAllergies { get; }

    public ObservableCollection<HibernateAllergy> PatientAllergies { get; }

    public ObservableCollection<HibernateChronicDisease> AllChronicDiseases { get; }

    public ObservableCollection<HibernateChronicDisease> PatientChronicDiseases { get; }

    public ObservableCollection<HibernateAnalysis> Analyses { get; }

    public ObservableCollection<HibernateRecipe> Recipes { get; }

    public ObservableCollection<HibernateAnalysisHistory> AnalysisHistories { get; }

    public ObservableCollection<HibernateRecipeHistory> RecipeHistories { get; }

    private HibernateAppointment? _selectedAppointment;
    public HibernateAppointment? SelectedAppointment
    {
        get => _selectedAppointment;
        set
        {
            if (SetProperty(ref _selectedAppointment, value))
            {
                _lastCreatedVisitHistoryId = 0;

                NotifySelectedAppointmentProperties();
                ClearPatientData();

                if (_selectedAppointment != null)
                    _ = LoadSelectedPatientDataAsync();

                RaiseCommands();
            }
        }
    }

    private HibernatePatient? _selectedPatient;
    public HibernatePatient? SelectedPatient
    {
        get => _selectedPatient;
        set
        {
            if (SetProperty(ref _selectedPatient, value))
            {
                NotifySelectedPatientProperties();
                RaiseCommands();
            }
        }
    }

    private HibernateMedCard? _selectedMedCard;
    public HibernateMedCard? SelectedMedCard
    {
        get => _selectedMedCard;
        set => SetProperty(ref _selectedMedCard, value);
    }

    private HibernateVisitHistory? _selectedVisitHistory;
    public HibernateVisitHistory? SelectedVisitHistory
    {
        get => _selectedVisitHistory;
        set
        {
            if (SetProperty(ref _selectedVisitHistory, value))
            {
                _lastCreatedVisitHistoryId = 0;

                LoadAssignmentsFromSelectedVisit();

                OnPropertyChanged(nameof(SelectedVisitDate));
                OnPropertyChanged(nameof(SelectedVisitTime));
                OnPropertyChanged(nameof(SelectedVisitResults));

                RaiseCommands();
            }
        }
    }

    private HibernateBloodGroup? _selectedBloodGroup;
    public HibernateBloodGroup? SelectedBloodGroup
    {
        get => _selectedBloodGroup;
        set => SetProperty(ref _selectedBloodGroup, value);
    }

    private DateTime _dateOfBirth = DateTime.Today;
    public DateTime DateOfBirth
    {
        get => _dateOfBirth;
        set => SetProperty(ref _dateOfBirth, value);
    }

    private bool _disability;
    public bool Disability
    {
        get => _disability;
        set => SetProperty(ref _disability, value);
    }

    private HibernateAllergy? _selectedAllergy;
    public HibernateAllergy? SelectedAllergy
    {
        get => _selectedAllergy;
        set
        {
            if (SetProperty(ref _selectedAllergy, value))
                RaiseCommands();
        }
    }

    private HibernateChronicDisease? _selectedChronicDisease;
    public HibernateChronicDisease? SelectedChronicDisease
    {
        get => _selectedChronicDisease;
        set
        {
            if (SetProperty(ref _selectedChronicDisease, value))
                RaiseCommands();
        }
    }

    private string _visitResults = string.Empty;
    public string VisitResults
    {
        get => _visitResults;
        set
        {
            if (SetProperty(ref _visitResults, value))
                RaiseCommands();
        }
    }

    private HibernateAnalysis? _selectedAnalysis;
    public HibernateAnalysis? SelectedAnalysis
    {
        get => _selectedAnalysis;
        set
        {
            if (SetProperty(ref _selectedAnalysis, value))
                RaiseCommands();
        }
    }

    private string _analysisResult = string.Empty;
    public string AnalysisResult
    {
        get => _analysisResult;
        set => SetProperty(ref _analysisResult, value);
    }

    private HibernateRecipe? _selectedRecipe;
    public HibernateRecipe? SelectedRecipe
    {
        get => _selectedRecipe;
        set
        {
            if (SetProperty(ref _selectedRecipe, value))
                RaiseCommands();
        }
    }

    private string _recipeDosage = string.Empty;
    public string RecipeDosage
    {
        get => _recipeDosage;
        set
        {
            if (SetProperty(ref _recipeDosage, value))
                RaiseCommands();
        }
    }

    private string _recipeDuration = string.Empty;
    public string RecipeDuration
    {
        get => _recipeDuration;
        set
        {
            if (SetProperty(ref _recipeDuration, value))
                RaiseCommands();
        }
    }

    private HibernateAnalysisHistory? _selectedAnalysisHistory;
    public HibernateAnalysisHistory? SelectedAnalysisHistory
    {
        get => _selectedAnalysisHistory;
        set
        {
            if (SetProperty(ref _selectedAnalysisHistory, value))
                RaiseCommands();
        }
    }

    private HibernateRecipeHistory? _selectedRecipeHistory;
    public HibernateRecipeHistory? SelectedRecipeHistory
    {
        get => _selectedRecipeHistory;
        set
        {
            if (SetProperty(ref _selectedRecipeHistory, value))
                RaiseCommands();
        }
    }

    public string SelectedPatientName =>
        SelectedPatient?.Full_Name ?? SelectedAppointment?.Patient?.Full_Name ?? string.Empty;

    public string SelectedPatientPhone =>
        SelectedPatient?.Phone_number ?? string.Empty;

    public string SelectedPatientPolicy =>
        SelectedPatient?.Insurance_Policy ?? string.Empty;

    public string SelectedPatientAddress =>
        SelectedPatient?.Address ?? string.Empty;

    public string SelectedAppointmentDate =>
        SelectedAppointment == null
            ? string.Empty
            : SelectedAppointment.AppointmentDate.ToString("dd.MM.yyyy");

    public string SelectedAppointmentTime =>
        SelectedAppointment == null
            ? string.Empty
            : SelectedAppointment.AppointmentDate.ToString("HH:mm");

    public string SelectedServiceName =>
        SelectedAppointment?.MedService?.ServiceName ?? "Услуга не указана";

    public string SelectedBookingStatus =>
        SelectedAppointment?.BookingStatus ?? string.Empty;

    public string SelectedVisitDate =>
        SelectedVisitHistory == null
            ? string.Empty
            : SelectedVisitHistory.VisitDate.ToString("dd.MM.yyyy");

    public string SelectedVisitTime =>
        SelectedVisitHistory == null
            ? string.Empty
            : SelectedVisitHistory.VisitTime.ToString(@"hh\:mm");

    public string SelectedVisitResults =>
        SelectedVisitHistory?.VisitResults ?? string.Empty;

    private int? CurrentVisitHistoryId
    {
        get
        {
            if (SelectedVisitHistory != null)
                return SelectedVisitHistory.Id;

            if (_lastCreatedVisitHistoryId > 0)
                return _lastCreatedVisitHistoryId;

            return null;
        }
    }

    public ICommand LoadAppointmentsCommand { get; }

    public ICommand SaveMedCardCommand { get; }

    public ICommand AddAllergyCommand { get; }

    public ICommand AddChronicDiseaseCommand { get; }

    public ICommand CreateVisitHistoryCommand { get; }

    public ICommand AddAnalysisCommand { get; }

    public ICommand AddRecipeCommand { get; }

    public ICommand DeleteAnalysisCommand { get; }

    public ICommand DeleteRecipeCommand { get; }

    private async Task InitializeAsync()
    {
        try
        {
            await LoadReferenceDataAsync();
            await LoadAppointmentsAsync();
        }
        catch (Exception ex)
        {
            ShowError("Ошибка инициализации кабинета врача", ex);
        }
    }

    private async Task LoadReferenceDataAsync()
    {
        BloodGroups.Clear();
        AllAllergies.Clear();
        AllChronicDiseases.Clear();
        Analyses.Clear();
        Recipes.Clear();

        var bloodGroups = await _doctorHibernateService.GetBloodGroupsAsync();
        foreach (var item in bloodGroups)
            BloodGroups.Add(item);

        var allergies = await _doctorHibernateService.GetAllergiesAsync();
        foreach (var item in allergies)
            AllAllergies.Add(item);

        var chronicDiseases = await _doctorHibernateService.GetChronicDiseasesAsync();
        foreach (var item in chronicDiseases)
            AllChronicDiseases.Add(item);

        var analyses = await _doctorHibernateService.GetAnalysesAsync();
        foreach (var item in analyses)
            Analyses.Add(item);

        var recipes = await _doctorHibernateService.GetRecipesAsync();
        foreach (var item in recipes)
            Recipes.Add(item);
    }

    private async Task LoadAppointmentsAsync()
    {
        try
        {
            Appointments.Clear();

            var appointments = await _doctorHibernateService.GetUpcomingAppointmentsAsync(DoctorId);

            foreach (var appointment in appointments)
                Appointments.Add(appointment);
        }
        catch (Exception ex)
        {
            ShowError("Ошибка загрузки записей врача", ex);
        }
    }

    private async Task LoadSelectedPatientDataAsync()
    {
        if (SelectedAppointment == null)
            return;

        try
        {
            var patientId = SelectedAppointment.PatientId;

            SelectedPatient = await _doctorHibernateService.GetPatientAsync(patientId);
            SelectedMedCard = await _doctorHibernateService.GetPatientMedCardAsync(patientId);

            LoadMedCardToFields();
            await LoadPatientVisitHistoriesAsync(patientId);

            NotifySelectedPatientProperties();
            RaiseCommands();
        }
        catch (Exception ex)
        {
            ShowError("Ошибка загрузки данных пациента", ex);
        }
    }

    private void LoadMedCardToFields()
    {
        PatientAllergies.Clear();
        PatientChronicDiseases.Clear();

        if (SelectedMedCard == null)
        {
            DateOfBirth = DateTime.Today;
            Disability = false;
            SelectedBloodGroup = null;
            return;
        }

        DateOfBirth = SelectedMedCard.DateOfBirth;
        Disability = SelectedMedCard.Disability;

        SelectedBloodGroup = BloodGroups
            .FirstOrDefault(b => b.Id == SelectedMedCard.BloodGroupId);

        foreach (var allergy in SelectedMedCard.Allergies)
            PatientAllergies.Add(allergy);

        foreach (var disease in SelectedMedCard.ChronicDiseases)
            PatientChronicDiseases.Add(disease);
    }

    private async Task LoadPatientVisitHistoriesAsync(int patientId)
    {
        VisitHistories.Clear();
        AnalysisHistories.Clear();
        RecipeHistories.Clear();

        var histories = await _doctorHibernateService.GetPatientVisitHistoriesAsync(patientId);

        foreach (var history in histories)
            VisitHistories.Add(history);

        if (_lastCreatedVisitHistoryId > 0)
        {
            SelectedVisitHistory = VisitHistories.FirstOrDefault(v => v.Id == _lastCreatedVisitHistoryId);
        }
        else
        {
            SelectedVisitHistory = VisitHistories.FirstOrDefault();
        }
    }

    private void LoadAssignmentsFromSelectedVisit()
    {
        AnalysisHistories.Clear();
        RecipeHistories.Clear();

        if (SelectedVisitHistory == null)
            return;

        foreach (var analysisHistory in SelectedVisitHistory.AnalysisHistories)
            AnalysisHistories.Add(analysisHistory);

        foreach (var recipeHistory in SelectedVisitHistory.RecipeHistories)
            RecipeHistories.Add(recipeHistory);
    }

    private async Task SaveMedCardAsync()
    {
        if (SelectedAppointment == null)
            return;

        try
        {
            await _doctorHibernateService.SaveMedCardAsync(
                SelectedAppointment.PatientId,
                DateOfBirth,
                Disability,
                SelectedBloodGroup?.Id);

            MessageBox.Show(
                "Медицинская карта пациента сохранена.",
                "NHibernate",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

            await LoadSelectedPatientDataAsync();
        }
        catch (Exception ex)
        {
            ShowError("Ошибка сохранения медицинской карты", ex);
        }
    }

    private async Task AddAllergyAsync()
    {
        if (SelectedAppointment == null || SelectedAllergy == null)
            return;

        try
        {
            await _doctorHibernateService.AddAllergyToPatientAsync(
                SelectedAppointment.PatientId,
                SelectedAllergy.Id);

            SelectedAllergy = null;

            await LoadSelectedPatientDataAsync();
        }
        catch (Exception ex)
        {
            ShowError("Ошибка добавления аллергии", ex);
        }
    }

    private async Task AddChronicDiseaseAsync()
    {
        if (SelectedAppointment == null || SelectedChronicDisease == null)
            return;

        try
        {
            await _doctorHibernateService.AddChronicDiseaseToPatientAsync(
                SelectedAppointment.PatientId,
                SelectedChronicDisease.Id);

            SelectedChronicDisease = null;

            await LoadSelectedPatientDataAsync();
        }
        catch (Exception ex)
        {
            ShowError("Ошибка добавления хронического заболевания", ex);
        }
    }

    private async Task CreateVisitHistoryAsync()
    {
        if (SelectedAppointment == null)
            return;

        try
        {
            _lastCreatedVisitHistoryId = await _doctorHibernateService.CreateVisitHistoryAsync(
                SelectedAppointment.Id,
                VisitResults);

            MessageBox.Show(
                "История посещения создана.",
                "NHibernate",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

            VisitResults = string.Empty;

            await LoadAppointmentsAsync();
            await LoadSelectedPatientDataAsync();
        }
        catch (Exception ex)
        {
            ShowError("Ошибка создания истории посещения", ex);
        }
    }

    private async Task AddAnalysisAsync()
    {
        if (SelectedAnalysis == null || CurrentVisitHistoryId == null)
            return;

        try
        {
            await _doctorHibernateService.AddAnalysisToVisitAsync(
                CurrentVisitHistoryId.Value,
                SelectedAnalysis.Id,
                AnalysisResult);

            SelectedAnalysis = null;
            AnalysisResult = string.Empty;

            if (SelectedAppointment != null)
                await LoadSelectedPatientDataAsync();
        }
        catch (Exception ex)
        {
            ShowError("Ошибка назначения анализа", ex);
        }
    }

    private async Task AddRecipeAsync()
    {
        if (SelectedRecipe == null || CurrentVisitHistoryId == null)
            return;

        try
        {
            await _doctorHibernateService.AddRecipeToVisitAsync(
                CurrentVisitHistoryId.Value,
                SelectedRecipe.Id,
                RecipeDosage,
                RecipeDuration);

            SelectedRecipe = null;
            RecipeDosage = string.Empty;
            RecipeDuration = string.Empty;

            if (SelectedAppointment != null)
                await LoadSelectedPatientDataAsync();
        }
        catch (Exception ex)
        {
            ShowError("Ошибка назначения препарата", ex);
        }
    }

    private async Task DeleteAnalysisAsync()
    {
        if (SelectedAnalysisHistory == null)
            return;

        try
        {
            await _doctorHibernateService.DeleteAnalysisHistoryAsync(SelectedAnalysisHistory.Id);

            SelectedAnalysisHistory = null;

            if (SelectedAppointment != null)
                await LoadSelectedPatientDataAsync();
        }
        catch (Exception ex)
        {
            ShowError("Ошибка удаления назначенного анализа", ex);
        }
    }

    private async Task DeleteRecipeAsync()
    {
        if (SelectedRecipeHistory == null)
            return;

        try
        {
            await _doctorHibernateService.DeleteRecipeHistoryAsync(SelectedRecipeHistory.Id);

            SelectedRecipeHistory = null;

            if (SelectedAppointment != null)
                await LoadSelectedPatientDataAsync();
        }
        catch (Exception ex)
        {
            ShowError("Ошибка удаления назначенного препарата", ex);
        }
    }

    private void ClearPatientData()
    {
        SelectedPatient = null;
        SelectedMedCard = null;
        SelectedVisitHistory = null;

        DateOfBirth = DateTime.Today;
        Disability = false;
        SelectedBloodGroup = null;

        VisitResults = string.Empty;
        AnalysisResult = string.Empty;
        RecipeDosage = string.Empty;
        RecipeDuration = string.Empty;

        PatientAllergies.Clear();
        PatientChronicDiseases.Clear();
        VisitHistories.Clear();
        AnalysisHistories.Clear();
        RecipeHistories.Clear();

        NotifySelectedPatientProperties();
    }

    private void NotifySelectedAppointmentProperties()
    {
        OnPropertyChanged(nameof(SelectedPatientName));
        OnPropertyChanged(nameof(SelectedAppointmentDate));
        OnPropertyChanged(nameof(SelectedAppointmentTime));
        OnPropertyChanged(nameof(SelectedServiceName));
        OnPropertyChanged(nameof(SelectedBookingStatus));
    }

    private void NotifySelectedPatientProperties()
    {
        OnPropertyChanged(nameof(SelectedPatientName));
        OnPropertyChanged(nameof(SelectedPatientPhone));
        OnPropertyChanged(nameof(SelectedPatientPolicy));
        OnPropertyChanged(nameof(SelectedPatientAddress));
    }

    private void RaiseCommands()
    {
        CommandManager.InvalidateRequerySuggested();
    }

    private static void ShowError(string title, Exception ex)
    {
        MessageBox.Show(
            $"{title}:\n{ex.Message}",
            "NHibernate",
            MessageBoxButton.OK,
            MessageBoxImage.Error);
    }
}