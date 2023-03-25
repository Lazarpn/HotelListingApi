using System.ComponentModel.DataAnnotations;

namespace HotelListingApi.Models.Country;

public class UpdateCountryDto : BaseCountryDto
{
    [Required]
    public int Id { get; set; }
}
