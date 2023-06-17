namespace WorkingGood.Infrastructure.Communication.Entities;

public record RemoveApplicationMessage
{
    public Guid OfferId { get; init; }
}