using System;
using System.Collections.Generic;
using FluentValidation;
using MediatR;

using simple_pet_clinic_api.Data;
using simple_pet_clinic_api.Models.DTOs;
using simple_pet_clinic_api.Models.Entities;

using System.Net;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using simple_pet_clinic_api.Models.Enums;

namespace simple_pet_clinic_api.Features.Transaction;

public record TransactionItemDto(Guid ProductId, int Quantity);
public record TransactionCreateCommand(List<TransactionItemDto> Items) : IRequest<APIResponseDTO>;

public class TransactionCreateValidator : AbstractValidator<TransactionCreateCommand>
{
    public TransactionCreateValidator()
    {
        RuleFor(x => x.Items).NotEmpty().WithMessage("Carts cannot be empty!");
    }
}

public class TransactionCreateHandler : IRequestHandler<TransactionCreateCommand, APIResponseDTO>
{
    private readonly AppDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TransactionCreateHandler(AppDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }
    
    public async Task<APIResponseDTO> Handle(TransactionCreateCommand request, CancellationToken ct)
    {
        var userId = Guid.Parse(_httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var transactionId = Guid.NewGuid();

        var user = _httpContextAccessor.HttpContext?.User;

        if (!user!.IsInRole(UserRole.Customer.ToString()))
        {
            return new APIResponseDTO { IsSuccess = false, StatusCode = HttpStatusCode.Forbidden, ErrorMessages = new List<string> { "Only Customer can make transaction!" } };
        }
        
        var transaction = new TransactionEntity
        {
            Id = transactionId,
            CustomerId = userId,
            TransactionDate = DateTime.UtcNow,
            TotalPrice = 0,
            TransactionDetails = new List<TransactionDetailsEntity>()
        };

        decimal total = 0;
        foreach (var item in request.Items)
        {
            var product = await _context.ServiceProduct.FindAsync(item.ProductId);
            if (product == null) continue;

            var subtotal = product.Price * item.Quantity;
            total += subtotal;
            
            transaction.TransactionDetails.Add(new TransactionDetailsEntity
            {
                Id = Guid.NewGuid(),
                TransactionId = transactionId,
                ItemId = product.Id,
                Amount = item.Quantity,
                Subtotal = subtotal
            });
        }
    
        transaction.TotalPrice = total;
        _context.Transaction.Add(transaction);
        await _context.SaveChangesAsync(ct);

        return new APIResponseDTO { StatusCode = HttpStatusCode.Created, Result = transaction };
    }
}