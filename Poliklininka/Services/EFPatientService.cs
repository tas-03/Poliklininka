using Microsoft.EntityFrameworkCore;
using Poliklininka.Entities;
using Poliklininka.Infrastructure.EF;

namespace Poliklininka.Services;

public class EFPatientService : IPatientService
{
    private readonly ApplicationDbContext _context;
    public EFPatientService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task CreateAppointmentByUserIdAsync(Appointment appointment, Schedule schedule)
    {
        var existSchedule = await _context.Schedules
            .FirstOrDefaultAsync(e => e.Id == schedule.Id);

        if (existSchedule == null)
        {
            throw new Exception("Ошибка создания записи на прием! Расписание не найдено!");
        }

        if (existSchedule.SlotStatus != "free")
        {
            throw new Exception("Выбранное время уже занято!");
        }

        existSchedule.SlotStatus = "booked";

        appointment.ScheduleId = existSchedule.Id;
        appointment.DoctorId = existSchedule.DoctorId;
        appointment.AppointmentDate = existSchedule.Date.ToDateTime(existSchedule.StartTime);
        appointment.BookingStatus = "active";
        appointment.CreatedAt = DateTime.Now;

        await _context.Appointments.AddAsync(appointment);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Appointment>> GetAppointmentsByUserIdAsync(int userId)
    {
        var appointments = await _context.Appointments
            .Include(a => a.Doctor)
            .Include(a => a.Schedule)
            .Include(a => a.MedService)
            .Where(a => a.PatientId == userId)
            .OrderBy(a => a.AppointmentDate)
            .ToListAsync();

        return appointments;
    }

    public async Task<List<Doctor>> GetDoctorBySpecializationAsync(string specialization)
    {
        var doctors = await _context.Doctors
            .Where(e => e.Specialization == specialization)
            .OrderBy(e => e.Full_Name)
            .ToListAsync();

        return doctors;
    }

    public async Task<List<Schedule>> GetDoctorScheduleByDoctorIdAsync(int doctorId)
    {
        var doctorSchedule = await _context.Schedules
            .Where(e => e.DoctorId == doctorId)
            .OrderBy(e => e.Date)
            .ThenBy(e => e.StartTime)
            .ToListAsync();

        return doctorSchedule;
    }

    public async Task<Patient?> GetMedCardByUserIdAsync(int userId)
    {
        var patient = await _context.Patients.Include(p => p.MedCard).ThenInclude(e=>e.BloodGroup).Include(p => p.MedCard).ThenInclude(e => e.AllergyPatient).ThenInclude(e => e.Allergy).Include(p => p.MedCard).ThenInclude(e => e.HronicDiseasesPatient).ThenInclude(e => e.ChronicDiseases).FirstOrDefaultAsync(p=>p.Id == userId);
        if (patient == null) 
            throw new Exception("Ошибка загрузки медкарты!");
        return patient;
    }

    public async Task<Patient> GetPatientByUserIdAsync(int userId)
    {
        var patient =await _context.Patients.FirstOrDefaultAsync(e => e.Id == userId);
        if (patient != null) return patient;
        else
        {
            throw new Exception("Невозможно загрузить профиль! Пациента с таким ID нет!");
        }
    }

    public async Task<List<VisitHistory>> GetVisitHistoryByUserIdAsync(int userId)
    {
        var history = await _context.VisitHistories.Where(e=>e.PatientId == userId).Include(m=>m.MedService).Include(a=>a.Appointment).ThenInclude(d=>d.Doctor)
            .Include(a => a.AnalysisHistories).ThenInclude(a=>a.Analysis).Include(r=>r.RecipeHistories).ThenInclude(r=>r.Recipe).ToListAsync();
            return history;
    }

    public async Task UpdatePatientByUserIdAsync(Patient patient)
    {
        var exist_patient= await _context.Patients.FirstOrDefaultAsync(e => e.Id == patient.Id);
        if (exist_patient != null)
        {
            exist_patient.Address = patient.Address ?? exist_patient.Address;
            exist_patient.Phone_number = patient.Phone_number ?? exist_patient.Phone_number;
            exist_patient.Photo = patient.Photo ?? exist_patient.Photo;
        }
        else
        {
            throw new Exception("Невозможно отредактировать анкету! Пациента с таким ID нет!");
        }
        
        await _context.SaveChangesAsync();

    }

    public async Task DeleteAppointmentByIdAsync(int appointmentId)
    {
        var appointment = await _context.Appointments
            .Include(a => a.VisitHistory)
            .FirstOrDefaultAsync(a => a.Id == appointmentId);

        if (appointment == null)
        {
            throw new Exception("Запись на прием не найдена!");
        }

        if (appointment.VisitHistory != null)
        {
            throw new Exception("Нельзя отменить запись, по которой уже есть история посещения!");
        }

        var schedule = await _context.Schedules
            .FirstOrDefaultAsync(s => s.Id == appointment.ScheduleId);

        if (schedule != null)
        {
            schedule.SlotStatus = "free";
        }

        _context.Appointments.Remove(appointment);
        await _context.SaveChangesAsync();
    }

    public async Task<List<MedService>> GetMedServicesAsync()
    {
        var services = await _context.MedServices
            .OrderBy(s => s.ServiceName)
            .ToListAsync();

        return services;
    }
    public async Task<List<Schedule>> GetFreeDoctorScheduleByDoctorIdAsync(int doctorId)
    {
        var schedules = await _context.Schedules
            .Where(s => s.DoctorId == doctorId && s.SlotStatus == "free")
            .OrderBy(s => s.Date)
            .ThenBy(s => s.StartTime)
            .ToListAsync();

        return schedules;
    }
    public async Task UpdateAppointmentAsync(Appointment appointment, Schedule oldSchedule, Schedule newSchedule)
    {
        var existAppointment = await _context.Appointments
            .Include(a => a.VisitHistory)
            .FirstOrDefaultAsync(a => a.Id == appointment.Id);

        if (existAppointment == null)
        {
            throw new Exception("Запись на прием не найдена!");
        }

        if (existAppointment.VisitHistory != null)
        {
            throw new Exception("Нельзя редактировать запись, по которой уже есть история посещения!");
        }

        var existOldSchedule = await _context.Schedules
            .FirstOrDefaultAsync(s => s.Id == oldSchedule.Id);

        if (existOldSchedule != null)
        {
            existOldSchedule.SlotStatus = "free";
        }

        var existNewSchedule = await _context.Schedules
            .FirstOrDefaultAsync(s => s.Id == newSchedule.Id);

        if (existNewSchedule == null)
        {
            throw new Exception("Выбранное время не найдено!");
        }

        if (existNewSchedule.SlotStatus != "free" && existNewSchedule.Id != oldSchedule.Id)
        {
            throw new Exception("Выбранное время уже занято!");
        }

        existNewSchedule.SlotStatus = "booked";

        existAppointment.DoctorId = existNewSchedule.DoctorId;
        existAppointment.ScheduleId = existNewSchedule.Id;
        existAppointment.AppointmentDate = existNewSchedule.Date.ToDateTime(existNewSchedule.StartTime);
        existAppointment.BookingStatus = appointment.BookingStatus;
        existAppointment.MedServiceId = appointment.MedServiceId;

        await _context.SaveChangesAsync();
    }
}
