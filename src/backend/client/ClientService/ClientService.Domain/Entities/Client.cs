using ClientService.Domain.Common;

namespace ClientService.Domain.Entities;

public class Client
{
    public long Id { get; }
    public Name Name { get; private set; }
    public Email Email { get; private set; }
    public PhoneNumber PhoneNumber { get; private set; }
    public ClientAddress Address { get; private set; }
    public DateTime BirthDate { get; private set; }
    public ClientType Type { get; private set; }
    public ClientStatus Status { get; private set; }
    public ClientGender Gender { get; private set; }
    public long Revision { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public Client(
        Name name,
        Email email,
        PhoneNumber phoneNumber,
        ClientAddress address,
        DateTime birthDate,
        ClientType type,
        ClientStatus status,
        ClientGender gender,
        DateTime createdAt
    )
    {
        Name = name;
        Email = email;
        PhoneNumber = phoneNumber;
        Address = address;
        BirthDate = birthDate;
        Type = type;
        Status = status;
        Gender = gender;
        CreatedAt = createdAt;
    }

    private Client() { }

    private void IncrementRevision(DateTime updateTime)
    {
        UpdatedAt = updateTime;
        Revision++;
    }

    public void ChangeName(Name name, DateTime updateTime)
    {
        _ = name ?? throw new ArgumentNullException(nameof(name));

        Name = name;
        IncrementRevision(updateTime);
    }

    public void ChangeEmail(Email email, DateTime updateTime)
    {
        _ = email ?? throw new ArgumentNullException(nameof(email));

        Email = email;
        IncrementRevision(updateTime);
    }

    public void ChangePhoneNumber(PhoneNumber phoneNumber, DateTime updateTime)
    {
        _ = phoneNumber ?? throw new ArgumentNullException(nameof(phoneNumber));

        PhoneNumber = phoneNumber;
        IncrementRevision(updateTime);
    }

    public void ChangeAddress(Address address, DateTime updateTime)
    {
        _ = address ?? throw new ArgumentNullException(nameof(address));

        Address.UpdateAddress(address, updateTime);
        IncrementRevision(updateTime);
    }

    public void ChangeBirthDate(DateTime birthDate, DateTime updateTime)
    {
        BirthDate = birthDate;
        IncrementRevision(updateTime);
    }

    public void Deactivate(DateTime updateTime)
    {
        Status = ClientStatus.Inactive;
        IncrementRevision(updateTime);
    }

    public void Activate(DateTime updateTime)
    {
        Status = ClientStatus.Active;
        IncrementRevision(updateTime);
    }

    public void ChangeGender(ClientGender gender, DateTime updateTime)
    {
        Gender = gender;
        IncrementRevision(updateTime);
    }
}

public enum ClientType
{
    Individual,
    Business,
}

public enum ClientStatus
{
    Active,
    Inactive,
}

public enum ClientGender
{
    Male,
    Female,
}
