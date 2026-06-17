using System;
using Microsoft.AspNetCore.Identity;

namespace simple_pet_clinic_api.Models.Entities;

public class UserEntity : IdentityUser<Guid>
{
    public string FullName { get; set; } = string.Empty;
}