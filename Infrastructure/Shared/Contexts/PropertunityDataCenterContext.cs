using _2_Domain.IAM.Models.Entities;
using _2_Domain.IAM.Models.ValueObjects;
using _2_Domain.Publication.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace _3_Data.Shared.Contexts;

public class PropertunityDataCenterContext : DbContext
{
    public PropertunityDataCenterContext()
    {
    }

    public PropertunityDataCenterContext(
        DbContextOptions<PropertunityDataCenterContext> options
    ) : base(options)
    {
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<RefreshTokenRecord> RefreshTokenRecords { get; set; }
    public DbSet<PublicationModel> Publication { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var serverVersion = new MySqlServerVersion(new Version(8, 0, 29));
            optionsBuilder.UseMySql("Server=sql10.freemysqlhosting.net,3306;Uid=sql10712330;Pwd=FEGcgiKql8;Database=sql10712330;",
                serverVersion);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>().OwnsOne(u => u._UserCredentials);
        modelBuilder.Entity<User>().OwnsOne(u => u._UserInformation);
        modelBuilder.Entity<User>().ToTable("User");
        modelBuilder.Entity<RefreshTokenRecord>().ToTable("RefreshTokenRecord");
        modelBuilder.Entity<PublicationModel>().OwnsOne(p => p._Location);
        modelBuilder.Entity<PublicationModel>().ToTable("Publication");
    }
}