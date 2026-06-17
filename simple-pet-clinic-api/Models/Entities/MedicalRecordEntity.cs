using System;

namespace simple_pet_clinic_api.Models.Entities;

public class MedicalRecordEntity
{
    public Guid Id { get; set; }
    public string DiagnosisResults { get; set; } = string.Empty;
    public DateTime CheckDate { get; set; } = DateTime.UtcNow;
    
    public Guid ReservationId { get; set; }
    public ReservationEntity? Reservation { get; set; }
    
    public Guid PetId { get; set; }
    public PetEntity? Pet { get; set; }
    
    public Guid DoctorId { get; set; }
    public UserEntity? Doctor { get; set; }
}