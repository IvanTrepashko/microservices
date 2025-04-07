namespace AuthService.Domain.Entities;

public class Client
{
    public long Id { get; private set; }

    public string Name { get; private set; }

    private Client() { }

    public Client(string name)
    {
        Name = name;
    }
}
