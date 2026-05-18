namespace Poliklininka.Entities;

public class Recipe
{
    public int Id { get; set; }
    public string Description { get; set; } = string.Empty;

    public ICollection<RecipeHistory> RecipeHistories { get; set; } = [];
}
