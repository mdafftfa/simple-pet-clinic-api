using System;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace simple_pet_clinic_api.Features.Pet;

public class PetModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/pet")
            .WithTags("Pet")
            .RequireAuthorization(policy => 
                policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme).RequireAuthenticatedUser());
        
        group.MapGet("/", async (IMediator mediator) =>
        {
            var result = await mediator.Send(new GetAllPetQuery());
            return Results.Ok(result);
        });
        
        group.MapPost("/add", async (AddPetCommand cmd, IMediator mediator) => 
        {
            var result = await mediator.Send(cmd);
            return Results.Ok(result);
        });
        
        group.MapGet("/{id:guid}", async (Guid id, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetPetProfileQuery(id));
            return result.IsSuccess ? Results.Ok(result) : Results.NotFound(result);
        });
    }
}