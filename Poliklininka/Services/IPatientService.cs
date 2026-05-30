using Poliklininka.Entities;

namespace Poliklininka.Services;

public interface IPatientService
{
    Task<Patient> GetPatientByUserIdAsync(int userId);
    Task UpdatePatientByUserIdAsync(Patient patient);
    Task<Patient?> GetMedCardByUserIdAsync(int userId);


}
