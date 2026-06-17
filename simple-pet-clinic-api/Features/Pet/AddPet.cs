using System;
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

namespace simple_pet_clinic_api.Features.Pet;

public record AddPetCommand(
    string PetName, 
    string Species, 
    double Weight, 
    string DiseaseHistory
) : IRequest<APIResponseDTO>;

public class AddPetValidator : AbstractValidator<AddPetCommand>
{
    public AddPetValidator()
    {
        RuleFor(x => x.PetName).NotEmpty().WithMessage("Pet name is required!");
        RuleFor(x => x.Species).NotEmpty();
        RuleFor(x => x.Weight).GreaterThan(0).WithMessage("Weight must be greater than 0!");
    }
}

public class AddPetHandler : IRequestHandler<AddPetCommand, APIResponseDTO>
{
    private readonly AppDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AddPetHandler(AppDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }
    
    public async Task<APIResponseDTO> Handle(AddPetCommand request, CancellationToken ct)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        var newPet = new PetEntity
        {
            PetName = request.PetName,
            Species = request.Species,
            Weight = request.Weight,
            DiseaseHistory = request.DiseaseHistory,
            CustomerId = Guid.Parse(userId!)
        };

        _context.Pet.Add(newPet);
        await _context.SaveChangesAsync(ct);
        
        return new APIResponseDTO { Result = newPet, StatusCode = HttpStatusCode.Created };
    }
}