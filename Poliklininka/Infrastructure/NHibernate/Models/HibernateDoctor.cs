namespace Poliklininka.Infrastructure.NHibernate.Models;

public class HibernateDoctor : HibernateUser
{
    public virtual string Specialization { get; set; } = string.Empty;

    public virtual string Office { get; set; } = string.Empty;

    public virtual IList<HibernateAppointment> Appointments { get; set; } = new List<HibernateAppointment>();
}