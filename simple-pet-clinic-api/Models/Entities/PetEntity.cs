using System;

namespace simple_pet_clinic_api.Models.Entities;

public class PetEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string PetName { get; set; } = string.Empty;
    public string Species { get; set; } = string.Empty;
    public double Weight { get; set; }
    public string? DiseaseHistory { get; set; }
    
    public Guid CustomerId { get; set; }
    public UserEntity? Customer { get; set; }
    
}