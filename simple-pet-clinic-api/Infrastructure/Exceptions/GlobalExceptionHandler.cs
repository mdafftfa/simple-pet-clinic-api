

using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using simple_pet_clinic_api.Models.DTOs;

namespace simple_pet_clinic_api.Infrastructure.Exceptions;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var response = new APIResponseDTO()
        {
            IsSuccess = false
        };

        if (exception is ValidationException validationException)
        {
            response.StatusCode = HttpStatusCode.BadRequest;
            response.ErrorMessages = validationException.Errors.Select(e => e.ErrorMessage).ToList();
            
            httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);
            return true;
        }

        response.StatusCode = HttpStatusCode.InternalServerError;
        response.ErrorMessages.Add(exception.Message);
        
        httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);
        
        return true;
    }
}