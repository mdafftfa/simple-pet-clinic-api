using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace simple_pet_clinic_api.Models.Entities;

public class TransactionDetailsEntity
{
    public Guid Id { get; set; }
    public int Amount { get; set; }
    public decimal Subtotal { get; set; }
    
    public Guid TransactionId { get; set; }
    [JsonIgnore]
    public TransactionEntity? Transaction { get; set; }
    
    [Column("product_id")]
    public Guid ItemId { get; set; }
    public ServiceProductEntity? Item { get; set; }
}