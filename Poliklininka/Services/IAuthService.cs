using Poliklininka.Entities;

namespace Poliklininka.Services;

public interface IAuthService
{
    Task<User> LoginAsync(string login, string password);
}
