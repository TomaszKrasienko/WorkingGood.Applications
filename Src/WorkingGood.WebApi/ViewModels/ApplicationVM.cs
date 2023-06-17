namespace WorkingGood.WebApi.ViewModels;

public record ApplicationVm
{
    public Guid Id { get; set; }
    public Guid OfferId { get; set; }
    public string CandidateFirstName { get; init; } = string.Empty;    
    public string CandidateLastName { get; init; } = string.Empty;
    public string CandidateEmailAddress { get; init; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Document { get; set; } = string.Empty;
}