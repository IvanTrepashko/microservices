namespace ApiGateway.Controllers.Areas.Client.ApiModels;

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