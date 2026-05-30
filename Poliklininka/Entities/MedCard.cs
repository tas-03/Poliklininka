namespace Poliklininka.Entities;

public class MedCard
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public int BloodGroupId { get; set; }
    public DateOnly OpenDate { get; set; }
    public bool? Disability { get; set; }
    public DateOnly DateOfBirth { get; set; }

    public ICollection<AllergyPatient> AllergyPatient { get; set; } = [];
    public ICollection<ÑronicDiseasesPatient> HronicDiseasesPatient { get; set; } = [];
    public Patient Patient { get; set; } = null!;
    public BloodGroup BloodGroup { get; set; } = null!;
}
