using tutorial_9.RequestModels;
using tutorial_9.ResponseModels;

namespace tutorial_9.Services;

public interface ITripService
{
    public Task<ICollection<TripsDTO>> GetTrips(CancellationToken cancellationToken, int pageNum = 1, int pageSize = 10);
    public Task<int> DeleteClient(CancellationToken cancellationToken, int id);
    public Task<bool> DoesClientExist(int id);
    public Task<bool> HasTrips(int id);
    public Task<bool> DoesPeselExist(string pesel);
    public Task<bool> HasAssignedTrip(int id, string pesel);
    public Task<bool> DoesTripExist(int idTrip);
    public Task<bool> IsTripInFuture(int idTrip);
    public Task AttachClient(int idTrip, ClientAttachDTO cl);
}