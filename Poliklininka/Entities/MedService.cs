namespace Poliklininka.Entities;

public class MedService
{
    public int Id { get; set; }
    public string ServiceName { get; set; } = string.Empty;
    public decimal Cost { get; set; }
    public string Category { get; set; } = string.Empty;

    public ICollection<VisitHistory> VisitHistories { get; set; } = [];
}
