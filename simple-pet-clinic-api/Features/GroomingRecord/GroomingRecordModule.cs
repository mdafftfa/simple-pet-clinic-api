using System;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using simple_pet_clinic_api.Models.Entities;

namespace simple_pet_clinic_api.Features.GroomingRecord;

public class GroomingRecordModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/grooming-record")
            .WithTags("Grooming Record")
            .RequireAuthorization(policy =>
                policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme).RequireAuthenticatedUser());
        
        group.MapGet("/pet/{petId:guid}", async (Guid petId, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetGroomingRecordByPetIdQuery(petId));
            return Results.Ok(result);
        });
        
        group.MapPost("/create", async (CreateGroomingRecordCommand cmd, IMediator mediator) =>
        {
            var result = await mediator.Send(cmd);
            
            if (!result.IsSuccess)
            {
                return result.StatusCode == System.Net.HttpStatusCode.Forbidden 
                    ? Results.Forbid() 
                    : Results.BadRequest(result);
            }
            
            return Results.Created($"/api/grooming-record/{((GroomingRecordEntity)result.Result!).Id}", result);
        });
    }
}