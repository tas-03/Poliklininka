using Poliklininka.Entities;

namespace Poliklininka.Services;

public interface IRegistrarService
{
    Task<List<Patient>> GetPatientsAsync(string? searchText);

    Task<List<Doctor>> GetDoctorsAsync();

    Task<List<Schedule>> GetDoctorSchedulesAsync(int doctorId);

    Task<List<Schedule>> GetFreeDoctorSchedulesAsync(int doctorId);

    Task<List<MedService>> GetServicesForDoctorAsync(int doctorId);

    Task<List<Appointment>> GetPatientAppointmentsAsync(int patientId);

    Task<Patient> CreatePatientAsync(
        string fullName,
        string login,
        string password,
        string phoneNumber,
        string insurancePolicy,
        string address);

    Task UpdatePatientAsync(
        int patientId,
        string fullName,
        string phoneNumber,
        string insurancePolicy,
        string address);

    Task<Schedule> CreateScheduleAsync(
        int doctorId,
        DateOnly date,
        TimeOnly startTime,
        TimeOnly endTime,
        string office);

    Task<Appointment> CreateAppointmentAsync(
        int patientId,
        int doctorId,
        int scheduleId,
        int medServiceId);

    Task CancelAppointmentAsync(int appointmentId);
}