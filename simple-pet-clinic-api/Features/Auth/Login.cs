using System;
using System.Collections.Generic;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using simple_pet_clinic_api.Models.DTOs;
using simple_pet_clinic_api.Models.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace simple_pet_clinic_api.Features.Auth;

public record LoginCommand(string Email, string Password) : IRequest<APIResponseDTO>;

public class LoginValidator : AbstractValidator<LoginCommand>
{
    public LoginValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty();
    }
}

public class LoginHandler : IRequestHandler<LoginCommand, APIResponseDTO>
{
    private readonly UserManager<UserEntity> _userManager;
    private readonly IConfiguration _configuration;

    public LoginHandler(UserManager<UserEntity> userManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }
    
    public async Task<APIResponseDTO> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var response = new APIResponseDTO();

        var user = await _userManager.FindByEmailAsync(request.Email);
        
        if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
        {
            response.IsSuccess = false;
            response.StatusCode = HttpStatusCode.Unauthorized;
            response.ErrorMessages.Add("Email or Password incorrect!");
            return response;
        }

        var roles = await _userManager.GetRolesAsync(user);

        var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
            new Claim(ClaimTypes.Name, user.FullName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        foreach (var role in roles)
        {
            authClaims.Add(new Claim(ClaimTypes.Role, role));
        }

        var jwtKey = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key is not defined");
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            expires: DateTime.UtcNow.AddHours(3),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        response.StatusCode = HttpStatusCode.OK;
        response.Result = new 
        { 
            user.Id, 
            user.FullName, 
            user.Email,
            Roles = roles,
            Token = tokenString
        };

        return response;
    }
}