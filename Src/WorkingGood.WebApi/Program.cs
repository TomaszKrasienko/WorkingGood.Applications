 using System.Text;
 using Amazon.Auth.AccessControlPolicy;
 using Microsoft.AspNetCore.Mvc;
using WorkingGood.Domain.Interfaces;
 using WorkingGood.Domain.Interfaces.Valida;
 using WorkingGood.Domain.Models;
using WorkingGood.Infrastructure.Common.Extensions;
using WorkingGood.WebApi.DTOs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureInfrastructureServices(builder.Configuration);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("Api/Applications/Add",async ([FromBody]ApplicationDto applicationDto, 
    IApplicationRepository applicationRepository,
    IOfferChecker offerChecker) =>
{
    if (await offerChecker.CheckOfferStatus((Guid) applicationDto.OfferId))
    {
        //SGVsbG8=
        byte[] byteDocument = Convert.FromBase64String(applicationDto.Document!);
        Application application = new(
            applicationDto.CandidateFirstName!,
            applicationDto.CandidateLastName!,
            applicationDto.CandidateEmail!,
            applicationDto.Description!,
            byteDocument
        );
        await applicationRepository.AddAsync(application);
    }
    else
    {
        Results.BadRequest("Offer status is not valid");
    }
});
app.Run();

