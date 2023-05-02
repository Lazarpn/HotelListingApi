using AutoMapper;
using HotelListingApi.Contracts;
using HotelListingApi.Data;
using HotelListingApi.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HotelListingApi.Repository
{
    public class AuthManager : IAuthManager
    {
        private readonly IMapper mapper;
        private readonly UserManager<ApiUser> userManager;
        private readonly IConfiguration configuration;

        public AuthManager(IMapper mapper, UserManager<ApiUser> userManager, IConfiguration configuration)
        {
            this.mapper = mapper;
            this.userManager = userManager;
            this.configuration = configuration;
        }

        public async Task<AuthResponseDto> Login(LoginDto loginDto)
        {

            var user = await this.userManager.FindByEmailAsync(loginDto.Email);
            bool isValidUser = await this.userManager.CheckPasswordAsync(user, loginDto.Password);
                
            
            if(user == null || isValidUser == false)
            {
                return null;

            }


            var token = await this.GenerateToken(user);

            return new AuthResponseDto
            {
                Token = token,
                UserId = user.Id
            };


        }

        public async Task<IEnumerable<IdentityError>> Register(ApiUserDto userDto)
        {
            var user = this.mapper.Map<ApiUser>(userDto);
            //user.UserName = user.Email;
            user.UserName = userDto.Email;

            var result = await this.userManager.CreateAsync(user, userDto.Password);

            if (result.Succeeded)
            {
                await this.userManager.AddToRoleAsync(user, "User");
            }

            return result.Errors;

        }

        private async Task<string> GenerateToken(ApiUser user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.configuration["JwtSettings:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var roles = await this.userManager.GetRolesAsync(user);

            var rolesClaims = roles.Select(x => new Claim(ClaimTypes.Role, x)).ToList();

            var userClaims = await this.userManager.GetClaimsAsync(user);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
            }.Union(userClaims).Union(rolesClaims);

            var token = new JwtSecurityToken(
                issuer: this.configuration["JwtSettings:Issuer"],
                audience: this.configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToInt32(this.configuration["JwtSettings:Minutes"])),
                signingCredentials: credentials
               );

            return new JwtSecurityTokenHandler().WriteToken(token);

            

        }


    }
}
