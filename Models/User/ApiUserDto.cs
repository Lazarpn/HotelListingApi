using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;
using RequiredAttribute = Microsoft.Build.Framework.RequiredAttribute;

namespace HotelListingApi.Models.User;

public class ApiUserDto : LoginDto
{
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
  
}
