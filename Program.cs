
using HotelListingApi.Configuration;
using HotelListingApi.Contracts;
using HotelListingApi.Data;
using HotelListingApi.Repository;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace HotelListingApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        var connectionString = builder.Configuration.GetConnectionString("HotelListingDbConnectionString");
        builder.Services.AddDbContext<HotelListingDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", b => b.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());
        });

        builder.Host.UseSerilog((ctx, lx) =>
        {
            lx.WriteTo.Console().ReadFrom.Configuration(ctx.Configuration);
        });

        builder.Services.AddAutoMapper(typeof(MapperConfig));

        builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRespository<>));
        builder.Services.AddScoped<ICountriesRepository, CountriesRepository>();


        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseSerilogRequestLogging();

        app.UseHttpsRedirection();

        app.UseCors("AllowAll");

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}