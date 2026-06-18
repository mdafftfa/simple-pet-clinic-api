using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using simple_pet_clinic_api.Data;
using simple_pet_clinic_api.Models.DTOs;

namespace simple_pet_clinic_api.Features.GroomingRecord;

public record GetGroomingRecordByPetIdQuery(Guid PetId) : IRequest<APIResponseDTO>;

public class GetGroomingRecordByPetIdValidator : AbstractValidator<GetGroomingRecordByPetIdQuery>
{
    public GetGroomingRecordByPetIdValidator()
    {
        RuleFor(x => x.PetId).NotEmpty().WithMessage("Pet ID is required!");
    }
}

public class GetGroomingRecordByPetIdHandler : IRequestHandler<GetGroomingRecordByPetIdQuery, APIResponseDTO>
{
    private readonly AppDbContext _context;

    public GetGroomingRecordByPetIdHandler(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<APIResponseDTO> Handle(GetGroomingRecordByPetIdQuery request, CancellationToken ct)
    {
        var records = await _context.GroomingRecord
            .Where(x => x.PetId == request.PetId)
            .ToListAsync(ct);

        return new APIResponseDTO 
        { 
            StatusCode = HttpStatusCode.OK, 
            Result = records 
        };
    }
}