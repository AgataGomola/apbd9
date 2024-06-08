using Microsoft.AspNetCore.Mvc;
using tutorial_9.Repositories;
using tutorial_9.ResponseModels;
using tutorial_9.Services;

namespace tutorial_9.Controllers;

public class TripService: ITripService
{
    private ITripRepository _repository;

    public TripService(ITripRepository repository)
    {
        _repository = repository;
    }

    public async Task<ICollection<TripsDTO>> GetTrips(CancellationToken cancellationToken, int pageNum = 1, int pageSize = 10)
    {
        var result = await _repository.GetTrips(cancellationToken,pageNum, pageSize);
        return result;
    }
}