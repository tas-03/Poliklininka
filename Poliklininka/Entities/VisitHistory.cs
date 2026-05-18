namespace Poliklininka.Entities;

public class VisitHistory
{
    public int Id { get; set; }
    public int AppointmentId { get; set; }
    public int PatientId { get; set; }
    public int MedServiceId { get; set; }
    public DateOnly VisitDate { get; set; }
    public TimeOnly VisitTime { get; set; }
    public string VisitResults { get; set; } = string.Empty;

    public Appointment Appointment { get; set; } = null!;
    public Patient Patient { get; set; } = null!;
    public MedService MedService { get; set; } = null!;
    public ICollection<AnalysisHistory> AnalysisHistories { get; set; } = [];
    public ICollection<RecipeHistory> RecipeHistories { get; set; } = [];
}
