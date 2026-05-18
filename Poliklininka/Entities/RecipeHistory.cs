namespace Poliklininka.Entities;

public class RecipeHistory
{
    public int Id { get; set; }
    public int VisitHistoryId { get; set; }
    public int RecipeId { get; set; }
    public string Dosage { get; set; } = string.Empty;
    public string Duration { get; set; } = string.Empty;

    public VisitHistory VisitHistory { get; set; } = null!;
    public Recipe Recipe { get; set; } = null!;
}
