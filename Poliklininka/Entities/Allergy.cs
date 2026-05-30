namespace Poliklininka.Entities;

public class Allergy
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;

    public ICollection<AllergyPatient> AllergyPatient { get; set; } = [];
}
