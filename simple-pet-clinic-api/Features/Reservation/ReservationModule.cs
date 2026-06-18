using System;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using simple_pet_clinic_api.Models.Entities;

namespace simple_pet_clinic_api.Features.Reservation;

public class ReservationModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/reservation")
            .WithTags("Reservation")
            .RequireAuthorization(policy => 
                policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme).RequireAuthenticatedUser());
        
        group.MapGet("/", async (IMediator mediator) =>
        {
            var result = await mediator.Send(new GetAllReservationQuery());
            return Results.Ok(result);
        });
        
        group.MapPost("/create", async (ReservationCreateCommand cmd, IMediator mediator) => 
        {
            var result = await mediator.Send(cmd);
            return Results.Created($"/api/reservation/{((ReservationEntity)result.Result!).Id}", result);
        });
        
        group.MapPut("/{reservationId:guid}/status", async (Guid reservationId, UpdateStatusCommand cmd, IMediator mediator) =>
        {
            var command = new UpdateStatusCommand(reservationId, cmd.NewStatus);
            var result = await mediator.Send(command);
            return result.IsSuccess ? Results.Ok(result) : Results.BadRequest(result);
        });
    }
    
}