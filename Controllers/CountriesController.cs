using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelListingApi.Data;
using HotelListingApi.Models.Country;
using AutoMapper;
using HotelListingApi.Contracts;

namespace HotelListingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
       
        private readonly IMapper mapper;
        private readonly ICountriesRepository countriesRepository;

        public CountriesController(IMapper mapper, ICountriesRepository countriesRepository)
        {

            this.mapper = mapper;
            this.countriesRepository = countriesRepository;
        }

        // GET: api/Countries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetCountryDto>>> GetCountries()
        {
            var countries = await this.countriesRepository.GetAllAsync();
            var records = this.mapper.Map<List<GetCountryDto>>(countries);
            return Ok(records); 
        }

        // GET: api/Countries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CountryDto>> GetCountry(int id)
        {
            var country = await this.countriesRepository.GetDetails(id);

            if (country == null)
            {
                return NotFound();
            }

            var countryDto = this.mapper.Map<CountryDto>(country);

            return countryDto;
        }

        // PUT: api/Countries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCountry(int id, UpdateCountryDto updateCountryDto)
        {
            if (id != updateCountryDto.Id)
            {
                return BadRequest("Invalid Record Id");
            }

            //this.context.Entry(country).State = EntityState.Modified;

            var country = await this.countriesRepository.GetAsync(id);

            if(country == null)
            {
                return NotFound();
            }

            this.mapper.Map(updateCountryDto, country);

            try
            {
                await this.countriesRepository.UpdateAsync(country);
                //await this.context.SaveChangesAsync();

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await CountryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Countries
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Country>> PostCountry(CreateCountryDto createCountry)
        {
            //var countryOld = new Country
            //{
            //    Name = createCountry.Name,
            //    ShortName = createCountry.ShortName,
            //};

            var country = mapper.Map<Country>(createCountry);

            //this.context.Countries.Add(country);
            //await this.context.SaveChangesAsync();

            await this.countriesRepository.AddAsync(country);

            return CreatedAtAction("GetCountry", new { id = country.Id }, country);
        }

        // DELETE: api/Countries/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            var country = await this.countriesRepository.GetAsync(id);
            if (country == null)
            {
                return NotFound();
            }

            await this.countriesRepository.DeleteAsync(id);

            return NoContent();
        }

        private async Task<bool> CountryExists(int id)
        {
            //return this.context.Countries.Any(e => e.Id == id);

            return await this.countriesRepository.Exists(id);

        }
    }
}
