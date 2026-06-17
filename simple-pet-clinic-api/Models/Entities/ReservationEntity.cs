using System;

namespace simple_pet_clinic_api.Models.Entities;

public class ReservationEntity
{
    public Guid Id { get; set; }
    public DateTime DateSchedule { get; set; }
    public string DestinationService { get; set; }
    public string Status { get; set; }
    
    public Guid CustomerId { get; set; }
    public UserEntity? Customer { get; set; }
    
    public Guid PetId { get; set; }
    public PetEntity? Pet { get; set; }
}