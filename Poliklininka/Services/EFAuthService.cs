using Microsoft.EntityFrameworkCore;
using Poliklininka.Entities;
using Poliklininka.Infrastructure.EF;
using System.Windows;

namespace Poliklininka.Services;

public class EFAuthService : IAuthService
{
    private readonly ApplicationDbContext _context;
    public EFAuthService(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<User> LoginAsync(string login, string password)
    {
        var existing_user = await _context.Users.FirstOrDefaultAsync(x => x.Login == login && x.Password == password);
        if (existing_user != null)
        {
            return existing_user;
        }
        else
        {
            
            return null!;
        }

    }
}
