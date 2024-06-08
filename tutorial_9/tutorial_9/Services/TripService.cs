using tutorial_9.Models;
using tutorial_9.Repositories;
using tutorial_9.RequestModels;
using tutorial_9.ResponseModels;

namespace tutorial_9.Services;

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

    public Task<int> DeleteClient(CancellationToken cancellationToken, int id)
    {
        return _repository.DeleteClient(cancellationToken, id);
    }

    public async Task<bool> DoesClientExist(int id)
    {
        var client = await _repository.DoesClientExist(id);
        return client;
    }

    public async Task<bool> HasTrips(int id)
    {
        var hasTrips = await _repository.HasTrips(id);
        return hasTrips;
    }
    public async Task<bool> DoesPeselExist(string pesel)
    {
        var client = await _repository.DoesPeselExist(pesel);
        return client;
    }
    public async Task<bool> HasAssignedTrip(int id, string pesel)
    {
        var client = await _repository.HasAssignedTrip(id, pesel);
        return client;
    }
    public async Task<bool> DoesTripExist(int idTrip)
    {
        var trip = await _repository.DoesTripExist(idTrip);
        return trip;
    }

    public async Task<bool> IsTripInFuture(int idTrip)
    {
        var trip = await _repository.IsTripInFuture(idTrip);
        return trip;
    }

    public async Task AttachClient(int idTrip, ClientAttachDTO cl)
    {
        var client = new Client
        {
            FirstName = cl.FirstName,
            LastName = cl.LastName,
            Email = cl.Email,
            Telephone = cl.Telephone,
            Pesel = cl.Pesel,
        };
        await _repository.AddClient(client);
        var trip = await _repository.GetTrip(idTrip);
        
        var clientTrip = new ClientTrip()
        {
            IdClient = client.IdClient,
            IdTrip = idTrip,
            RegisteredAt = DateTime.Now,
            PaymentDate = cl.PaymentDate,
            IdClientNavigation = client,
            IdTripNavigation = trip
        };
        await _repository.AddClientTrip(clientTrip);
    }
}