namespace Poliklininka.Infrastructure.NHibernate.Models;

public class HibernateChronicDisease
{
    public virtual int Id { get; set; }

    public virtual string Name { get; set; } = string.Empty;

    public virtual string Code { get; set; } = string.Empty;

    public virtual IList<HibernateMedCard> MedCards { get; set; } = new List<HibernateMedCard>();
}