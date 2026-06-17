using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace simple_pet_clinic_api.Features.Auth;

public class AuthModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/auth/").WithTags("Auth");
        
        group.MapPost("/register", async (RegisterCommand cmd, IMediator mediator) =>
        {
            var result = await mediator.Send(cmd);
            return result.IsSuccess ? Results.Ok(result) : Results.BadRequest(result);
        });
        
        group.MapPost("/login", async (LoginCommand cmd, IMediator mediator) =>
        {
            var result = await mediator.Send(cmd);
            return result.IsSuccess ? Results.Ok(result) : Results.Json(result, statusCode: 401);
        });
    }
}