using System;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using simple_pet_clinic_api.Models.Entities;

namespace simple_pet_clinic_api.Features.Product;

public class ProductModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/product")
            .WithTags("Product")
            .RequireAuthorization(policy => 
                policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme).RequireAuthenticatedUser());
        
        group.MapGet("/", async (IMediator mediator) =>
        {
            var result = await mediator.Send(new GetAllProductQuery());
            return Results.Ok(result);
        });
        
        group.MapGet("/{productId:guid}", async (Guid productId, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetProductDetailsByIdQuery(productId));
            return result.IsSuccess ? Results.Ok(result) : Results.NotFound(result);
        });
        
        group.MapPost("/create", async (ProductCreateCommand cmd, IMediator mediator) => 
        {
            var result = await mediator.Send(cmd);
            
            if (!result.IsSuccess)
            {
                return result.StatusCode == System.Net.HttpStatusCode.Forbidden 
                    ? Results.Forbid() 
                    : Results.BadRequest(result);
            }
            
            return Results.Created($"/api/product/{((ServiceProductEntity)result.Result!).Id}", result);
        });
    }
}