using System;
using System.Collections.Generic;
using FluentValidation;
using MediatR;
using simple_pet_clinic_api.Data;
using simple_pet_clinic_api.Models.DTOs;
using simple_pet_clinic_api.Models.Entities;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using simple_pet_clinic_api.Models.Enums;

namespace simple_pet_clinic_api.Features.Product;

public record ProductCreateCommand(string ItemName, string Category, decimal Price, int? Stock) : IRequest<APIResponseDTO>;

public class ProductCreateValidator : AbstractValidator<ProductCreateCommand>
{
    public ProductCreateValidator()
    {
        RuleFor(x => x.ItemName).NotEmpty().WithMessage("Product Name is required!");
        RuleFor(x => x.Category).NotEmpty().WithMessage("Category is required!");
        RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0!");
        
        RuleFor(x => x.Stock).GreaterThanOrEqualTo(0).When(x => x.Stock.HasValue)
            .WithMessage("Stock cannot be negative!");
    }
}

public class ProductCreateHandler : IRequestHandler<ProductCreateCommand, APIResponseDTO>
{
    private readonly AppDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ProductCreateHandler(AppDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<APIResponseDTO> Handle(ProductCreateCommand request, CancellationToken cancellationToken)
    {
        var user = _httpContextAccessor.HttpContext?.User;
        
        if (!user!.IsInRole(UserRole.Operator.ToString()))
        {
            return new APIResponseDTO { IsSuccess = false, StatusCode = HttpStatusCode.Forbidden, ErrorMessages = new List<string> { "Only Operator can create a product!" } };
        }

        List<string> allowedCategory = new List<string>()
        {
            ServiceProductCategory.Grooming.ToString(),
            ServiceProductCategory.Medical.ToString(),
            ServiceProductCategory.PetHotel.ToString(),
            ServiceProductCategory.Product.ToString()
        };
        
        if (!allowedCategory.Contains(request.Category))
        {
            return new APIResponseDTO { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest, ErrorMessages = new List<string> { $"Invalid Category! Allowed categories are: {string.Join(", ", allowedCategory)}" } };
        }
        
        var newProduct = new ServiceProductEntity
        {
            Id = Guid.NewGuid(),
            ItemName = request.ItemName,
            Category = request.Category,
            Price = request.Price,
            Stock = request.Stock
        };

        _context.ServiceProduct.Add(newProduct);
        await _context.SaveChangesAsync(cancellationToken);

        return new APIResponseDTO 
        { 
            StatusCode = HttpStatusCode.Created, 
            Result = newProduct 
        };
    }
}