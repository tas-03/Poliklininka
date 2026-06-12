using Poliklininka.Core;
using Poliklininka.Entities;
using Poliklininka.Services;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Poliklininka.ViewModels.Patient_Model;

public class AppointmentWindowViewModel : BaseViewModel
{
    private readonly Patient _patient;
    private readonly IPatientService _patientService;
    private readonly Appointment? _appointment;
    private readonly Schedule? _oldSchedule;

    public event Action? OnSaveSuccess;
    public event Action? RequestClose;

    public bool IsEditMode => _appointment != null;

    private string _windowTitle = string.Empty;
    public string WindowTitle
    {
        get => _windowTitle;
        set => SetProperty(ref _windowTitle, value);
    }

    private ObservableCollection<MedService> _medServices = new();
    public ObservableCollection<MedService> MedServices
    {
        get => _medServices;
        set => SetProperty(ref _medServices, value);
    }

    private MedService? _selectedMedService;
    public MedService? SelectedMedService
    {
        get => _selectedMedService;
        set
        {
            if (SetProperty(ref _selectedMedService, value))
            {
                _ = LoadDoctorsAsync();
            }
        }
    }

    private ObservableCollection<Doctor> _doctors = new();
    public ObservableCollection<Doctor> Doctors
    {
        get => _doctors;
        set => SetProperty(ref _doctors, value);
    }

    private Doctor? _selectedDoctor;
    public Doctor? SelectedDoctor
    {
        get => _selectedDoctor;
        set
        {
            if (SetProperty(ref _selectedDoctor, value))
            {
                _ = LoadSchedulesAsync();
            }
        }
    }

    private ObservableCollection<Schedule> _schedules = new();
    public ObservableCollection<Schedule> Schedules
    {
        get => _schedules;
        set => SetProperty(ref _schedules, value);
    }

    private Schedule? _selectedSchedule;
    public Schedule? SelectedSchedule
    {
        get => _selectedSchedule;
        set => SetProperty(ref _selectedSchedule, value);
    }

    public ICommand SaveCommand { get; }
    public ICommand CloseCommand { get; }

    public AppointmentWindowViewModel(
        Patient patient,
        IPatientService patientService,
        Appointment? appointment = null)
    {
        _patient = patient;
        _patientService = patientService;
        _appointment = appointment;
        _oldSchedule = appointment?.Schedule;

        WindowTitle = IsEditMode ? "Редактирование записи" : "Запись на прием";

        SaveCommand = new RelayCommand(_ => _ = SaveAsync());
        CloseCommand = new RelayCommand(_ => RequestClose?.Invoke());

        _ = LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        try
        {
            var services = await _patientService.GetMedServicesAsync();
            MedServices = new ObservableCollection<MedService>(services);

            if (IsEditMode && _appointment?.MedServiceId != null)
            {
                SelectedMedService = MedServices
                    .FirstOrDefault(s => s.Id == _appointment.MedServiceId);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Ошибка загрузки услуг", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private async Task LoadDoctorsAsync()
    {
        if (SelectedMedService == null)
        {
            Doctors.Clear();
            Schedules.Clear();
            return;
        }

        try
        {
            var doctors = await _patientService
                .GetDoctorBySpecializationAsync(SelectedMedService.Category);

            Doctors = new ObservableCollection<Doctor>(doctors);

            if (IsEditMode && _appointment != null)
            {
                SelectedDoctor = Doctors
                    .FirstOrDefault(d => d.Id == _appointment.DoctorId);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Ошибка загрузки врачей", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private async Task LoadSchedulesAsync()
    {
        if (SelectedDoctor == null)
        {
            Schedules.Clear();
            return;
        }

        try
        {
            var schedules = await _patientService
                .GetDoctorScheduleByDoctorIdAsync(SelectedDoctor.Id);

            var availableSchedules = schedules
                .Where(s =>
                    s.SlotStatus == "free" ||
                    (IsEditMode && _appointment != null && s.Id == _appointment.ScheduleId))
                .OrderBy(s => s.Date)
                .ThenBy(s => s.StartTime)
                .ToList();

            Schedules = new ObservableCollection<Schedule>(availableSchedules);

            if (IsEditMode && _appointment != null)
            {
                SelectedSchedule = Schedules
                    .FirstOrDefault(s => s.Id == _appointment.ScheduleId);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Ошибка загрузки расписания", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private async Task SaveAsync()
    {
        if (SelectedMedService == null)
        {
            MessageBox.Show("Выберите процедуру.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (SelectedDoctor == null)
        {
            MessageBox.Show("Выберите врача.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (SelectedSchedule == null)
        {
            MessageBox.Show("Выберите время приема.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        try
        {
            if (IsEditMode && _appointment != null && _oldSchedule != null)
            {
                var updatedAppointment = new Appointment
                {
                    Id = _appointment.Id,
                    PatientId = _patient.Id,
                    DoctorId = SelectedDoctor.Id,
                    ScheduleId = SelectedSchedule.Id,
                    MedServiceId = SelectedMedService.Id,
                    AppointmentDate = SelectedSchedule.Date.ToDateTime(SelectedSchedule.StartTime),
                    BookingStatus = "active"
                };

                await _patientService.UpdateAppointmentAsync(
                    updatedAppointment,
                    _oldSchedule,
                    SelectedSchedule);

                MessageBox.Show(
                    "Запись успешно изменена.",
                    "Готово",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            else
            {
                var newAppointment = new Appointment
                {
                    PatientId = _patient.Id,
                    DoctorId = SelectedDoctor.Id,
                    ScheduleId = SelectedSchedule.Id,
                    MedServiceId = SelectedMedService.Id,
                    AppointmentDate = SelectedSchedule.Date.ToDateTime(SelectedSchedule.StartTime),
                    BookingStatus = "active",
                    CreatedAt = DateTime.Now
                };

                await _patientService.CreateAppointmentByUserIdAsync(
                    newAppointment,
                    SelectedSchedule);

                MessageBox.Show(
                    "Вы успешно записались на прием.",
                    "Готово",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }

            OnSaveSuccess?.Invoke();
            RequestClose?.Invoke();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Ошибка сохранения", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}