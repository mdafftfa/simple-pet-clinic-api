using System;
using System.Collections.Generic;
using FluentValidation;
using MediatR;
using simple_pet_clinic_api.Data;
using simple_pet_clinic_api.Models.DTOs;
using simple_pet_clinic_api.Models.Entities;
using System.Net;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using simple_pet_clinic_api.Models.Enums;

namespace simple_pet_clinic_api.Features.Reservation;

public record ReservationCreateCommand(DateTime DateSchedule, string DestinationService, Guid PetId) : IRequest<APIResponseDTO>;

public class ReservationCreateValidator : AbstractValidator<ReservationCreateCommand>
{
    public ReservationCreateValidator()
    {
        RuleFor(x => x.DateSchedule).GreaterThan(DateTime.UtcNow).WithMessage("Date is invalid!");
        RuleFor(x => x.DestinationService).NotEmpty();
        RuleFor(x => x.PetId).NotEmpty();
    }
}

public class ReservationCreateHandler : IRequestHandler<ReservationCreateCommand, APIResponseDTO>
{
    private readonly AppDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ReservationCreateHandler(AppDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<APIResponseDTO> Handle(ReservationCreateCommand request, CancellationToken cancellationToken)
    {
        var user = _httpContextAccessor.HttpContext?.User;
        
        if (!user!.IsInRole(UserRole.Customer.ToString()))
        {
            return new APIResponseDTO { IsSuccess = false, StatusCode = HttpStatusCode.Forbidden, ErrorMessages = new List<string> { "Hanya pelanggan yang bisa membuat reservasi." } };
        }

        var userId = Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        
        var newReservation = new ReservationEntity
        {
            Id = Guid.NewGuid(),
            DateSchedule = request.DateSchedule,
            DestinationService = request.DestinationService,
            Status = ServiceStatus.Waiting.ToString(),
            CustomerId = userId,
            PetId = request.PetId
        };

        _context.Reservation.Add(newReservation);
        await _context.SaveChangesAsync(cancellationToken);

        return new APIResponseDTO { StatusCode = HttpStatusCode.Created, Result = newReservation };
    }
}