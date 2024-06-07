using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tutorial_9.Data;
using tutorial_9.Models;

namespace tutorial_9.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TripsController : ControllerBase
{
    private readonly Apbd2Context _context;

    public TripsController(Apbd2Context context)
    {
        _context = context;
    }
    [HttpGet]
    public async Task<IActionResult> GetTrips(int pageNum = 1, int pageSize = 10)
    {
        var totalTrips = await _context.Trips.CountAsync();
        var totalPages = (int)Math.Ceiling(totalTrips / (double)pageSize);

        var trips = await _context.Trips.Select(e=> new
        {
            Name = e.Name, 
            Description = e.Description,
            DateFrom = e.DateFrom,
            DateTo = e.DateTo,
            MaxPeople = e.MaxPeople,
            Countries = e.IdCountries.Select(c=> new
            {
                Name = c.Name 
            }),
            Clients = e.ClientTrips.Select(cl => new
            {
                Name = cl.IdClientNavigation.FirstName,
                SecondName = cl.IdClientNavigation.LastName
            })
        }).ToListAsync();
        var result = new
        {
            PageNum = pageNum,
            PageSize = pageSize,
            AllPages = totalPages,
            Trips = trips
        };
        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteClient(int id)
    {
        var client = await _context.Clients.FindAsync(id);

        if (client == null)
        {
            NotFound("Client does not exist");
        }
        var hasTrips = await _context.ClientTrips.AnyAsync(cl => cl.IdClient == id);
        if (hasTrips)
        {
            return BadRequest("Client with assigned trips cannot be deleted");
        }

        _context.Clients.Remove(client);
        await _context.SaveChangesAsync();
        return NoContent();
    }
    
    
}