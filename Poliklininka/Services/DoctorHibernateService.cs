using NHibernate.Linq;
using Poliklininka.Infrastructure.NHibernate;
using Poliklininka.Infrastructure.NHibernate.Models;

namespace Poliklininka.Services.Hibernate;

public class DoctorHibernateService : IDoctorHibernateService
{
    public Task<IList<HibernateAppointment>> GetUpcomingAppointmentsAsync(int doctorId)
    {
        using var session = NHibernateHelper.OpenSession();

        // Get — специально оставляем для требования преподавателя.
        var doctor = session.Get<HibernateDoctor>(doctorId);

        if (doctor == null)
            return Task.FromResult<IList<HibernateAppointment>>(new List<HibernateAppointment>());

        var appointments = session.Query<HibernateAppointment>()
            .Where(a => a.DoctorId == doctor.Id)
            .Where(a => a.BookingStatus != "cancelled")
            .Where(a => a.BookingStatus != "completed")
            .OrderBy(a => a.AppointmentDate)
            .ToList();

        return Task.FromResult<IList<HibernateAppointment>>(appointments);
    }

    public Task<HibernateAppointment?> GetAppointmentAsync(int appointmentId)
    {
        using var session = NHibernateHelper.OpenSession();

        // Get — явная демонстрация NHibernate Get.
        var appointment = session.Get<HibernateAppointment>(appointmentId);

        return Task.FromResult(appointment);
    }

    public Task<HibernatePatient?> GetPatientAsync(int patientId)
    {
        using var session = NHibernateHelper.OpenSession();

        // Get — получаем пациента по ID.
        var patient = session.Get<HibernatePatient>(patientId);

        return Task.FromResult(patient);
    }

    public Task<HibernateMedCard?> GetPatientMedCardAsync(int patientId)
    {
        using var session = NHibernateHelper.OpenSession();

        var medCard = session.Query<HibernateMedCard>()
            .FirstOrDefault(m => m.PatientId == patientId);

        return Task.FromResult(medCard);
    }

    public Task<IList<HibernateVisitHistory>> GetPatientVisitHistoriesAsync(int patientId)
    {
        using var session = NHibernateHelper.OpenSession();

        var histories = session.Query<HibernateVisitHistory>()
            .Where(v => v.PatientId == patientId)
            .OrderByDescending(v => v.VisitDate)
            .ThenByDescending(v => v.VisitTime)
            .ToList();

        return Task.FromResult<IList<HibernateVisitHistory>>(histories);
    }

    public Task<IList<HibernateBloodGroup>> GetBloodGroupsAsync()
    {
        using var session = NHibernateHelper.OpenSession();

        var bloodGroups = session.Query<HibernateBloodGroup>()
            .OrderBy(b => b.Name)
            .ThenBy(b => b.RhFactor)
            .ToList();

        return Task.FromResult<IList<HibernateBloodGroup>>(bloodGroups);
    }

    public Task<IList<HibernateAllergy>> GetAllergiesAsync()
    {
        using var session = NHibernateHelper.OpenSession();

        var allergies = session.Query<HibernateAllergy>()
            .OrderBy(a => a.Name)
            .ToList();

        return Task.FromResult<IList<HibernateAllergy>>(allergies);
    }

    public Task<IList<HibernateChronicDisease>> GetChronicDiseasesAsync()
    {
        using var session = NHibernateHelper.OpenSession();

        var diseases = session.Query<HibernateChronicDisease>()
            .OrderBy(d => d.Name)
            .ToList();

        return Task.FromResult<IList<HibernateChronicDisease>>(diseases);
    }

    public Task<IList<HibernateAnalysis>> GetAnalysesAsync()
    {
        using var session = NHibernateHelper.OpenSession();

        var analyses = session.Query<HibernateAnalysis>()
            .OrderBy(a => a.AnalysisName)
            .ToList();

        return Task.FromResult<IList<HibernateAnalysis>>(analyses);
    }

    public Task<IList<HibernateRecipe>> GetRecipesAsync()
    {
        using var session = NHibernateHelper.OpenSession();

        var recipes = session.Query<HibernateRecipe>()
            .OrderBy(r => r.Description)
            .ToList();

        return Task.FromResult<IList<HibernateRecipe>>(recipes);
    }

    public Task SaveMedCardAsync(
        int patientId,
        DateTime dateOfBirth,
        bool disability,
        int? bloodGroupId)
    {
        using var session = NHibernateHelper.OpenSession();
        using var transaction = session.BeginTransaction();

        try
        {
            var patient = session.Get<HibernatePatient>(patientId);

            if (patient == null)
                throw new InvalidOperationException("Пациент не найден.");

            var medCard = session.Query<HibernateMedCard>()
                .FirstOrDefault(m => m.PatientId == patientId);

            if (medCard == null)
            {
                medCard = new HibernateMedCard
                {
                    PatientId = patientId,
                    DateOfBirth = dateOfBirth,
                    Disability = disability,
                    BloodGroupId = bloodGroupId,
                    OpenDate = DateTime.Today
                };

                // Save — создаем медицинскую карту.
                session.Save(medCard);
            }
            else
            {
                medCard.DateOfBirth = dateOfBirth;
                medCard.Disability = disability;
                medCard.BloodGroupId = bloodGroupId;
            }

            // Flush — отправляем изменения в БД.
            session.Flush();

            // Commit — подтверждаем транзакцию.
            transaction.Commit();

            return Task.CompletedTask;
        }
        catch
        {
            if (transaction.IsActive)
                transaction.Rollback();

            throw;
        }
    }

    public Task AddAllergyToPatientAsync(int patientId, int allergyId)
    {
        using var session = NHibernateHelper.OpenSession();
        using var transaction = session.BeginTransaction();

        try
        {
            var medCard = session.Query<HibernateMedCard>()
                .FirstOrDefault(m => m.PatientId == patientId);

            if (medCard == null)
                throw new InvalidOperationException("Сначала создайте медицинскую карту пациента.");

            var allergy = session.Get<HibernateAllergy>(allergyId);

            if (allergy == null)
                throw new InvalidOperationException("Аллергия не найдена.");

            var alreadyExists = medCard.Allergies.Any(a => a.Id == allergy.Id);

            if (!alreadyExists)
            {
                medCard.Allergies.Add(allergy);
            }

            session.Flush();
            transaction.Commit();

            return Task.CompletedTask;
        }
        catch
        {
            if (transaction.IsActive)
                transaction.Rollback();

            throw;
        }
    }

    public Task AddChronicDiseaseToPatientAsync(int patientId, int chronicDiseaseId)
    {
        using var session = NHibernateHelper.OpenSession();
        using var transaction = session.BeginTransaction();

        try
        {
            var medCard = session.Query<HibernateMedCard>()
                .FirstOrDefault(m => m.PatientId == patientId);

            if (medCard == null)
                throw new InvalidOperationException("Сначала создайте медицинскую карту пациента.");

            var disease = session.Get<HibernateChronicDisease>(chronicDiseaseId);

            if (disease == null)
                throw new InvalidOperationException("Хроническое заболевание не найдено.");

            var alreadyExists = medCard.ChronicDiseases.Any(d => d.Id == disease.Id);

            if (!alreadyExists)
            {
                medCard.ChronicDiseases.Add(disease);
            }

            session.Flush();
            transaction.Commit();

            return Task.CompletedTask;
        }
        catch
        {
            if (transaction.IsActive)
                transaction.Rollback();

            throw;
        }
    }

    public Task<int> CreateVisitHistoryAsync(int appointmentId, string visitResults)
    {
        using var session = NHibernateHelper.OpenSession();
        using var transaction = session.BeginTransaction();

        try
        {
            var appointment = session.Get<HibernateAppointment>(appointmentId);

            if (appointment == null)
                throw new InvalidOperationException("Запись на прием не найдена.");

            if (appointment.MedServiceId == null)
                throw new InvalidOperationException("У записи не указана медицинская услуга.");

            var existingVisit = session.Query<HibernateVisitHistory>()
                .FirstOrDefault(v => v.AppointmentId == appointmentId);

            if (existingVisit != null)
            {
                existingVisit.VisitResults = visitResults;
                existingVisit.VisitDate = DateTime.Today;
                existingVisit.VisitTime = DateTime.Now.TimeOfDay;

                appointment.BookingStatus = "completed";

                session.Flush();
                transaction.Commit();

                return Task.FromResult(existingVisit.Id);
            }

            var visitHistory = new HibernateVisitHistory
            {
                AppointmentId = appointment.Id,
                PatientId = appointment.PatientId,
                MedServiceId = appointment.MedServiceId.Value,
                VisitDate = DateTime.Today,
                VisitTime = DateTime.Now.TimeOfDay,
                VisitResults = visitResults
            };

            // Save — создаем историю посещения.
            session.Save(visitHistory);

            appointment.BookingStatus = "completed";

            session.Flush();
            transaction.Commit();

            return Task.FromResult(visitHistory.Id);
        }
        catch
        {
            if (transaction.IsActive)
                transaction.Rollback();

            throw;
        }
    }

    public Task AddAnalysisToVisitAsync(int visitHistoryId, int analysisId, string result)
    {
        using var session = NHibernateHelper.OpenSession();
        using var transaction = session.BeginTransaction();

        try
        {
            var visitHistory = session.Get<HibernateVisitHistory>(visitHistoryId);

            if (visitHistory == null)
                throw new InvalidOperationException("История посещения не найдена.");

            var analysis = session.Get<HibernateAnalysis>(analysisId);

            if (analysis == null)
                throw new InvalidOperationException("Анализ не найден.");

            var analysisHistory = new HibernateAnalysisHistory
            {
                VisitHistoryId = visitHistoryId,
                AnalysisId = analysisId,
                Result = result
            };

            // Save — назначаем анализ.
            session.Save(analysisHistory);

            session.Flush();
            transaction.Commit();

            return Task.CompletedTask;
        }
        catch
        {
            if (transaction.IsActive)
                transaction.Rollback();

            throw;
        }
    }

    public Task AddRecipeToVisitAsync(int visitHistoryId, int recipeId, string dosage, string duration)
    {
        using var session = NHibernateHelper.OpenSession();
        using var transaction = session.BeginTransaction();

        try
        {
            var visitHistory = session.Get<HibernateVisitHistory>(visitHistoryId);

            if (visitHistory == null)
                throw new InvalidOperationException("История посещения не найдена.");

            var recipe = session.Get<HibernateRecipe>(recipeId);

            if (recipe == null)
                throw new InvalidOperationException("Препарат не найден.");

            var recipeHistory = new HibernateRecipeHistory
            {
                VisitHistoryId = visitHistoryId,
                RecipeId = recipeId,
                Dosage = dosage,
                Duration = duration
            };

            // Save — назначаем препарат.
            session.Save(recipeHistory);

            session.Flush();
            transaction.Commit();

            return Task.CompletedTask;
        }
        catch
        {
            if (transaction.IsActive)
                transaction.Rollback();

            throw;
        }
    }

    public Task DeleteAnalysisHistoryAsync(int analysisHistoryId)
    {
        using var session = NHibernateHelper.OpenSession();
        using var transaction = session.BeginTransaction();

        try
        {
            var analysisHistory = session.Get<HibernateAnalysisHistory>(analysisHistoryId);

            if (analysisHistory == null)
                return Task.CompletedTask;

            // Delete — удаляем назначенный анализ.
            session.Delete(analysisHistory);

            session.Flush();
            transaction.Commit();

            return Task.CompletedTask;
        }
        catch
        {
            if (transaction.IsActive)
                transaction.Rollback();

            throw;
        }
    }

    public Task DeleteRecipeHistoryAsync(int recipeHistoryId)
    {
        using var session = NHibernateHelper.OpenSession();
        using var transaction = session.BeginTransaction();

        try
        {
            var recipeHistory = session.Get<HibernateRecipeHistory>(recipeHistoryId);

            if (recipeHistory == null)
                return Task.CompletedTask;

            // Delete — удаляем назначенный препарат.
            session.Delete(recipeHistory);

            session.Flush();
            transaction.Commit();

            return Task.CompletedTask;
        }
        catch
        {
            if (transaction.IsActive)
                transaction.Rollback();

            throw;
        }
    }
}