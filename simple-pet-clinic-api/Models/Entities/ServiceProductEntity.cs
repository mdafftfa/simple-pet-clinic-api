using System;

namespace simple_pet_clinic_api.Models.Entities;

public class ServiceProductEntity
{
    public Guid Id { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int? Stock { get; set; }
}