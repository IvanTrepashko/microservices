namespace ClientService.Domain.Entities;

public class ClientPreferences(
    long clientId,
    bool receiveEmailNotifications,
    bool receiveSmsNotifications,
    DateTime createdAt
)
{
    public long Id { get; }
    public long ClientId { get; } = clientId;
    public bool ReceiveEmailNotifications { get; private set; } = receiveEmailNotifications;
    public bool ReceiveSmsNotifications { get; private set; } = receiveSmsNotifications;
    public DateTime CreatedAt { get; private set; } = createdAt;
    public DateTime UpdatedAt { get; private set; }
    public long Revision { get; private set; }

    public void UpdatePreferences(
        bool receiveEmailNotifications,
        bool receiveSmsNotifications,
        DateTime updateTime
    )
    {
        ReceiveEmailNotifications = receiveEmailNotifications;
        ReceiveSmsNotifications = receiveSmsNotifications;
        IncrementRevision(updateTime);
    }

    private void IncrementRevision(DateTime updateTime)
    {
        UpdatedAt = updateTime;
        Revision++;
    }
}
