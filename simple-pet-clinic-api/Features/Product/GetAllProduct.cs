using MediatR;
using Microsoft.EntityFrameworkCore;
using simple_pet_clinic_api.Data;
using simple_pet_clinic_api.Models.DTOs;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace simple_pet_clinic_api.Features.Product;

public record GetAllProductQuery() : IRequest<APIResponseDTO>;

public class GetAllProductHandler : IRequestHandler<GetAllProductQuery, APIResponseDTO>
{
    private readonly AppDbContext _context;

    public GetAllProductHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<APIResponseDTO> Handle(GetAllProductQuery request, CancellationToken cancellationToken)
    {
        var products = await _context.ServiceProduct.ToListAsync(cancellationToken);

        return new APIResponseDTO 
        { 
            StatusCode = HttpStatusCode.OK, 
            Result = products 
        };
    }
}