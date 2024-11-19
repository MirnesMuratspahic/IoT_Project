using AlarmSystem.Models;
using AlarmSystem.Models.DTO;

namespace AlarmSystem.Services.Interfaces
{
    public interface IUserService
    {

        Task<ErrorProvider> UserRegistration(dtoUserRegistration user);
        Task<(ErrorProvider, string)> UserLogin(dtoUserLogin user);
        Task<(ErrorProvider, User)> GetUserInformations(string email);
        Task<ErrorProvider> UpdateUserInformations(dtoUserInformations dtoUserInformation);

    }
}
