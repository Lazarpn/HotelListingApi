using HotelListingApi.Contracts;
using HotelListingApi.Data;

namespace HotelListingApi.Repository
{
    public class HotelsRepository : GenericRepositoy<Hotel>, IHotelsRepository
    {
        private readonly HotelListingDbContext context;

        public HotelsRepository(HotelListingDbContext context) : base(context)
        {
            this.context = context;
        }
    }
}
