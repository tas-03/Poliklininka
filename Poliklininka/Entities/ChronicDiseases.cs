namespace Poliklininka.Entities;

public class ChronicDiseases
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;

    public ICollection<СronicDiseasesPatient> HronicDiseasesPatient = [];
}
