using HotelListingApi.Models.User;
using Microsoft.AspNetCore.Identity;

namespace HotelListingApi.Contracts
{
    public interface IAuthManager
    {
        Task<IEnumerable<IdentityError>> Register(ApiUserDto userDto);
        Task<AuthResponseDto> Login(LoginDto userDto);



    }
}
