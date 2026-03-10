using ClientService.Domain.Common;

namespace ClientService.API.ApiModels;

public record CreateClientRequest(
    Name Name,
    Email Email,
    PhoneNumber PhoneNumber,
    Address Address,
    DateTime BirthDate,
    ClientGender Gender
);

public record NameApiModel(string FirstName, string LastName);

public record EmailApiModel(string Value);

public record PhoneNumberApiModel(string Value);

public record ClientAddressApiModel(
    string Street,
    string City,
    string State,
    string ZipCode,
    string Country
);

public enum ClientGender
{
    Male,
    Female,
}

public record ProfileInfoApiResponse();
