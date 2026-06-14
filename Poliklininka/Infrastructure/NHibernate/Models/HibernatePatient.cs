namespace Poliklininka.Infrastructure.NHibernate.Models;

public class HibernatePatient : HibernateUser
{
    public virtual string? Phone_number { get; set; }

    public virtual string Insurance_Policy { get; set; } = string.Empty;

    public virtual string? Address { get; set; }

    public virtual HibernateMedCard? MedCard { get; set; }

    public virtual IList<HibernateAppointment> Appointments { get; set; } = new List<HibernateAppointment>();

    public virtual IList<HibernateVisitHistory> VisitHistories { get; set; } = new List<HibernateVisitHistory>();
}