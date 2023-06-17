using AutoMapper;
using WorkingGood.Domain.Models;
using WorkingGood.WebApi.ViewModels;

namespace WorkingGood.WebApi.Mappers;

public class ApplicationMapperProfile : Profile
{
    public ApplicationMapperProfile()
    {
        CreateMap<Application, ApplicationVm>()
            .ForMember(dest => dest.Id,
                opt => opt.MapFrom(
                    src => src.Id))
            .ForMember(dest => dest.OfferId,
                opt => opt.MapFrom(
                    src => src.OfferId))
            .ForMember(dest => dest.CandidateFirstName,
                opt => opt.MapFrom(
                    src => src.ApplicationCandidate.FirstName))
            .ForMember(dest => dest.CandidateLastName,
                opt => opt.MapFrom(
                    src => src.ApplicationCandidate.LastName))
            .ForMember(dest => dest.CandidateEmailAddress,
                opt => opt.MapFrom(
                    src => src.ApplicationCandidate.EmailAddress))
            .ForMember(dest => dest.Description,
                opt => opt.MapFrom(
                    src => src.Description));
    }
}