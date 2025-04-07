namespace ClientService.API.ApiModels;

public record CreateClientRequest(
    NameApiModel Name,
    EmailApiModel Email,
    PhoneNumberApiModel PhoneNumber,
    ClientAddressApiModel Address,
    DateTime BirthDate,
    ClientGender Gender
);

public record NameApiModel(string FirstName, string LastName);

public record EmailApiModel(string Value);

public record PhoneNumberApiModel(string Value);

public record ClientAddressApiModel(
    string AddressLine1,
    string AddressLine2,
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
