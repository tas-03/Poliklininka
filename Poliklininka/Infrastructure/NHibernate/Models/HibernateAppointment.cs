namespace Poliklininka.Infrastructure.NHibernate.Models;

public class HibernateAppointment
{
    public virtual int Id { get; set; }

    public virtual int PatientId { get; set; }

    public virtual int DoctorId { get; set; }

    public virtual int ScheduleId { get; set; }

    public virtual int? MedServiceId { get; set; }

    public virtual DateTime AppointmentDate { get; set; }

    public virtual string BookingStatus { get; set; } = string.Empty;

    public virtual DateTime CreatedAt { get; set; }

    public virtual HibernatePatient? Patient { get; set; }

    public virtual HibernateDoctor? Doctor { get; set; }

    public virtual HibernateMedService? MedService { get; set; }

    public virtual IList<HibernateVisitHistory> VisitHistories { get; set; } = new List<HibernateVisitHistory>();
}