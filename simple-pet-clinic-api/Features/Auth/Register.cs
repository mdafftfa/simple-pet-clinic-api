using System.Linq;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using simple_pet_clinic_api.Models.DTOs;
using simple_pet_clinic_api.Models.Entities;
using simple_pet_clinic_api.Models.Enums;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace simple_pet_clinic_api.Features.Auth;

public record RegisterCommand(string FullName, string Email, string Password, string Role) : IRequest<APIResponseDTO>;

public class RegisterValidator : AbstractValidator<RegisterCommand>
{
    public RegisterValidator()
    {
        RuleFor(x => x.FullName).NotEmpty().WithMessage("FullName is required.");
        RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Email is invalid!");
        RuleFor(x => x.Password).NotEmpty().MinimumLength(6).WithMessage("Password must greather than 6 characters!");
        RuleFor(x => x.Role).IsEnumName(typeof(UserRole), caseSensitive: false)
            .WithMessage("Invalid Role (Choose: Operator, Cashier, Doctor, Groomer, Customer).");
    }
}

public class RegisterHandler : IRequestHandler<RegisterCommand, APIResponseDTO>
{
    private readonly UserManager<UserEntity> _userManager;

    public RegisterHandler(UserManager<UserEntity> userManager)
    {
        _userManager = userManager;
    }
    
    
    public async Task<APIResponseDTO> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var response = new APIResponseDTO();

        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser != null)
        {
            response.IsSuccess = false;
            response.StatusCode = HttpStatusCode.BadRequest;
            response.ErrorMessages.Add("Email already exists!");
            return response;
        }

        var newUser = new UserEntity
        {
            UserName = request.Email,
            Email = request.Email,
            FullName = request.FullName
        };

        var result = await _userManager.CreateAsync(newUser, request.Password);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(newUser, request.Role);
            
            response.StatusCode = HttpStatusCode.OK;
            response.Result = new { newUser.Id, newUser.FullName, newUser.Email, request.Role };
        }
        else
        {
            response.IsSuccess = false;
            response.StatusCode = HttpStatusCode.BadRequest;
            response.ErrorMessages = result.Errors.Select(e => e.Description).ToList();
        }

        return response;
    }
}