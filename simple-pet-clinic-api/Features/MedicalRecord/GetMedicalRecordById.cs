using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using simple_pet_clinic_api.Data;
using simple_pet_clinic_api.Models.DTOs;
using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace simple_pet_clinic_api.Features.MedicalRecord;

public record GetMedicalRecordByPetIdQuery(Guid PetId) : IRequest<APIResponseDTO>;

public class GetMedicalRecordByPetIdValidator : AbstractValidator<GetMedicalRecordByPetIdQuery>
{
    public GetMedicalRecordByPetIdValidator()
    {
        RuleFor(x => x.PetId).NotEmpty().WithMessage("Pet ID is required!");
    }
}

public class GetMedicalRecordByPetIdHandler : IRequestHandler<GetMedicalRecordByPetIdQuery, APIResponseDTO>
{
    private readonly AppDbContext _context;

    public GetMedicalRecordByPetIdHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<APIResponseDTO> Handle(GetMedicalRecordByPetIdQuery request, CancellationToken cancellationToken)
    {
        var records = await _context.MedicalRecord
            .Where(r => r.PetId == request.PetId)
            .ToListAsync(cancellationToken);

        return new APIResponseDTO 
        { 
            StatusCode = HttpStatusCode.OK, 
            Result = records 
        };
    }
}