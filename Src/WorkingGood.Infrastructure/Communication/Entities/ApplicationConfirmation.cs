namespace WorkingGood.Infrastructure.Communication.Entities;

public record ApplicationConfirmation
{
    public string CandidateEmail { get; init; } = string.Empty;
    public string CandidateFirstName { get; init; } = string.Empty;
    public string CandidateLastName { get; init; } = string.Empty;
    public Guid OfferId { get; init; } 
}