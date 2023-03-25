using HotelListingApi.Data;

namespace HotelListingApi.Contracts;

public interface ICountriesRepository : IGenericRepository<Country>
{

    Task<Country> GetDetails(int id);

}
