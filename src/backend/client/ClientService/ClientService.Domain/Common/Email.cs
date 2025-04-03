using System.Text.RegularExpressions;

namespace ClientService.Domain.Common;

public partial record Email
{
    private Email() { }

    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")]
    private static partial Regex EmailRegex();

    public string Value { get; private set; }

    private Email(string value)
    {
        Value = value;
    }

    public static Email Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Email cannot be empty", nameof(value));
        }

        if (!EmailRegex().IsMatch(value))
        {
            throw new ArgumentException("Invalid email format", nameof(value));
        }

        return new Email(value);
    }

    public static implicit operator string(Email email) => email.Value;

    public static implicit operator Email(string value) => Create(value);
}
