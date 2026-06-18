using System;
using simple_pet_clinic_api.Models.DTOs;

namespace simple_pet_clinic_api.Models.Entities;

public class GroomingRecordEntity
{
    public Guid Id { get; set; }
    public required GroomingResultsDTO GroomingResultsDto { get; set; }
    public DateTime CheckDate { get; set; } = DateTime.UtcNow;
    
    public Guid ReservationId { get; set; }
    public ReservationEntity? Reservation { get; set; }
    
    public Guid PetId { get; set; }
    public PetEntity? Pet { get; set; }
    
    public Guid GroomerId { get; set; }
    public UserEntity? Groomer { get; set; }
}