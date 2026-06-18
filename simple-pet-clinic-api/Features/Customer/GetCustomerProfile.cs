using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using simple_pet_clinic_api.Models.DTOs;
using simple_pet_clinic_api.Models.Entities;
using System.Net;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace simple_pet_clinic_api.Features.Customer;

public record GetCustomerProfileCommand() : IRequest<APIResponseDTO>;

public class GetCustomerProfileValidator : AbstractValidator<GetCustomerProfileCommand>
{
    public GetCustomerProfileValidator()
    {
    }
}

public class GetCustomerProfileHandler : IRequestHandler<GetCustomerProfileCommand, APIResponseDTO>
{
    private readonly UserManager<UserEntity> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetCustomerProfileHandler(UserManager<UserEntity> userManager, IHttpContextAccessor httpContextAccessor)
    {
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
    }
    
    public async Task<APIResponseDTO> Handle(GetCustomerProfileCommand request, CancellationToken cancellationToken)
    {
        var response = new APIResponseDTO();
        
        var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                          ?? _httpContextAccessor.HttpContext?.User?.FindFirst("sub")?.Value
                          ?? _httpContextAccessor.HttpContext?.User?.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
        
        if (string.IsNullOrEmpty(userIdClaim))
        {
            response.IsSuccess = false;
            response.StatusCode = HttpStatusCode.Unauthorized;
            response.ErrorMessages.Add("Unauthorized access! Token invalid or missing.");
            return response;
        }

        var user = await _userManager.FindByIdAsync(userIdClaim);

        if (user == null)
        {
            response.IsSuccess = false;
            response.StatusCode = HttpStatusCode.NotFound;
            response.ErrorMessages.Add("Customer profile not found.");
            return response;
        }

        var roles = await _userManager.GetRolesAsync(user);
        
        response.IsSuccess = true;
        response.StatusCode = HttpStatusCode.OK;
        response.Result = new
        {
            user.Id,
            user.FullName,
            user.Email,
            user.PhoneNumber,
            Roles = roles
        };

        return response;
    }
}