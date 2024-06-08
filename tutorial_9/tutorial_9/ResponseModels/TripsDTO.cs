using tutorial_9.Models;

namespace tutorial_9.ResponseModels;

public class TripsDTO
{
    public int PageNum { get; set; }
    public int PageSize { get; set; }
    public int AllPages { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
    public int MaxPeople { get; set; }
    public IEnumerable<CountryDTO> Countries { get; set; } = new List<CountryDTO>();
    public IEnumerable<ClientDTO> Clients { get; set; } = new List<ClientDTO>();
}

