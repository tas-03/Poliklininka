namespace Poliklininka.Infrastructure.NHibernate.Models;

public class HibernateMedService
{
    public virtual int Id { get; set; }

    public virtual string ServiceName { get; set; } = string.Empty;

    public virtual decimal Cost { get; set; }

    public virtual string Category { get; set; } = string.Empty;

    public virtual IList<HibernateAppointment> Appointments { get; set; } = new List<HibernateAppointment>();

    public virtual IList<HibernateVisitHistory> VisitHistories { get; set; } = new List<HibernateVisitHistory>();
}