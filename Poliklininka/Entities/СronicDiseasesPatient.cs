namespace Poliklininka.Entities;

public class СronicDiseasesPatient
{
    public int MedCardId { get; set; }
    public int ChronicDiseasesId { get; set; }

    public ChronicDiseases ChronicDiseases { get; set; } = null!;
    public MedCard MedCard { get; set; } = null!;

}
