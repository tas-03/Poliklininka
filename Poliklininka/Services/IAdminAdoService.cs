using Poliklininka.Models.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poliklininka.Services;

public interface IAdminAdoService
{
    Task<List<AdminUserDto>> GetUsersAsync();
    Task AddUserAsync(AdminUserDto user);
    Task UpdateUserAsync(AdminUserDto user);
    Task DeleteUserAsync(int userId);
}
