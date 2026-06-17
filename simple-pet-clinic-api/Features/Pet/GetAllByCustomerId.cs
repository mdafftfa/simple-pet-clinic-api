using System.Linq;
using MediatR;
using Microsoft.EntityFrameworkCore;
using simple_pet_clinic_api.Data;
using simple_pet_clinic_api.Models.DTOs;
using System.Net;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace simple_pet_clinic_api.Features.Pet;

public record GetAllPetQuery() : IRequest<APIResponseDTO>;

public class GetAllPetHandler : IRequestHandler<GetAllPetQuery, APIResponseDTO>
{
    private readonly AppDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetAllPetHandler(AppDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }
    
    public async Task<APIResponseDTO> Handle(GetAllPetQuery request, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (string.IsNullOrEmpty(userId))
            return new APIResponseDTO { IsSuccess = false, StatusCode = HttpStatusCode.Unauthorized };

        var pets = await _context.Pet
            .Where(p => p.CustomerId.ToString() == userId)
            .ToListAsync(cancellationToken);

        return new APIResponseDTO 
        { 
            StatusCode = HttpStatusCode.OK, 
            Result = pets 
        };
    }
}