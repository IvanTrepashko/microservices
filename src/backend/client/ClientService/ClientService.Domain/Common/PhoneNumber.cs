using System.Text.RegularExpressions;

namespace ClientService.Domain.Common;

public partial record PhoneNumber
{
    public string Value { get; }

    private PhoneNumber(string value)
    {
        Value = value;
    }

    public static PhoneNumber Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Phone number cannot be empty", nameof(value));
        }

        if (!PhoneNumberRegex().IsMatch(value))
        {
            throw new ArgumentException("Invalid phone number format", nameof(value));
        }

        return new PhoneNumber(value);
    }

    public static implicit operator string(PhoneNumber phoneNumber) => phoneNumber.Value;

    public static implicit operator PhoneNumber(string value) => Create(value);

    [GeneratedRegex(@"^\+?[1-9]\d{1,14}$")]
    private static partial Regex PhoneNumberRegex();
}
