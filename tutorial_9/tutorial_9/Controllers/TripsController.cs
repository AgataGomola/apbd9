using System.Runtime.InteropServices.JavaScript;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tutorial_9.Data;
using tutorial_9.RequestModels;
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

    [HttpPost("{idTrip:int}/clients")]
    public async Task<IActionResult> attachClient(int idTrip, [FromBody] ClientAttachDTO cl)
    {
        var trip = await _context.Trips.FindAsync(idTrip);
        if (trip == null)
        {
            return NotFound($"Trip with id {idTrip} does not exist.");
        }

        if (trip.DateFrom < DateTime.Now)
        {
            return BadRequest("Trip already started.");
        }
        
        var client = await _context.Clients.SingleOrDefaultAsync(c => c.Pesel == cl.Pesel);
        
        if (client != null)
        {
            var isAssigned = await _context.ClientTrips.AnyAsync(t => t.IdClientNavigation.Pesel == cl.Pesel && t.IdTrip == idTrip);
            if (isAssigned)
            {
                return BadRequest("Client is already assigned to this trip");
            }
            return BadRequest("Client already exists.");   
        }
        client = new Client()
        {
            FirstName = cl.FirstName,
            LastName = cl.LastName,
            Email = cl.Email,
            Telephone = cl.Telephone,
            Pesel = cl.Pesel,
        };
        _context.Clients.Add(client);
        await _context.SaveChangesAsync();

        var clientTrip = new ClientTrip()
        {
           IdClient = client.IdClient,
           IdTrip = idTrip,
           RegisteredAt = DateTime.Now,
           PaymentDate = cl.PaymentDate,
           IdClientNavigation = client,
           IdTripNavigation = trip
        };
        _context.ClientTrips.Add(clientTrip);
        await _context.SaveChangesAsync();
        return NoContent();
    }
    
}