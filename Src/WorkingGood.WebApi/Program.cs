 using Microsoft.AspNetCore.Mvc;
 using Newtonsoft.Json;
 using NLog;
 using NLog.Web;
 using WorkingGood.Domain.Enums;
 using WorkingGood.Domain.Interfaces;
 using WorkingGood.Domain.Interfaces.Communication;
 using WorkingGood.Domain.Interfaces.Valida;
 using WorkingGood.Domain.Models;
 using WorkingGood.Infrastructure.Common.ConfigModels;
 using WorkingGood.Infrastructure.Common.Extensions;
 using WorkingGood.Infrastructure.Communication.Entities;
 using WorkingGood.WebApi.Common.Extensions.Configuration;
 using WorkingGood.WebApi.Common.Statics;
 using WorkingGood.WebApi.DTOs;
 
 Logger logger = LogManager.GetLogger("RmqTarget");
 try
 {
     var builder = WebApplication.CreateBuilder(args);
     builder.Services.AddEndpointsApiExplorer();
     builder.Services.AddSwaggerGen();
     builder.Services.AddConfiguration(builder.Configuration);
     builder.Host.UseNLog();
     var app = builder.Build();
     app.UseSwagger(); 
     app.UseSwaggerUI(options =>
     {
         options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
         options.RoutePrefix = string.Empty;
     });
     app.UseHttpsRedirection();
     app.UseCors(ConfigurationConst.CORS_POLICY_NAME);
     app.MapPost("api/applications/add", async ([FromBody] ApplicationDto applicationDto,
         IApplicationRepository applicationRepository,
         IOfferChecker offerChecker,
         IRabbitManager rabbitManager,
         BrokerConfig brokerConfig) =>
     {
         if (await offerChecker.CheckOfferStatus((Guid) applicationDto.OfferId!))
         {
             //SGVsbG8=
             byte[] byteDocument = Convert.FromBase64String(applicationDto.Document!);
             Application application = new(
                 applicationDto.CandidateFirstName!,
                 applicationDto.CandidateLastName!,
                 applicationDto.CandidateEmail!,
                 applicationDto.Description!,
                 byteDocument,
                 (Guid) applicationDto.OfferId
             );
             await applicationRepository.AddAsync(application);
             RabbitMqRoutesConfig routingConfig = brokerConfig.SendingRoutes.SingleOrDefault(x =>
                 x.Destination == MessageDestinations.ApplicationConfirmation.ToString())!;
             await rabbitManager.Send(
                 JsonConvert.SerializeObject(new ApplicationConfirmation()
                 {
                     Email = applicationDto.CandidateEmail!
                 }),
                 routingConfig!.Exchange,
                 routingConfig.RoutingKey
             );
             return Results.Ok(application);
         }
         else
         {
             return Results.BadRequest("Offer status is not valid");
         }
     });
     app.MapGet("api/applications/getById/{id}", async ([FromQuery] Guid id,
         IApplicationRepository applicationRepository) =>
     {
         var application = await applicationRepository.GetByIdAsync(id);
         return Results.Ok(application);
     });
     app.MapGet("api/applications/getByOfferId/{offerId}", async ([FromQuery] Guid offerId,
         IApplicationRepository applicationRepository) =>
     {
         var applicationsList = await applicationRepository.GetAllAsync(x => x.OfferId == offerId);
         return Results.Ok(applicationsList);
     });
     app.Run();
 }
 catch (Exception ex)
 {
     logger.Error(ex);
 }
 finally
 {
     
 }

