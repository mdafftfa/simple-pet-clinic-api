using System;
using System.Text;
using System.Threading.Tasks;
using Carter;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using simple_pet_clinic_api.Data;
using simple_pet_clinic_api.Infrastructure.Behaviors;
using simple_pet_clinic_api.Infrastructure.Exceptions;
using simple_pet_clinic_api.Models.Entities;

namespace simple_pet_clinic_api;

public static class DependencyInjection
{

    public static void AddApplication(this WebApplicationBuilder builder)
    {
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                // ValidIssuer = builder.Configuration["Jwt:Issuer"],
                // ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
            };
            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    Console.WriteLine($"[JWT ERROR] Autentikasi gagal: {context.Exception.Message}");
                    return Task.CompletedTask;
                }
            };
        });
        
        builder.Services.AddAuthorization();
        
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddProblemDetails();

        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });
        
        builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
        
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });
        
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
                .UseSnakeCaseNamingConvention());
        
        builder.Services.AddCarter();
        
        builder.Services.AddIdentity<UserEntity, IdentityRole<Guid>>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();
        
        builder.Services.AddScoped<IDbInitializer, DbInitializer>();
    }
}