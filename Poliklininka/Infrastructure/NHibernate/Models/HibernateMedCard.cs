namespace Poliklininka.Infrastructure.NHibernate.Models;

public class HibernateMedCard
{
    public virtual int Id { get; set; }

    public virtual int PatientId { get; set; }

    public virtual int? BloodGroupId { get; set; }

    public virtual DateTime DateOfBirth { get; set; }

    public virtual bool Disability { get; set; }

    public virtual DateTime OpenDate { get; set; }

    public virtual HibernatePatient? Patient { get; set; }

    public virtual HibernateBloodGroup? BloodGroup { get; set; }

    public virtual IList<HibernateAllergy> Allergies { get; set; } = new List<HibernateAllergy>();

    public virtual IList<HibernateChronicDisease> ChronicDiseases { get; set; } = new List<HibernateChronicDisease>();
}