using System;

namespace simple_pet_clinic_api.Models.Entities;

public class TransactionDetailsEntity
{
    public Guid Id { get; set; }
    public int Amount { get; set; }
    public decimal Subtotal { get; set; }
    
    public Guid TransactionId { get; set; }
    public TransactionEntity? Transaction { get; set; }

    public Guid ItemId { get; set; }
    public ServiceProductEntity? Item { get; set; }
}