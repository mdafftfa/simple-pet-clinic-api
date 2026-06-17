using System;
using System.Collections.Generic;

namespace simple_pet_clinic_api.Models.Entities;

public class TransactionEntity
{
    public Guid Id { get; set; }
    public decimal TotalPrice { get; set; }
    public DateTime PayTime { get; set; }
    
    public Guid? ReservationId { get; set; }
    public ReservationEntity? Reservation { get; set; }

    public Guid? EmployeeId { get; set; }
    public UserEntity? Employee { get; set; }
    
    public ICollection<TransactionDetailsEntity>? TransactionDetails { get; set; }
}