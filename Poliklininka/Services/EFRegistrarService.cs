using Microsoft.EntityFrameworkCore;
using Poliklininka.Entities;
using Poliklininka.Infrastructure.EF;

namespace Poliklininka.Services;

public class EFRegistrarService : IRegistrarService
{
    private readonly ApplicationDbContext _context;

    public EFRegistrarService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Patient>> GetPatientsAsync(string? searchText)
    {
        var query = _context.Patients.AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchText))
        {
            var text = searchText.ToLower();

            query = query.Where(p =>
                p.Full_Name.ToLower().Contains(text) ||
                p.Insurance_Policy.ToLower().Contains(text));
        }

        return await query
            .OrderBy(p => p.Full_Name)
            .ToListAsync();
    }

    public async Task<List<Doctor>> GetDoctorsAsync()
    {
        return await _context.Doctors
            .OrderBy(d => d.Full_Name)
            .ToListAsync();
    }

    public async Task<List<Schedule>> GetDoctorSchedulesAsync(int doctorId)
    {
        return await _context.Schedules
            .Where(s => s.DoctorId == doctorId)
            .OrderBy(s => s.Date)
            .ThenBy(s => s.StartTime)
            .ToListAsync();
    }

    public async Task<List<Schedule>> GetFreeDoctorSchedulesAsync(int doctorId)
    {
        return await _context.Schedules
            .Where(s => s.DoctorId == doctorId)
            .Where(s => s.SlotStatus == "free")
            .OrderBy(s => s.Date)
            .ThenBy(s => s.StartTime)
            .ToListAsync();
    }

    public async Task<List<MedService>> GetServicesForDoctorAsync(int doctorId)
    {
        var doctor = await _context.Doctors
            .FirstOrDefaultAsync(d => d.Id == doctorId);

        if (doctor == null)
            return new List<MedService>();

        return await _context.MedServices
            .Where(s => s.Category == doctor.Specialization)
            .OrderBy(s => s.ServiceName)
            .ToListAsync();
    }

    public async Task<List<Appointment>> GetPatientAppointmentsAsync(int patientId)
    {
        return await _context.Appointments
            .Include(a => a.Doctor)
            .Include(a => a.Schedule)
            .Include(a => a.MedService)
            .Where(a => a.PatientId == patientId)
            .OrderByDescending(a => a.AppointmentDate)
            .ToListAsync();
    }

    public async Task<Patient> CreatePatientAsync(
        string fullName,
        string login,
        string password,
        string phoneNumber,
        string insurancePolicy,
        string address)
    {
        var patient = new Patient
        {
            Full_Name = fullName,
            Login = login,
            Password = password,
            Role = "Patient",
            Discriminator = "Patient",
            Phone_number = phoneNumber,
            Insurance_Policy = insurancePolicy,
            Address = address
        };

        _context.Patients.Add(patient);

        await _context.SaveChangesAsync();

        return patient;
    }

    public async Task UpdatePatientAsync(
        int patientId,
        string fullName,
        string phoneNumber,
        string insurancePolicy,
        string address)
    {
        var patient = await _context.Patients
            .FirstOrDefaultAsync(p => p.Id == patientId);

        if (patient == null)
            throw new InvalidOperationException("Пациент не найден.");

        patient.Full_Name = fullName;
        patient.Phone_number = phoneNumber;
        patient.Insurance_Policy = insurancePolicy;
        patient.Address = address;

        await _context.SaveChangesAsync();
    }

    public async Task<Schedule> CreateScheduleAsync(
        int doctorId,
        DateOnly date,
        TimeOnly startTime,
        TimeOnly endTime,
        string office)
    {
        var doctor = await _context.Doctors
            .FirstOrDefaultAsync(d => d.Id == doctorId);

        if (doctor == null)
            throw new InvalidOperationException("Врач не найден.");

        var schedule = new Schedule
        {
            DoctorId = doctorId,
            Date = date,
            StartTime = startTime,
            EndTime = endTime,
            Office = office,
            SlotStatus = "free"
        };

        _context.Schedules.Add(schedule);

        await _context.SaveChangesAsync();

        return schedule;
    }

    public async Task<Appointment> CreateAppointmentAsync(
     int patientId,
     int doctorId,
     int scheduleId,
     int medServiceId)
    {
        var patient = await _context.Patients
            .FirstOrDefaultAsync(p => p.Id == patientId);

        if (patient == null)
            throw new InvalidOperationException("Пациент не найден.");

        var doctor = await _context.Doctors
            .FirstOrDefaultAsync(d => d.Id == doctorId);

        if (doctor == null)
            throw new InvalidOperationException("Врач не найден.");

        var schedule = await _context.Schedules
            .FirstOrDefaultAsync(s => s.Id == scheduleId);

        if (schedule == null)
            throw new InvalidOperationException("Расписание не найдено.");

        if (schedule.SlotStatus != "free")
            throw new InvalidOperationException("Этот слот уже занят.");

        var medService = await _context.MedServices
            .FirstOrDefaultAsync(s => s.Id == medServiceId);

        if (medService == null)
            throw new InvalidOperationException("Услуга не найдена.");

        var appointmentDate = schedule.Date.ToDateTime(schedule.StartTime);

        appointmentDate = DateTime.SpecifyKind(
            appointmentDate,
            DateTimeKind.Utc);

        var appointment = new Appointment
        {
            PatientId = patientId,
            DoctorId = doctorId,
            ScheduleId = scheduleId,
            MedServiceId = medServiceId,
            AppointmentDate = appointmentDate,
            BookingStatus = "active",
            CreatedAt = DateTime.UtcNow
        };

        schedule.SlotStatus = "busy";

        _context.Appointments.Add(appointment);

        await _context.SaveChangesAsync();

        return appointment;
    }

    public async Task CancelAppointmentAsync(int appointmentId)
    {
        var appointment = await _context.Appointments
            .Include(a => a.Schedule)
            .FirstOrDefaultAsync(a => a.Id == appointmentId);

        if (appointment == null)
            throw new InvalidOperationException("Запись не найдена.");

        appointment.BookingStatus = "cancelled";

        if (appointment.Schedule != null)
        {
            appointment.Schedule.SlotStatus = "free";
        }

        await _context.SaveChangesAsync();
    }
}