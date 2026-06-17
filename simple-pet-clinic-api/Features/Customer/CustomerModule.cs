using System;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace simple_pet_clinic_api.Features.Customer;

public class CustomerModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/customer")
            .WithTags("Customer")
            .RequireAuthorization(policy => 
                policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme).RequireAuthenticatedUser());
        
        group.MapGet("/me", async (IMediator mediator) =>
        {
            try
            {
                var result = await mediator.Send(new GetCustomerProfileCommand());
                return result.IsSuccess ? Results.Ok(result) : Results.BadRequest(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        });
    }
}