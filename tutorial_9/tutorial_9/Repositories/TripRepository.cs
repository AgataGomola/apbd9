using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tutorial_9.Controllers;
using tutorial_9.Data;
using tutorial_9.ResponseModels;

namespace tutorial_9.Repositories;

public class TripRepository : ITripRepository
{
    private readonly Apbd2Context _context;

    public TripRepository(Apbd2Context context)
    {
        _context = context;
    }

    public async Task<ICollection<TripsDTO>> GetTrips(CancellationToken cancellationToken, int pageNum = 1, int pageSize = 10)
    {
        var totalTrips = await _context.Trips.CountAsync();
        var totalPages = (int)Math.Ceiling(totalTrips / (double)pageSize);

        var trips = await _context.Trips.Select(t => new TripsDTO
        {
            PageNum = pageNum,
            PageSize = pageSize,
            AllPages = totalPages,
            Name = t.Name,
            Description = t.Description,
            DateFrom = t.DateFrom,
            DateTo = t.DateTo,
            MaxPeople = t.MaxPeople,
            Countries = t.IdCountries.Select(c => new CountryDTO
            {
                Name = c.Name
            }),
            Clients = t.ClientTrips.Select(cl => new ClientDTO()
            {
                FirstName = cl.IdClientNavigation.FirstName,
                LastName = cl.IdClientNavigation.LastName
            })

        }).ToListAsync(cancellationToken);
       
        return trips;
    }
}