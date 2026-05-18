namespace Poliklininka.Entities;

public class MedCard
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public int BloodGroupId { get; set; }
    public string CardNumber { get; set; } = string.Empty;
    public DateOnly OpenDate { get; set; }
    public string? Allergies { get; set; }
    public string? ChronicDiseases { get; set; }
    public bool Disability { get; set; }
    public DateOnly DateOfBirth { get; set; }

    public Patient Patient { get; set; } = null!;
    public BloodGroup BloodGroup { get; set; } = null!;
}
