namespace Poliklininka.Infrastructure.NHibernate.Models;

public class HibernateRecipeHistory
{
    public virtual int Id { get; set; }

    public virtual int VisitHistoryId { get; set; }

    public virtual int RecipeId { get; set; }

    public virtual string Dosage { get; set; } = string.Empty;

    public virtual string Duration { get; set; } = string.Empty;

    public virtual HibernateVisitHistory? VisitHistory { get; set; }

    public virtual HibernateRecipe? Recipe { get; set; }
}