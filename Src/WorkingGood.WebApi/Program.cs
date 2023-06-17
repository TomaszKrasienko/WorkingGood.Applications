 using AutoMapper;
 using Microsoft.AspNetCore.Mvc;
 using Newtonsoft.Json;
 using WorkingGood.Domain.Enums;
 using WorkingGood.Domain.Interfaces;
 using WorkingGood.Domain.Interfaces.Communication;
 using WorkingGood.Domain.Interfaces.Valida;
 using WorkingGood.Domain.Models;
 using WorkingGood.Infrastructure.Common.ConfigModels;
 using WorkingGood.Infrastructure.Communication.Entities;
 using WorkingGood.Log;
 using WorkingGood.Log.Configuration;
 using WorkingGood.WebApi.Common.Extensions.Configuration;
 using WorkingGood.WebApi.Common.Statics;
 using WorkingGood.WebApi.DTOs;
 using WorkingGood.WebApi.ViewModels;

 var builder = WebApplication.CreateBuilder(args);
 var logger = WgLogger.CreateInstance(builder.Configuration);
 try
 {
     builder.Services.AddEndpointsApiExplorer();
     builder.Services.AddSwaggerGen();
     builder.Services.AddConfiguration(builder.Configuration);
     builder.Services.UseWgLog(builder.Configuration, "WorkingGood.Applications");
     var app = builder.Build();
     app.UseSwagger(); 
     app.UseSwaggerUI(options =>
     {
         options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
         options.RoutePrefix = string.Empty;
     });
     app.UseHttpsRedirection();
     app.UseCors(ConfigurationConst.CORS_POLICY_NAME);
     app.AddCustomMiddlewares();
     
     app.MapGet("applications/getById/{id}", async ([FromRoute] Guid id,
         IApplicationRepository applicationRepository) =>
     {
         logger.Info("Handling applications/getById");
         var application = await applicationRepository.GetByIdAsync(id);
         return Results.Ok(application);
     });
     
     app.MapGet("applications/getByOfferId/{offerId}", async ([FromRoute] Guid offerId,
         IApplicationRepository applicationRepository, IMapper mapper) =>
     {
         logger.Info("Handling applications/getByOfferId");
         var applicationsList = await applicationRepository.GetAllAsync(x => x.OfferId == offerId);
         List<ApplicationVm> applicationVmList = new();
         applicationsList.ForEach(x => 
             applicationVmList.Add(mapper.Map<ApplicationVm>(x)));
         return Results.Ok(applicationVmList);
     });
     
     app.MapGet("applications/downloadDocument/{applicationId}", async ([FromRoute] Guid applicationId,
         IApplicationRepository applicationRepository, IMapper mapper) =>
     {
         logger.Info("Handling applications/downloadFile");
         Application application = await applicationRepository.GetByIdAsync(applicationId);
         string mimeType = "application/octet-stream"; 
         string fileName = "CV";
         return Results.File(application.Document, mimeType, fileName);
     });

    app.MapPost("applications/add", async ([FromBody] ApplicationDto applicationDto,
         IApplicationRepository applicationRepository,
         IOfferChecker offerChecker,
         IRabbitManager rabbitManager,
         RabbitMqConfig brokerConfig) =>
     {
         logger.Info("Handling applications/add");
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
             logger.Info("Sending confirmation message to broker");
             await rabbitManager.Send(
                 JsonConvert.SerializeObject(new ApplicationConfirmation()
                 {
                     CandidateFirstName = applicationDto.CandidateFirstName!,
                     CandidateLastName = applicationDto.CandidateLastName!,
                     CandidateEmail = applicationDto.CandidateEmail!,
                     OfferId = (Guid)applicationDto.OfferId
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
     app.MapDelete("applications/deleteAllForOffer/{offerId}", async ([FromRoute] Guid offerId,
         IApplicationRepository applicationRepository) =>
     {
         await applicationRepository.DeleteForOfferAsync(offerId);
         return Results.Ok();
     });
     app.Run();
     
 }
 catch (Exception ex)
 {
     logger.Error(ex);
 }

