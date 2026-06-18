using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using simple_pet_clinic_api.Data;
using simple_pet_clinic_api.Models.DTOs;
using simple_pet_clinic_api.Models.Entities;
using simple_pet_clinic_api.Models.Enums;

namespace simple_pet_clinic_api.Features.GroomingRecord;

public record CreateGroomingRecordCommand(
    string GroomingResults, 
    Guid ReservationId
) : IRequest<APIResponseDTO>;

public class CreateGroomingRecordValidator : AbstractValidator<CreateGroomingRecordCommand>
{
    public CreateGroomingRecordValidator()
    {
        RuleFor(x => x.GroomingResults).NotEmpty().WithMessage("Grooming result is required!");
        RuleFor(x => x.ReservationId).NotEmpty().WithMessage("ReservationId is required!");
    }
}

public class CreateGroomingRecordHandler : IRequestHandler<CreateGroomingRecordCommand, APIResponseDTO>
{
    private readonly AppDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CreateGroomingRecordHandler(AppDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }
    
    public async Task<APIResponseDTO> Handle(CreateGroomingRecordCommand request, CancellationToken ct)
    {
        var user = _httpContextAccessor.HttpContext?.User;
        
        if (user == null || (!user.IsInRole(UserRole.Groomer.ToString()) && !user.IsInRole(UserRole.Operator.ToString())))
        {
            return new APIResponseDTO 
            { 
                IsSuccess = false, 
                StatusCode = HttpStatusCode.Forbidden, 
                ErrorMessages = new List<string> { "Only Groomer or Operator can create Grooming Record!" } 
            };
        }
        
        var groomerId = Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        Guid petId = _context.Reservation.Find(request.ReservationId)!.PetId;
        
        var groomingRecord = new GroomingRecordEntity
        {
            Id = Guid.NewGuid(),
            GroomingResults = request.GroomingResults,
            CheckDate = DateTime.UtcNow,
            ReservationId = request.ReservationId,
            PetId = petId,
            GroomerId = groomerId
        };

        _context.GroomingRecord.Add(groomingRecord);
        
        try
        {
            await _context.SaveChangesAsync(ct);
        }
        catch (Microsoft.EntityFrameworkCore.DbUpdateException ex)
        {
            var innerMessage = ex.InnerException?.Message ?? ex.Message;
            return new APIResponseDTO 
            { 
                IsSuccess = false, 
                StatusCode = HttpStatusCode.BadRequest, 
                ErrorMessages = new List<string> { $"Database Error: {innerMessage}" } 
            };
        }

        return new APIResponseDTO 
        { 
            StatusCode = HttpStatusCode.Created, 
            Result = groomingRecord 
        };
    }
}