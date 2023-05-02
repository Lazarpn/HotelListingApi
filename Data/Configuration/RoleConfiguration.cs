using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelListingApi.Data.Configuration
{
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
 
        {
            builder.HasData(
                new IdentityRole
                {
                    Name = "Aministrator",
                    NormalizedName = "ADMINISTRATOR"
                },
                new IdentityRole
                {
                    Name = "User",
                    NormalizedName = "USER"
                }

            );
                
        }
    }
}
