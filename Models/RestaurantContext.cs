using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ProjectWorkAPI.Models;

public partial class RestaurantContext : DbContext
{
    public RestaurantContext()
    {
    }

    public RestaurantContext(DbContextOptions<RestaurantContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductPrepStation> ProductPrepStations { get; set; }

    public virtual DbSet<RevokedToken> RevokedTokens { get; set; }

    public virtual DbSet<Table> Tables { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Categori__3214EC07A213DF7D");

            entity.Property(e => e.Image)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(1000)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Orders__3214EC07BF86FCC0");

            entity.Property(e => e.CompletionDate).HasColumnType("datetime");
            entity.Property(e => e.OrderDate).HasColumnType("datetime");
            entity.Property(e => e.TableKey)
                .HasMaxLength(64)
                .IsUnicode(false);

            entity.HasOne(d => d.Product).WithMany(p => p.Orders)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Orders__ProductI__45F365D3");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Products__3214EC07CBE4531D");

            entity.Property(e => e.Description).IsUnicode(false);
            entity.Property(e => e.IdPostazionePreparazione).HasDefaultValue(1);
            entity.Property(e => e.Image)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(1000)
                .IsUnicode(false);

            entity.HasOne(d => d.IdCategoryNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.IdCategory)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Products__IdCate__4222D4EF");

            entity.HasOne(d => d.IdPostazionePreparazioneNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.IdPostazionePreparazione)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Products__IdPost__4316F928");
        });

        modelBuilder.Entity<ProductPrepStation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ProductP__3214EC071EF79186");

            entity.Property(e => e.Name)
                .HasMaxLength(1000)
                .IsUnicode(false);
        });

        modelBuilder.Entity<RevokedToken>(entity =>
        {
            entity.HasKey(e => e.Token).HasName("PK__RevokedT__1EB4F81644C27705");

            entity.Property(e => e.Token)
                .HasMaxLength(128)
                .IsUnicode(false);
            entity.Property(e => e.Expire).HasColumnType("datetime");
        });

        modelBuilder.Entity<Table>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tables__3214EC07EBC4D5D7");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.OccupancyDate).HasColumnType("datetime");
            entity.Property(e => e.TableKey)
                .HasMaxLength(64)
                .IsUnicode(false);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC0715A96C47");

            entity.Property(e => e.LastLogin).HasColumnType("datetime");
            entity.Property(e => e.LastLogout).HasColumnType("datetime");
            entity.Property(e => e.Password).IsUnicode(false);
            entity.Property(e => e.Salt)
                .HasMaxLength(32)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.SessionToken)
                .HasMaxLength(128)
                .IsUnicode(false);
            entity.Property(e => e.Username).HasMaxLength(1000);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
