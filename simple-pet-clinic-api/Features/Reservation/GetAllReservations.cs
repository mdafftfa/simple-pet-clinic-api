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
using simple_pet_clinic_api.Models.Enums;

namespace simple_pet_clinic_api.Features.Reservation;

public record GetAllReservationQuery() : IRequest<APIResponseDTO>;

public class GetAllReservationHandler : IRequestHandler<GetAllReservationQuery, APIResponseDTO>
{
    private readonly AppDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetAllReservationHandler(AppDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }
    
    public async Task<APIResponseDTO> Handle(GetAllReservationQuery request, CancellationToken cancellationToken)
    {
        var user = _httpContextAccessor.HttpContext?.User;
        var userId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var roles = user?.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();
        
        var query = _context.Reservation.AsQueryable();
        
        if (roles!.Contains(UserRole.Doctor.ToString()))
        {
            query = query.Where(r => r.DestinationService == "Medis");
        }
        else if (roles!.Contains(UserRole.Groomer.ToString()))
        {
            query = query.Where(r => r.DestinationService == "Grooming");
        }
        else if (roles!.Contains(UserRole.Customer.ToString()))
        {
            query = query.Where(r => r.CustomerId.ToString() == userId);
        }
        else 
        {
        }

        var result = await query.ToListAsync(cancellationToken);
        
        return new APIResponseDTO { StatusCode = HttpStatusCode.OK, Result = result };
    }
}