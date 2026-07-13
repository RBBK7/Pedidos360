using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pedidos360.Models;

namespace Pedidos360.Data;

public partial class Pedidos360Context : IdentityDbContext
{
    public Pedidos360Context(DbContextOptions<Pedidos360Context> options)
        : base(options)
    {}

    public virtual DbSet<Categoria> Categorias { get; set; }
    public virtual DbSet<Producto> Productos { get; set; }
    public virtual DbSet<Estado> Estados { get; set; }
    public virtual DbSet<Cliente> Clientes { get; set; }
    public virtual DbSet<CorreoCliente> CorreosClientes { get; set; }
    public virtual DbSet<TelefonoCliente> TelefonosClientes { get; set; }
    public virtual DbSet<Provincia> Provincias { get; set; }
    public virtual DbSet<Canton> Cantones { get; set; }
    public virtual DbSet<Distrito> Distritos { get; set; }
    public virtual DbSet<Direccion> Direcciones { get; set; }
    public virtual DbSet<Pedido> Pedidos { get; set; }
    public virtual DbSet<DetallePedido> DetallesPedido { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ── Categoria( CATEGORIA_PRODUCTO)
        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.ToTable("CATEGORIA_PRODUCTO");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("ID_CATEGORIA_PRODUCTO");
            entity.Property(e => e.Nombre).HasColumnName("NOMBRE").HasMaxLength(100).IsRequired();
        });

        // ── Producto( PRODUCTOS)
        modelBuilder.Entity<Producto>(entity =>
        {
            entity.ToTable("PRODUCTOS");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("ID_PRODUCTO");
            entity.Property(e => e.CategoriaId).HasColumnName("ID_CATEGORIA_PRODUCTO");
            entity.Property(e => e.Nombre).HasColumnName("NOMBRE").HasMaxLength(150).IsRequired();
            entity.Property(e => e.Precio).HasColumnName("PRECIO_UNITARIO").HasColumnType("decimal(18,2)");
            entity.Property(e => e.ImpuestoPorc).HasColumnName("IMPUESTO").HasColumnType("decimal(5,2)");
            entity.Property(e => e.Stock).HasColumnName("STOCK");
            entity.Property(e => e.Imagen).HasColumnName("IMAGEN").HasMaxLength(500).IsRequired();
            entity.Property(e => e.EstadoId).HasColumnName("ID_ESTADO");

            // FK de Categoria
            entity.HasOne(e => e.Categoria)
                  .WithMany(c => c.Productos)
                  .HasForeignKey(e => e.CategoriaId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Estado)
                  .WithMany(c => c.Productos)
                  .HasForeignKey(e => e.EstadoId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // ── Estado  ( ESTADOS)
        modelBuilder.Entity<Estado>(entity =>
        {
            entity.ToTable("ESTADOS");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("ID_ESTADO");
            entity.Property(e => e.Descripcion).HasColumnName("DESCRIPCION").HasMaxLength(100).IsRequired();
        });

        // ── Cliente(CLIENTES)
        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.ToTable("CLIENTES");
            entity.HasKey(e => e.Cedula);
            entity.Property(e => e.Cedula).HasColumnName("CEDULA").HasMaxLength(20);
            entity.Property(e => e.EstadoId).HasColumnName("ID_ESTADO");
            entity.Property(e => e.Nombre).HasColumnName("NOMBRE").HasMaxLength(100).IsRequired();
            entity.Property(e => e.ApellidoPaterno).HasColumnName("APELLIDO_PATERNO").HasMaxLength(100).IsRequired();
            entity.Property(e => e.ApellidoMaterno).HasColumnName("APELLIDO_MATERNO").HasMaxLength(100);

            entity.HasOne(e => e.Estado)
                  .WithMany(s => s.Clientes)
                  .HasForeignKey(e => e.EstadoId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // ── CorreoCliente (CORREOS_CLIENTES)
        modelBuilder.Entity<CorreoCliente>(entity =>
        {
            entity.ToTable("CORREOS_CLIENTES");
            entity.HasKey(e => new { e.Cedula, e.Correo });
            entity.Property(e => e.Cedula).HasColumnName("CEDULA");
            entity.Property(e => e.Correo).HasColumnName("CORREO").HasMaxLength(150).IsRequired();

            entity.HasOne(e => e.Cliente)
                  .WithMany(c => c.Correos)
                  .HasForeignKey(e => e.Cedula)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // ── TelefonoCliente(TELEFONOS_CLIENTES)
        modelBuilder.Entity<TelefonoCliente>(entity =>
        {
            entity.ToTable("TELEFONOS_CLIENTES");
            entity.HasKey(e => new { e.Cedula, e.Telefono });
            entity.Property(e => e.Cedula).HasColumnName("CEDULA");
            entity.Property(e => e.Telefono).HasColumnName("TELEFONO").HasMaxLength(20).IsRequired();

            entity.HasOne(e => e.Cliente)
                  .WithMany(c => c.Telefonos)
                  .HasForeignKey(e => e.Cedula)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // ── Provincia / Canton / Distrito (PROVINCIA, CANTON, DISTRITO)
        modelBuilder.Entity<Provincia>(entity =>
        {
            entity.ToTable("PROVINCIA");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("ID_PROVINCIA");
            entity.Property(e => e.NombreProvincia).HasColumnName("NOMBRE_PROVINCIA").HasMaxLength(100).IsRequired();
        });

        modelBuilder.Entity<Canton>(entity =>
        {
            entity.ToTable("CANTON");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("ID_CANTON");
            entity.Property(e => e.ProvinciaId).HasColumnName("ID_PROVINCIA");
            entity.Property(e => e.NombreCanton).HasColumnName("NOMBRE_CANTON").HasMaxLength(100).IsRequired();

            entity.HasOne(e => e.Provincia)
                  .WithMany(p => p.Cantones)
                  .HasForeignKey(e => e.ProvinciaId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Distrito>(entity =>
        {
            entity.ToTable("DISTRITO");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("ID_DISTRITO");
            entity.Property(e => e.CantonId).HasColumnName("ID_CANTON");
            entity.Property(e => e.NombreDistrito).HasColumnName("NOMBRE_DISTRITO").HasMaxLength(100).IsRequired();

            entity.HasOne(e => e.Canton)
                  .WithMany(c => c.Distritos)
                  .HasForeignKey(e => e.CantonId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // ── Direccion  ( DIRECCIONES)
        modelBuilder.Entity<Direccion>(entity =>
        {
            entity.ToTable("DIRECCIONES");
            entity.HasKey(e => e.Cedula);
            entity.Ignore(e => e.Id); 
            entity.Property(e => e.Cedula).HasColumnName("CEDULA");
            entity.Property(e => e.ProvinciaId).HasColumnName("ID_PROVINCIA");
            entity.Property(e => e.CantonId).HasColumnName("ID_CANTON");
            entity.Property(e => e.DistritoId).HasColumnName("ID_DISTRITO");
            entity.Property(e => e.OtrasSenas).HasColumnName("OTRAS_SENAS").HasMaxLength(300).IsRequired();

            entity.HasOne(e => e.Cliente)
                  .WithMany(c => c.Direcciones)
                  .HasForeignKey(e => e.Cedula)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Provincia).WithMany().HasForeignKey(e => e.ProvinciaId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.Canton).WithMany().HasForeignKey(e => e.CantonId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.Distrito).WithMany().HasForeignKey(e => e.DistritoId).OnDelete(DeleteBehavior.Restrict);
        });

        // ── Pedido( PEDIDOS)
        modelBuilder.Entity<Pedido>(entity =>
        {
            entity.ToTable("PEDIDOS");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("ID_PEDIDO");
            entity.Property(e => e.ClienteId).HasColumnName("ID_CLIENTE");
            entity.Property(e => e.EstadoId).HasColumnName("ID_ESTADO");
            entity.Property(e => e.RolId).HasColumnName("ID_ROL");
            entity.Property(e => e.Fecha).HasColumnName("FECHA");
            entity.Property(e => e.Subtotal).HasColumnName("SUBTOTAL").HasColumnType("decimal(18,2)");
            entity.Property(e => e.Impuesto).HasColumnName("IMPUESTO").HasColumnType("decimal(18,2)");
            entity.Property(e => e.Descuento).HasColumnName("DESCUENTO").HasColumnType("decimal(18,2)");
            entity.Property(e => e.Total).HasColumnName("TOTAL").HasColumnType("decimal(18,2)");

            entity.HasOne(e => e.Cliente)
                  .WithMany(c => c.Pedidos)
                  .HasForeignKey(e => e.ClienteId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Estado)
                  .WithMany(s => s.Pedidos)
                  .HasForeignKey(e => e.EstadoId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // ── DetallePedido  (DETALLE_PEDIDO)
        modelBuilder.Entity<DetallePedido>(entity =>
        {
            entity.ToTable("DETALLE_PEDIDO");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("ID_DETALLE_PEDIDO");
            entity.Property(e => e.PedidoId).HasColumnName("ID_PEDIDO");
            entity.Property(e => e.ProductoId).HasColumnName("ID_PRODUCTO");
            entity.Property(e => e.Cantidad).HasColumnName("CANTIDAD");
            entity.Property(e => e.PrecioUnitario).HasColumnName("PRECIO_UNITARIO").HasColumnType("decimal(18,2)");
            entity.Property(e => e.DescuentoPorc).HasColumnName("DESCUENTO").HasColumnType("decimal(5,2)");
            entity.Property(e => e.ImpuestoPorc).HasColumnName("IMPUESTO_PORCENTAJE").HasColumnType("decimal(5,2)");
            entity.Property(e => e.TotalLinea).HasColumnName("TOTAL_LINEA").HasColumnType("decimal(18,2)");

            entity.HasOne(e => e.Pedido)
                  .WithMany(p => p.Detalles)
                  .HasForeignKey(e => e.PedidoId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Producto)
                  .WithMany(p => p.DetallesPedido)
                  .HasForeignKey(e => e.ProductoId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}