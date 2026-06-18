using System;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using simple_pet_clinic_api.Models.Entities;

namespace simple_pet_clinic_api.Features.MedicalRecord;

public class MedicalRecordModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/medical-record")
            .WithTags("Medical Record")
            .RequireAuthorization(policy =>
                policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme).RequireAuthenticatedUser());
        
        group.MapGet("/pet/{petId:guid}", async (Guid petId, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetMedicalRecordByPetIdQuery(petId));
            return Results.Ok(result);
        });
        
        group.MapPost("/create", async (CreateMedicalRecordCommand cmd, IMediator mediator) =>
        {
            var result = await mediator.Send(cmd);
            
            if (!result.IsSuccess)
            {
                return result.StatusCode == System.Net.HttpStatusCode.Forbidden 
                    ? Results.Forbid() 
                    : Results.BadRequest(result);
            }
            
            return Results.Created($"/api/medical-record/{((MedicalRecordEntity)result.Result!).Id}", result);
        });
    }
}