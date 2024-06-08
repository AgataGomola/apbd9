using Microsoft.AspNetCore.Mvc;
using tutorial_9.ResponseModels;

namespace tutorial_9.Repositories;

public interface ITripRepository
{
    public Task<ICollection<TripsDTO>> GetTrips(CancellationToken cancellationToken, int pageNum = 1, int pageSize = 10);

}