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

namespace simple_pet_clinic_api.Features.MedicalRecord;

public record CreateMedicalRecordCommand(
    string MedicaResults, 
    Guid ReservationId 
) : IRequest<APIResponseDTO>;

public class CreateMedicalRecordValidator : AbstractValidator<CreateMedicalRecordCommand>
{
    public CreateMedicalRecordValidator()
    {
        RuleFor(x => x.MedicaResults).NotEmpty().WithMessage("Diagnosis Result is required!");
        RuleFor(x => x.ReservationId).NotEmpty().WithMessage("ReservationId is required!");
    }
}

public class CreateMedicalRecordHandler : IRequestHandler<CreateMedicalRecordCommand, APIResponseDTO>
{
    private readonly AppDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CreateMedicalRecordHandler(AppDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }
    
    public async Task<APIResponseDTO> Handle(CreateMedicalRecordCommand request, CancellationToken ct)
    {
        var user = _httpContextAccessor.HttpContext?.User;

        if (user == null || (!user.IsInRole(UserRole.Doctor.ToString()) && !user.IsInRole(UserRole.Operator.ToString())))
        {
            return new APIResponseDTO 
            { 
                IsSuccess = false, 
                StatusCode = HttpStatusCode.Forbidden, 
                ErrorMessages = new List<string> { "Only Doctor or Operator can create Medical Record!" } 
            };
        }
        
        var doctorId = Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        Guid petId = _context.Reservation.Find(request.ReservationId)!.PetId;
        
        var medicalRecord = new MedicalRecordEntity
        {
            Id = Guid.NewGuid(),
            MedicaResults = request.MedicaResults,
            CheckDate = DateTime.UtcNow,
            ReservationId = request.ReservationId,
            PetId = petId,
            DoctorId = doctorId
        };
        
        _context.MedicalRecord.Add(medicalRecord);
        
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
            Result = medicalRecord 
        };
    }
}