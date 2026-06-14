using Poliklininka.Models.Admin;
using System.Data;

namespace Poliklininka.Services;

public interface IAdminAdoService
{
    Task<List<AdminUserDto>> GetUsersAsync();
    Task<DataTable> GetUserDetailsAsync(int userId, string role);

    Task AddUserAsync(AdminUserDto user);
    Task UpdateUserAsync(AdminUserDto user);
    Task DeleteUserAsync(int userId);
}