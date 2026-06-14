namespace Poliklininka.Infrastructure.NHibernate.Models;

public class HibernateVisitHistory
{
    public virtual int Id { get; set; }

    public virtual int AppointmentId { get; set; }

    public virtual int PatientId { get; set; }

    public virtual int MedServiceId { get; set; }

    public virtual DateTime VisitDate { get; set; }

    public virtual TimeSpan VisitTime { get; set; }

    public virtual string VisitResults { get; set; } = string.Empty;

    public virtual HibernateAppointment? Appointment { get; set; }

    public virtual HibernatePatient? Patient { get; set; }

    public virtual HibernateMedService? MedService { get; set; }

    public virtual IList<HibernateAnalysisHistory> AnalysisHistories { get; set; } = new List<HibernateAnalysisHistory>();

    public virtual IList<HibernateRecipeHistory> RecipeHistories { get; set; } = new List<HibernateRecipeHistory>();
}