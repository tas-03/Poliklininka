using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Input;
using Poliklininka.Core;
using Poliklininka.Entities;
using Poliklininka.Services;

namespace Poliklininka.ViewModels.Registrar_Model;

public class RegistrarViewModel : BaseViewModel
{
    private readonly IRegistrarService _registrarService;
    private readonly User _currentUser;

    public RegistrarViewModel(User currentUser, IRegistrarService registrarService)
    {
        _currentUser = currentUser;
        _registrarService = registrarService;

        RegistrarName = currentUser.Full_Name;

        Patients = new ObservableCollection<Patient>();
        Doctors = new ObservableCollection<Doctor>();
        DoctorSchedules = new ObservableCollection<Schedule>();
        FreeSchedules = new ObservableCollection<Schedule>();
        MedServices = new ObservableCollection<MedService>();
        PatientAppointments = new ObservableCollection<Appointment>();

        ScheduleDate = DateTime.Today;
        ScheduleStartTime = "08:00";
        ScheduleEndTime = "08:30";

        RefreshCommand = new RelayCommand(async _ => await InitializeAsync());
        SearchPatientsCommand = new RelayCommand(async _ => await LoadPatientsAsync());
        NewPatientCommand = new RelayCommand(_ => ClearPatientFields());

        CreatePatientCommand = new RelayCommand(
            async _ => await CreatePatientAsync(),
            _ => CanCreatePatient());

        UpdatePatientCommand = new RelayCommand(
            async _ => await UpdatePatientAsync(),
            _ => SelectedPatient != null);

        CreateScheduleCommand = new RelayCommand(
            async _ => await CreateScheduleAsync(),
            _ => SelectedDoctor != null);

        CreateAppointmentCommand = new RelayCommand(
            async _ => await CreateAppointmentAsync(),
            _ => SelectedPatient != null
                 && SelectedDoctor != null
                 && SelectedFreeSchedule != null
                 && SelectedMedService != null);

        CancelAppointmentCommand = new RelayCommand(
            async _ => await CancelAppointmentAsync(),
            _ => SelectedAppointment != null);

        _ = InitializeAsync();
    }

    private string _registrarName = string.Empty;
    public string RegistrarName
    {
        get => _registrarName;
        set => SetProperty(ref _registrarName, value);
    }

    public ObservableCollection<Patient> Patients { get; }
    public ObservableCollection<Doctor> Doctors { get; }
    public ObservableCollection<Schedule> DoctorSchedules { get; }
    public ObservableCollection<Schedule> FreeSchedules { get; }
    public ObservableCollection<MedService> MedServices { get; }
    public ObservableCollection<Appointment> PatientAppointments { get; }

    private string _searchText = string.Empty;
    public string SearchText
    {
        get => _searchText;
        set => SetProperty(ref _searchText, value);
    }

    private Patient? _selectedPatient;
    public Patient? SelectedPatient
    {
        get => _selectedPatient;
        set
        {
            if (SetProperty(ref _selectedPatient, value))
            {
                FillPatientFields();

                if (_selectedPatient != null)
                    _ = LoadPatientAppointmentsAsync(_selectedPatient.Id);
                else
                    PatientAppointments.Clear();

                RaiseCommands();
            }
        }
    }

    private Doctor? _selectedDoctor;
    public Doctor? SelectedDoctor
    {
        get => _selectedDoctor;
        set
        {
            if (SetProperty(ref _selectedDoctor, value))
            {
                if (_selectedDoctor != null)
                {
                    ScheduleOffice = _selectedDoctor.Office;
                    _ = LoadDoctorDataAsync(_selectedDoctor.Id);
                }
                else
                {
                    DoctorSchedules.Clear();
                    FreeSchedules.Clear();
                    MedServices.Clear();
                }

                RaiseCommands();
            }
        }
    }

    private Schedule? _selectedFreeSchedule;
    public Schedule? SelectedFreeSchedule
    {
        get => _selectedFreeSchedule;
        set
        {
            if (SetProperty(ref _selectedFreeSchedule, value))
                RaiseCommands();
        }
    }

    private MedService? _selectedMedService;
    public MedService? SelectedMedService
    {
        get => _selectedMedService;
        set
        {
            if (SetProperty(ref _selectedMedService, value))
                RaiseCommands();
        }
    }

    private Appointment? _selectedAppointment;
    public Appointment? SelectedAppointment
    {
        get => _selectedAppointment;
        set
        {
            if (SetProperty(ref _selectedAppointment, value))
                RaiseCommands();
        }
    }

    private string _patientFullName = string.Empty;
    public string PatientFullName
    {
        get => _patientFullName;
        set
        {
            if (SetProperty(ref _patientFullName, value))
                RaiseCommands();
        }
    }

    private string _patientLogin = string.Empty;
    public string PatientLogin
    {
        get => _patientLogin;
        set
        {
            if (SetProperty(ref _patientLogin, value))
                RaiseCommands();
        }
    }

    private string _patientPassword = string.Empty;
    public string PatientPassword
    {
        get => _patientPassword;
        set
        {
            if (SetProperty(ref _patientPassword, value))
                RaiseCommands();
        }
    }

    private string _patientPhone = string.Empty;
    public string PatientPhone
    {
        get => _patientPhone;
        set => SetProperty(ref _patientPhone, value);
    }

    private string _patientInsurancePolicy = string.Empty;
    public string PatientInsurancePolicy
    {
        get => _patientInsurancePolicy;
        set
        {
            if (SetProperty(ref _patientInsurancePolicy, value))
                RaiseCommands();
        }
    }

    private string _patientAddress = string.Empty;
    public string PatientAddress
    {
        get => _patientAddress;
        set => SetProperty(ref _patientAddress, value);
    }

    private DateTime _scheduleDate;
    public DateTime ScheduleDate
    {
        get => _scheduleDate;
        set => SetProperty(ref _scheduleDate, value);
    }

    private string _scheduleStartTime = string.Empty;
    public string ScheduleStartTime
    {
        get => _scheduleStartTime;
        set => SetProperty(ref _scheduleStartTime, value);
    }

    private string _scheduleEndTime = string.Empty;
    public string ScheduleEndTime
    {
        get => _scheduleEndTime;
        set => SetProperty(ref _scheduleEndTime, value);
    }

    private string _scheduleOffice = string.Empty;
    public string ScheduleOffice
    {
        get => _scheduleOffice;
        set => SetProperty(ref _scheduleOffice, value);
    }

    public ICommand RefreshCommand { get; }
    public ICommand SearchPatientsCommand { get; }
    public ICommand NewPatientCommand { get; }
    public ICommand CreatePatientCommand { get; }
    public ICommand UpdatePatientCommand { get; }
    public ICommand CreateScheduleCommand { get; }
    public ICommand CreateAppointmentCommand { get; }
    public ICommand CancelAppointmentCommand { get; }

    private async Task InitializeAsync()
    {
        try
        {
            await LoadPatientsAsync();
            await LoadDoctorsAsync();
        }
        catch (Exception ex)
        {
            ShowError("Ошибка загрузки данных регистратора", ex);
        }
    }

    private async Task LoadPatientsAsync()
    {
        try
        {
            Patients.Clear();

            var patients = await _registrarService.GetPatientsAsync(SearchText);

            foreach (var patient in patients)
                Patients.Add(patient);
        }
        catch (Exception ex)
        {
            ShowError("Ошибка загрузки пациентов", ex);
        }
    }

    private async Task LoadDoctorsAsync()
    {
        Doctors.Clear();

        var doctors = await _registrarService.GetDoctorsAsync();

        foreach (var doctor in doctors)
            Doctors.Add(doctor);
    }

    private async Task LoadDoctorDataAsync(int doctorId)
    {
        DoctorSchedules.Clear();
        FreeSchedules.Clear();
        MedServices.Clear();

        var schedules = await _registrarService.GetDoctorSchedulesAsync(doctorId);
        foreach (var schedule in schedules)
            DoctorSchedules.Add(schedule);

        var freeSchedules = await _registrarService.GetFreeDoctorSchedulesAsync(doctorId);
        foreach (var schedule in freeSchedules)
            FreeSchedules.Add(schedule);

        var services = await _registrarService.GetServicesForDoctorAsync(doctorId);
        foreach (var service in services)
            MedServices.Add(service);
    }

    private async Task LoadPatientAppointmentsAsync(int patientId)
    {
        PatientAppointments.Clear();

        var appointments = await _registrarService.GetPatientAppointmentsAsync(patientId);

        foreach (var appointment in appointments)
            PatientAppointments.Add(appointment);
    }

    private void FillPatientFields()
    {
        if (SelectedPatient == null)
        {
            ClearPatientFields();
            return;
        }

        PatientFullName = SelectedPatient.Full_Name;
        PatientLogin = SelectedPatient.Login;
        PatientPassword = SelectedPatient.Password;
        PatientPhone = SelectedPatient.Phone_number ?? string.Empty;
        PatientInsurancePolicy = SelectedPatient.Insurance_Policy;
        PatientAddress = SelectedPatient.Address ?? string.Empty;
    }

    private void ClearPatientFields()
    {
        SelectedPatient = null;

        PatientFullName = string.Empty;
        PatientLogin = string.Empty;
        PatientPassword = string.Empty;
        PatientPhone = string.Empty;
        PatientInsurancePolicy = string.Empty;
        PatientAddress = string.Empty;

        PatientAppointments.Clear();

        RaiseCommands();
    }

    private bool CanCreatePatient()
    {
        return !string.IsNullOrWhiteSpace(PatientFullName)
               && !string.IsNullOrWhiteSpace(PatientLogin)
               && !string.IsNullOrWhiteSpace(PatientPassword)
               && !string.IsNullOrWhiteSpace(PatientInsurancePolicy);
    }

    private async Task CreatePatientAsync()
    {
        try
        {
            var patient = await _registrarService.CreatePatientAsync(
                PatientFullName,
                PatientLogin,
                PatientPassword,
                PatientPhone,
                PatientInsurancePolicy,
                PatientAddress);

            MessageBox.Show(
                "Пациент зарегистрирован.",
                "Регистратор",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

            await LoadPatientsAsync();

            SelectedPatient = Patients.FirstOrDefault(p => p.Id == patient.Id);
        }
        catch (Exception ex)
        {
            ShowError("Ошибка регистрации пациента", ex);
        }
    }

    private async Task UpdatePatientAsync()
    {
        if (SelectedPatient == null)
            return;

        try
        {
            await _registrarService.UpdatePatientAsync(
                SelectedPatient.Id,
                PatientFullName,
                PatientPhone,
                PatientInsurancePolicy,
                PatientAddress);

            MessageBox.Show(
                "Данные пациента сохранены.",
                "Регистратор",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

            await LoadPatientsAsync();

            SelectedPatient = Patients.FirstOrDefault(p => p.Id == SelectedPatient.Id);
        }
        catch (Exception ex)
        {
            ShowError("Ошибка сохранения пациента", ex);
        }
    }

    private async Task CreateScheduleAsync()
    {
        if (SelectedDoctor == null)
            return;

        try
        {
            if (!TryParseTime(ScheduleStartTime, out var startTime))
            {
                MessageBox.Show("Введите время начала в формате HH:mm.");
                return;
            }

            if (!TryParseTime(ScheduleEndTime, out var endTime))
            {
                MessageBox.Show("Введите время окончания в формате HH:mm.");
                return;
            }

            if (endTime <= startTime)
            {
                MessageBox.Show("Время окончания должно быть позже времени начала.");
                return;
            }

            await _registrarService.CreateScheduleAsync(
                SelectedDoctor.Id,
                DateOnly.FromDateTime(ScheduleDate),
                startTime,
                endTime,
                ScheduleOffice);

            MessageBox.Show(
                "Расписание врача добавлено.",
                "Регистратор",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

            await LoadDoctorDataAsync(SelectedDoctor.Id);
        }
        catch (Exception ex)
        {
            ShowError("Ошибка создания расписания", ex);
        }
    }

    private async Task CreateAppointmentAsync()
    {
        if (SelectedPatient == null ||
            SelectedDoctor == null ||
            SelectedFreeSchedule == null ||
            SelectedMedService == null)
        {
            return;
        }

        try
        {
            await _registrarService.CreateAppointmentAsync(
                SelectedPatient.Id,
                SelectedDoctor.Id,
                SelectedFreeSchedule.Id,
                SelectedMedService.Id);

            MessageBox.Show(
                "Пациент записан на прием.",
                "Регистратор",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

            await LoadDoctorDataAsync(SelectedDoctor.Id);
            await LoadPatientAppointmentsAsync(SelectedPatient.Id);
        }
        catch (Exception ex)
        {
            ShowError("Ошибка записи на прием", ex);
        }
    }

    private async Task CancelAppointmentAsync()
    {
        if (SelectedAppointment == null)
            return;

        try
        {
            await _registrarService.CancelAppointmentAsync(SelectedAppointment.Id);

            MessageBox.Show(
                "Запись отменена.",
                "Регистратор",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

            if (SelectedDoctor != null)
                await LoadDoctorDataAsync(SelectedDoctor.Id);

            if (SelectedPatient != null)
                await LoadPatientAppointmentsAsync(SelectedPatient.Id);
        }
        catch (Exception ex)
        {
            ShowError("Ошибка отмены записи", ex);
        }
    }

    private static bool TryParseTime(string value, out TimeOnly time)
    {
        return TimeOnly.TryParseExact(
            value,
            "HH:mm",
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out time);
    }

    private void RaiseCommands()
    {
        CommandManager.InvalidateRequerySuggested();
    }

    private static void ShowError(string title, Exception ex)
    {
        MessageBox.Show(
            $"{title}:\n{GetFullExceptionMessage(ex)}",
            "Регистратор",
            MessageBoxButton.OK,
            MessageBoxImage.Error);
    }

    private static string GetFullExceptionMessage(Exception ex)
    {
        var message = ex.Message;

        var inner = ex.InnerException;

        while (inner != null)
        {
            message += $"\n\nВнутренняя ошибка:\n{inner.Message}";
            inner = inner.InnerException;
        }

        return message;
    }
}