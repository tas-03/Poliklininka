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
        var exist_schedule = await _context.Schedules.FirstOrDefaultAsync(e => e.Id == schedule.Id);
        if (exist_schedule == null)
        {
            throw new Exception("Ошибка создания записи на прием! Расписание не найдено!");
        }
        exist_schedule.SlotStatus = "booked";
        await _context.Appointments.AddAsync(appointment);
        await _context.SaveChangesAsync();
        
    }

    public Task<List<Appointment>> GetAppointmentsByUserIdAsync(int userId)
    {
        var Appointments = _context.Appointments.Where(e => e.PatientId == userId).ToListAsync();
        if (Appointments == null)
        {
            throw new Exception("Ошибка загрузки записей на прием!");
        }
        return Appointments;
    }

    public Task<List<Doctor>> GetDoctorBySpecializationAsync(string specialization)
    {
       var doctors = _context.Doctors.Where(e => e.Specialization == specialization).ToListAsync();
        if (doctors == null)
        {
            throw new Exception("Ошибка загрузки врачей!");
        }
        return doctors;
    }

    public async Task<List<Schedule>> GetDoctorScheduleByDoctorIdAsync(int doctorId)
    {
        var DoctorSchedule = await _context.Schedules.Where(e=>e.DoctorId == doctorId).ToListAsync();
        return DoctorSchedule;
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
}
