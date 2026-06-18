using Carter;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using simple_pet_clinic_api.Models.Entities;

namespace simple_pet_clinic_api.Features.Transaction;

public class TransactionModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/transaction")
            .WithTags("Transaction")
            .RequireAuthorization(policy => 
                policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme).RequireAuthenticatedUser());
        
        group.MapPost("/create", async (TransactionCreateCommand cmd, IMediator mediator) =>
        {
            var result = await mediator.Send(cmd);
            
            if (!result.IsSuccess)
            {
                return result.StatusCode == System.Net.HttpStatusCode.Forbidden 
                    ? Results.Forbid() 
                    : Results.BadRequest(result);
            }
            
            return Results.Created($"/api/transaction/{((TransactionEntity)result.Result!).Id}", result);
        });
    }
}