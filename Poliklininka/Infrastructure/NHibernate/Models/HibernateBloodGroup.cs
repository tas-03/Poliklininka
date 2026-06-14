namespace Poliklininka.Infrastructure.NHibernate.Models;

public class HibernateBloodGroup
{
    public virtual int Id { get; set; }

    public virtual string Name { get; set; } = string.Empty;

    public virtual string RhFactor { get; set; } = string.Empty;

    
    public virtual string? Office { get; set; }

    public virtual IList<HibernateMedCard> MedCards { get; set; } = new List<HibernateMedCard>();
}