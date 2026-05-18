namespace Poliklininka.Entities;

public class BloodGroup
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string RhFactor { get; set; } = string.Empty;
    public string Office { get; set; } = string.Empty;

    public ICollection<MedCard> MedCards { get; set; } = [];
}
