using System;
using System.Collections.Generic;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using simple_pet_clinic_api.Data;
using simple_pet_clinic_api.Models.DTOs;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

using simple_pet_clinic_api.Models.Enums;

namespace simple_pet_clinic_api.Features.Reservation;

public record UpdateStatusCommand(Guid ReservationId, string NewStatus) : IRequest<APIResponseDTO>
{
    public UpdateStatusCommand() : this( Guid.Empty, string.Empty) { }
}

public class UpdateStatusValidator : AbstractValidator<UpdateStatusCommand>
{
    public UpdateStatusValidator()
    {
        RuleFor(x => x.ReservationId).NotEmpty();
        RuleFor(x => x.NewStatus).NotEmpty().WithMessage("Status is Required!");
    }
}

public class UpdateStatusHandler : IRequestHandler<UpdateStatusCommand, APIResponseDTO>
{
    private readonly AppDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UpdateStatusHandler(AppDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }
    
    public async Task<APIResponseDTO> Handle(UpdateStatusCommand request, CancellationToken cancellationToken)
    {
        var user = _httpContextAccessor.HttpContext?.User;
        
        if (!user!.IsInRole(UserRole.Operator.ToString()) && !user!.IsInRole(UserRole.Cashier.ToString()) && !user!.IsInRole(UserRole.Groomer.ToString()) && !user!.IsInRole(UserRole.Doctor.ToString()))
        {
            return new APIResponseDTO { IsSuccess = false, StatusCode = HttpStatusCode.Forbidden, ErrorMessages = new List<string> { "Access Restricted!" } };
        }

        var reservation = await _context.Reservation.FirstOrDefaultAsync(r => r.Id == request.ReservationId, cancellationToken);
        if (reservation == null)
            return new APIResponseDTO { IsSuccess = false, StatusCode = HttpStatusCode.NotFound, ErrorMessages = new List<string> { "Reservation not found!" } };
        
        reservation.Status = request.NewStatus;
        await _context.SaveChangesAsync(cancellationToken);

        return new APIResponseDTO { StatusCode = HttpStatusCode.OK, Result = reservation };
    }
}