using Microsoft.AspNetCore.Mvc;
using tutorial_9.RequestModels;
using tutorial_9.Services;

namespace tutorial_9.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TripsController : ControllerBase
{
    private readonly ITripService _tripService;

    public TripsController(ITripService tripService)
    {
        _tripService = tripService;
    }
    [HttpGet]
    public async Task<IActionResult> GetTrips(CancellationToken cancellationToken,int pageNum = 1, int pageSize = 10)
    {
        var result = await _tripService.GetTrips(cancellationToken, pageNum, pageSize);
        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteClient(CancellationToken cancellationToken, int id)
    {
        var doesClientExist = await _tripService.DoesClientExist(id);
        
        if (!doesClientExist)
        {
            NotFound("Client does not exist");
        }
        
        var hasTrips = await _tripService.HasTrips(id);
        if (hasTrips)
        {
            return BadRequest("Client with assigned trips cannot be deleted");
        }

        var delete = await _tripService.DeleteClient(cancellationToken, id);
        if (delete == -1)
        {
            return BadRequest("Client could not be deleted");
        }
        return NoContent();
    }

    [HttpPost("{idTrip:int}/clients")]
    public async Task<IActionResult> AttachClient(int idTrip, [FromBody] ClientAttachDTO cl)
    {
        var doesPeselExist = await _tripService.DoesPeselExist(cl.Pesel);
        
        if (doesPeselExist)
        {
            return BadRequest("Client already exists.");   
        }
        
        var tripExist = await _tripService.DoesTripExist(idTrip);
        if (!tripExist)
        {
            return NotFound($"Trip with id {idTrip} does not exist.");
        }
        
        var isAssigned = await _tripService.HasAssignedTrip(idTrip, cl.Pesel);
        if (isAssigned)
        {
            return BadRequest("Client is already assigned to this trip");
        }
        
        var isTripInTheFuture = await _tripService.IsTripInFuture(idTrip);
        
        if (isTripInTheFuture)
        {
            return BadRequest("Trip already started.");
        }

        await _tripService.AttachClient(idTrip, cl);
        return NoContent();
    }
    
}