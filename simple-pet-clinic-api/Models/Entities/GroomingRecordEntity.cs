using System;
using System.Text.Json.Serialization;

namespace simple_pet_clinic_api.Models.Entities;

public class GroomingRecordEntity
{
    public Guid Id { get; set; }
    
    public required string GroomingResults { get; set; }
    public DateTime CheckDate { get; set; } = DateTime.UtcNow;
    
    public Guid ReservationId { get; set; }
    [JsonIgnore]
    public ReservationEntity? Reservation { get; set; }
    
    public Guid PetId { get; set; }
    [JsonIgnore]
    public PetEntity? Pet { get; set; }
    
    public Guid GroomerId { get; set; }
    [JsonIgnore]
    public UserEntity? Groomer { get; set; }
}