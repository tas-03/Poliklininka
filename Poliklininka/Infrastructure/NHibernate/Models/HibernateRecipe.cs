namespace Poliklininka.Infrastructure.NHibernate.Models;

public class HibernateRecipe
{
    public virtual int Id { get; set; }

    public virtual string Description { get; set; } = string.Empty;

    public virtual IList<HibernateRecipeHistory> RecipeHistories { get; set; } = new List<HibernateRecipeHistory>();
}