namespace Poliklininka.Entities;

public class Analysis
{
    public int Id { get; set; }
    public string AnalysisName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public ICollection<AnalysisHistory> AnalysisHistories { get; set; } = [];
}
