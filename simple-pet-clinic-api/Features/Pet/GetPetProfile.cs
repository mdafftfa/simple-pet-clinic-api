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

namespace simple_pet_clinic_api.Features.Pet;

public record GetPetProfileQuery(Guid Id) : IRequest<APIResponseDTO>;

public class GetPetProfileValidator : AbstractValidator<GetPetProfileQuery>
{
    public GetPetProfileValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Pet ID is required!");
    }
}

public class GetPetProfileHandler : IRequestHandler<GetPetProfileQuery, APIResponseDTO>
{
    private readonly AppDbContext _context;

    public GetPetProfileHandler(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<APIResponseDTO> Handle(GetPetProfileQuery request, CancellationToken cancellationToken)
    {
        var pet = await _context.Pet.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (pet == null)
        {
            return new APIResponseDTO 
            { 
                IsSuccess = false, 
                StatusCode = HttpStatusCode.NotFound, 
                ErrorMessages = new List<string> { "Pet not found!" } 
            };
        }

        return new APIResponseDTO 
        { 
            StatusCode = HttpStatusCode.OK, 
            Result = pet 
        };
    }
}