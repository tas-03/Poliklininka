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
    public async Task<Patient> GetPatientByUserIdAsync(int userId)
    {
        var patient =await _context.Patients.FirstOrDefaultAsync(e => e.Id == userId);
        if (patient != null) return patient;
        else
        {
            throw new Exception("Невозможно загрузить профиль! Пациента с таким ID нет!");
        }
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
