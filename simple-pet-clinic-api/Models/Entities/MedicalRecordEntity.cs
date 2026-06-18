using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace simple_pet_clinic_api.Models.Entities;

public class MedicalRecordEntity
{
    public Guid Id { get; set; }
    
    [Column("medical_results")]
    public required string MedicaResults { get; set; }
    public DateTime CheckDate { get; set; } = DateTime.UtcNow;
    
    public Guid ReservationId { get; set; }
    [JsonIgnore]
    public ReservationEntity? Reservation { get; set; }
    
    public Guid PetId { get; set; }
    [JsonIgnore]
    public PetEntity? Pet { get; set; }
    
    [Column("doctor_id")] 
    public Guid DoctorId { get; set; }
    
    [JsonIgnore]
    public UserEntity? Doctor { get; set; }
}