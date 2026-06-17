using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using simple_pet_clinic_api.Models.Entities;

namespace simple_pet_clinic_api.Data;

public class AppDbContext : IdentityDbContext<UserEntity, IdentityRole<Guid>, Guid>
{
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    public DbSet<UserEntity> User { get; set; }
    public DbSet<PetEntity> Pet { get; set; }
    public DbSet<ServiceProductEntity> ServiceProduct { get; set; }
    public DbSet<ReservationEntity> Reservation { get; set; }
    public DbSet<TransactionEntity> Transaction { get; set; }
    public DbSet<TransactionDetailsEntity> TransactionDetails { get; set; }
    public DbSet<MedicalRecordEntity> MedicalRecord { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.Entity<ServiceProductEntity>()
            .Property(p => p.Price)
            .HasColumnType("decimal(18,2)");

        builder.Entity<TransactionEntity>()
            .Property(t => t.TotalPrice)
            .HasColumnType("decimal(18,2)");

        builder.Entity<TransactionDetailsEntity>()
            .Property(d => d.Subtotal)
            .HasColumnType("decimal(18,2)");
    }
}