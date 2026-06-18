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

namespace simple_pet_clinic_api.Features.Product;

public record GetProductDetailsByIdQuery(Guid ProductId) : IRequest<APIResponseDTO>;

public class GetProductDetailsByIdValidator : AbstractValidator<GetProductDetailsByIdQuery>
{
    public GetProductDetailsByIdValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty().WithMessage("Product ID is required!");
    }
}

public class GetProductDetailsByIdHandler : IRequestHandler<GetProductDetailsByIdQuery, APIResponseDTO>
{
    private readonly AppDbContext _context;

    public GetProductDetailsByIdHandler(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<APIResponseDTO> Handle(GetProductDetailsByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _context.ServiceProduct
            .FirstOrDefaultAsync(p => p.Id == request.ProductId, cancellationToken);

        if (product == null)
        {
            return new APIResponseDTO 
            { 
                IsSuccess = false, 
                StatusCode = HttpStatusCode.NotFound, 
                ErrorMessages = new List<string> { "Product not found!" } 
            };
        }

        return new APIResponseDTO 
        { 
            StatusCode = HttpStatusCode.OK, 
            Result = product 
        };
    }
}