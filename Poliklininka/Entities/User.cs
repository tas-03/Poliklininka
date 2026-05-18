namespace Poliklininka.Entities;

public class User
{
    public int Id { get; set; }
    public string Login { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Full_Name { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string Discriminator { get; set; } = string.Empty;
    public byte[]? Photo { get; set; }

}
