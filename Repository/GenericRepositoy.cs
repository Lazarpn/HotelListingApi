using HotelListingApi.Contracts;
using HotelListingApi.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelListingApi.Repository
{
    public class GenericRepositoy<T> : IGenericRepository<T> where T : class
    {
        private readonly HotelListingDbContext context;

        public GenericRepositoy(HotelListingDbContext context)
        {
            this.context = context;
        }
        public async Task<T> AddAsync(T entity)
        {
            await this.context.AddAsync(entity);
            await this.context.SaveChangesAsync() ;
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetAsync(id);
            this.context.Set<T>().Remove(entity);
            await this.context.SaveChangesAsync();

        }

        public async Task<bool> Exists(int id)
        {
            var entity = await GetAsync(id);
            return entity != null;
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await this.context.Set<T>().ToListAsync();
        }

        public async Task<T> GetAsync(int? id)
        {
            if(id == null)
            {
                return null;
            }

            return await this.context.Set<T>().FindAsync(id);
        }

        public async Task UpdateAsync(T entity)
        {
            this.context.Update(entity);
            await this.context.SaveChangesAsync();

        }
    }
}
