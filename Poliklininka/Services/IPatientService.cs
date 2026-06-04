using Poliklininka.Entities;

namespace Poliklininka.Services;

public interface IPatientService
{
    Task<Patient> GetPatientByUserIdAsync(int userId);
    Task UpdatePatientByUserIdAsync(Patient patient);
    Task<Patient?> GetMedCardByUserIdAsync(int userId);
    Task <List<Appointment>> GetAppointmentsByUserIdAsync(int userId);
    Task  CreateAppointmentByUserIdAsync(Appointment appointment, Schedule schedule);
    Task<List<Schedule>> GetDoctorScheduleByDoctorIdAsync(int doctorId);
    Task<List<VisitHistory>> GetVisitHistoryByUserIdAsync(int userId);

    Task<List<Doctor>> GetDoctorBySpecializationAsync(string specialization);


}
