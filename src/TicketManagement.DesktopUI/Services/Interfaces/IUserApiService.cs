using System.Collections.Generic;
using System.Threading.Tasks;
using TicketManagement.DesktopUI.Models;

namespace TicketManagement.DesktopUI.Services.Interfaces
{
    public interface IUserApiService
    {
        Task<bool> AddRoleAsync(string login, string role);
        Task<AuthenticatedUserModel> AuthenticateAsync(string login, string password);
        Task<bool> DeleteRoleAsync(string login, string role);
        Task<bool> DeleteUserAsync(string login);
        Task<IEnumerable<ProfileModel>> GetAllAsync();
        Task<IEnumerable<RoleModel>> GetRolesAsync();
        Task<bool> UpdateUserAsync(ProfileModel model);
    }
}