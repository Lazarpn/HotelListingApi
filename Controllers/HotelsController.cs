using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelListingApi.Data;
using HotelListingApi.Contracts;
using AutoMapper;
using HotelListingApi.Models.Hotel;

namespace HotelListingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelsController : ControllerBase
    {
    
        private readonly IHotelsRepository hotelsRepository;
        private readonly IMapper mapper;

        public HotelsController( IHotelsRepository hotelsRepository, IMapper mapper)
        {
         
            this.hotelsRepository = hotelsRepository;
            this.mapper = mapper;
        }

        // GET: api/Hotels
        [HttpGet]
        public async Task<ActionResult<List<HotelDto>>> GetHotels()
        {
            var hotels = await this.hotelsRepository.GetAllAsync();
            var records = this.mapper.Map<List<HotelDto>>(hotels);
            return Ok(records);
        }

        // GET: api/Hotels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HotelDto>> GetHotel(int id)
        {
            var hotel = await this.hotelsRepository.GetAsync(id);

            if (hotel == null)
            {
                return NotFound();
            }

            var record = this.mapper.Map<HotelDto>(hotel);

            return Ok(record);
        }

        // PUT: api/Hotels/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHotel(int id, Hotel hotelDto)
        {
            if (id != hotelDto.Id)
            {
                return BadRequest();
            }

            var hotel = await this.hotelsRepository.GetAsync(id);
            if (hotel == null)
            {
                return NotFound();
            }

            this.mapper.Map(hotelDto, hotel);



            try
            {
                await this.hotelsRepository.UpdateAsync(hotel);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (! await HotelExists(id))
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

        // POST: api/Hotels
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Hotel>> PostHotel(CreateHotelDto hotelDto)
        {
            var hotel = this.mapper.Map<Hotel>(hotelDto);
            await this.hotelsRepository.AddAsync(hotel);

            return CreatedAtAction("GetHotel", new { id = hotel.Id }, hotel);
        }

        // DELETE: api/Hotels/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            var hotel = await this.hotelsRepository.GetAsync(id);
            if (hotel == null)
            {
                return NotFound();
            }

            await this.hotelsRepository.DeleteAsync(id);

            return NoContent();
        }

        private async Task<bool> HotelExists(int id)
        {
            return await this.hotelsRepository.Exists(id);
        }
    }
}
