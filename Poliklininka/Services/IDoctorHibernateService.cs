using Poliklininka.Infrastructure.NHibernate.Models;

namespace Poliklininka.Services.Hibernate;

public interface IDoctorHibernateService
{
    Task<IList<HibernateAppointment>> GetUpcomingAppointmentsAsync(int doctorId);

    Task<HibernateAppointment?> GetAppointmentAsync(int appointmentId);

    Task<HibernatePatient?> GetPatientAsync(int patientId);

    Task<HibernateMedCard?> GetPatientMedCardAsync(int patientId);

    Task<IList<HibernateVisitHistory>> GetPatientVisitHistoriesAsync(int patientId);

    Task<IList<HibernateBloodGroup>> GetBloodGroupsAsync();

    Task<IList<HibernateAllergy>> GetAllergiesAsync();

    Task<IList<HibernateChronicDisease>> GetChronicDiseasesAsync();

    Task<IList<HibernateAnalysis>> GetAnalysesAsync();

    Task<IList<HibernateRecipe>> GetRecipesAsync();

    Task SaveMedCardAsync(
        int patientId,
        DateTime dateOfBirth,
        bool disability,
        int? bloodGroupId);

    Task AddAllergyToPatientAsync(int patientId, int allergyId);

    Task AddChronicDiseaseToPatientAsync(int patientId, int chronicDiseaseId);

    Task<int> CreateVisitHistoryAsync(int appointmentId, string visitResults);

    Task AddAnalysisToVisitAsync(int visitHistoryId, int analysisId, string result);

    Task AddRecipeToVisitAsync(int visitHistoryId, int recipeId, string dosage, string duration);

    Task DeleteAnalysisHistoryAsync(int analysisHistoryId);

    Task DeleteRecipeHistoryAsync(int recipeHistoryId);
}