using Microsoft.EntityFrameworkCore;
using Pedidos360.Models;

namespace Pedidos360.Data;

public partial class Pedidos360Context : DbContext
{
    public Pedidos360Context(DbContextOptions<Pedidos360Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Categoria> Categorias { get; set; }
    public virtual DbSet<Producto> Productos { get; set; }
    public virtual DbSet<Cliente> Clientes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // ── Categoria 
        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nombre).HasMaxLength(100).IsRequired();
        });

        // ── Producto
        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nombre).HasMaxLength(150).IsRequired();
            entity.Property(e => e.Precio).HasColumnType("decimal(18,2)");
            entity.Property(e => e.ImpuestoPorc).HasColumnType("decimal(5,2)");
            entity.Property(e => e.ImagenUrl).HasMaxLength(500).IsRequired();

            // FK → Categoria
            entity.HasOne(e => e.Categoria)
                  .WithMany(c => c.Productos)
                  .HasForeignKey(e => e.CategoriaId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // ── Cliente 
        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nombre).HasMaxLength(150).IsRequired();
            entity.Property(e => e.Cedula).HasMaxLength(20).IsRequired();
            entity.Property(e => e.Correo).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Telefono).HasMaxLength(20).IsRequired();
            entity.Property(e => e.Direccion).HasMaxLength(250).IsRequired();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
