using System.Collections.Generic;
using System.Net;

namespace simple_pet_clinic_api.Models.DTOs;

public class APIResponseDTO
{

    public bool IsSuccess { get; set; } = true;
    public object? Result { get; set; }
    public HttpStatusCode StatusCode { get; set; }
    public List<string> ErrorMessages { get; set; } = [];
    
    public APIResponseDTO() { }
    
}