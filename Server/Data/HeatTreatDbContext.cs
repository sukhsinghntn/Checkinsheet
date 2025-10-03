using Microsoft.EntityFrameworkCore;
using NDAProcesses.Shared.Models;

namespace NDAProcesses.Server.Data;

public class HeatTreatDbContext : DbContext
{
    public HeatTreatDbContext(DbContextOptions<HeatTreatDbContext> options)
        : base(options)
    {
    }

    public DbSet<HeatTreatMasterRecord> HeatTreatMasterRecords => Set<HeatTreatMasterRecord>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var entity = modelBuilder.Entity<HeatTreatMasterRecord>();
        entity.ToTable("HeatTreatMasterRecords");
        entity.HasKey(record => record.PartNumber);
        entity.Property(record => record.PartNumber)
            .HasMaxLength(128)
            .IsRequired();
    }
}
