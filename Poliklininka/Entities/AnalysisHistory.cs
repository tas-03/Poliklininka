namespace Poliklininka.Entities;

public class AnalysisHistory
{
    public int Id { get; set; }
    public int VisitHistoryId { get; set; }
    public int AnalysisId { get; set; }
    public string Result { get; set; } = string.Empty;

    public VisitHistory VisitHistory { get; set; } = null!;
    public Analysis Analysis { get; set; } = null!;
}
