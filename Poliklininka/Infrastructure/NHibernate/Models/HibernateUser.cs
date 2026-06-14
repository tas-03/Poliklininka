namespace Poliklininka.Infrastructure.NHibernate.Models;

public class HibernateUser
{
    public virtual int Id { get; set; }

    public virtual string Login { get; set; } = string.Empty;

    public virtual string Password { get; set; } = string.Empty;

    public virtual string Full_Name { get; set; } = string.Empty;

    public virtual string Role { get; set; } = string.Empty;

    public virtual string? Discriminator { get; set; }
}