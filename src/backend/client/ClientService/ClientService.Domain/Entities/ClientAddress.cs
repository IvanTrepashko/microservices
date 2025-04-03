using ClientService.Domain.Common;

namespace ClientService.Domain.Entities;

public class ClientAddress(long clientId, Address address)
{
    public long Id { get; }
    public long ClientId { get; } = clientId;
    public Address Address { get; private set; } = address;
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public long Revision { get; private set; }

    public void UpdateAddress(Address address, DateTime updateTime)
    {
        Address = address;
        UpdatedAt = updateTime;
        Revision++;
    }
}
