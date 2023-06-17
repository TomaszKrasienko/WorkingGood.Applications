using AutoMapper;
using FluentAssertions;
using WorkingGood.Domain.Models;
using WorkingGood.WebApi.Mappers;
using WorkingGood.WebApi.ViewModels;

namespace WorkingGood.WebApi.Tests.Mappers;

public class ApplicationMapperProfileTests
{
    [Fact]
    public void ApplicationVMMAp_ForApplication_ShouldReturnApplicationVM()
    {
        //Arrange
        string base64Document = "SGVsbG8=";
        byte[] byteDocument = Convert.FromBase64String(base64Document);
        Application application = new Application(
            "TestFirstName",
            "TestLastName",
            "Test@Test.pl",
            "DescriptionTest",
            byteDocument,
            Guid.NewGuid());
        var config = new MapperConfiguration(cfg => cfg.AddProfile<ApplicationMapperProfile>());
        var mapper = config.CreateMapper();
        //Act
        ApplicationVm applicationVm = mapper.Map<ApplicationVm>(application);
        //Assert
        applicationVm.Should().NotBeNull();
        applicationVm.Id.Should().Be(application.Id);
        applicationVm.OfferId.Should().Be(application.OfferId);
        applicationVm.CandidateFirstName.Should().Be(application.ApplicationCandidate.FirstName);
        applicationVm.CandidateLastName.Should().Be(application.ApplicationCandidate.LastName);
        applicationVm.CandidateEmailAddress.Should().Be(application.ApplicationCandidate.EmailAddress);
        applicationVm.Description.Should().Be(application.Description);
    }
}