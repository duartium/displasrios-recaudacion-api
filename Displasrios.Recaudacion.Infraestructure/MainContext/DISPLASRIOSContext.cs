using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Displasrios.Recaudacion.Infraestructure.MainContext
{
    public partial class DISPLASRIOSContext : DbContext
    {
        public DISPLASRIOSContext()
        {
        }

        public DISPLASRIOSContext(DbContextOptions<DISPLASRIOSContext> options)
            : base(options)
        {
        }

        public virtual DbSet<BajaProductos> BajaProductos { get; set; }
        public virtual DbSet<Catalogos> Catalogos { get; set; }
        public virtual DbSet<Clientes> Clientes { get; set; }
        public virtual DbSet<Empleados> Empleados { get; set; }
        public virtual DbSet<Entradas> Entradas { get; set; }
        public virtual DbSet<EntradasDetalle> EntradasDetalle { get; set; }
        public virtual DbSet<EstadoCuenta> EstadoCuenta { get; set; }
        public virtual DbSet<Factura> Factura { get; set; }
        public virtual DbSet<FacturaDetalle> FacturaDetalle { get; set; }
        public virtual DbSet<Ingresos> Ingresos { get; set; }
        public virtual DbSet<ItemCatalogo> ItemCatalogo { get; set; }
        public virtual DbSet<MovimientosCaja> MovimientosCaja { get; set; }
        public virtual DbSet<NotaCredito> NotaCredito { get; set; }
        public virtual DbSet<NotaCreditoDetalle> NotaCreditoDetalle { get; set; }
        public virtual DbSet<Pagos> Pagos { get; set; }
        public virtual DbSet<Parametros> Parametros { get; set; }
        public virtual DbSet<PasswordReset> PasswordReset { get; set; }
        public virtual DbSet<Perfiles> Perfiles { get; set; }
        public virtual DbSet<Productos> Productos { get; set; }
        public virtual DbSet<Proveedores> Proveedores { get; set; }
        public virtual DbSet<Secuenciales> Secuenciales { get; set; }
        public virtual DbSet<Usuarios> Usuarios { get; set; }
        public virtual DbSet<Visitas> Visitas { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=SQL8004.site4now.net; Initial Catalog=db_a89177_displasrios; user id=db_a89177_displasrios_admin; password=jw2V9ir6miaPSqP");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BajaProductos>(entity =>
            {
                entity.HasKey(e => e.IdBaja);

                entity.ToTable("BAJA_PRODUCTOS");

                entity.Property(e => e.IdBaja).HasColumnName("id_baja");

                entity.Property(e => e.Cantidad).HasColumnName("cantidad");

                entity.Property(e => e.Fecha)
                    .HasColumnName("fecha")
                    .HasColumnType("datetime");

                entity.Property(e => e.MotivoId).HasColumnName("motivo_id");

                entity.Property(e => e.OtroMotivo)
                    .HasColumnName("otro_motivo")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.ProductoId).HasColumnName("producto_id");

                entity.Property(e => e.Usuario)
                    .IsRequired()
                    .HasColumnName("usuario")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.Producto)
                    .WithMany(p => p.BajaProductos)
                    .HasForeignKey(d => d.ProductoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BAJA_PRODUCTOS_PRODUCTOS");
            });

            modelBuilder.Entity<Catalogos>(entity =>
            {
                entity.HasKey(e => e.IdCatalogo);

                entity.ToTable("CATALOGOS");

                entity.Property(e => e.IdCatalogo).HasColumnName("id_catalogo");

                entity.Property(e => e.CreadoEn)
                    .HasColumnName("creado_en")
                    .HasColumnType("datetime");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasColumnName("descripcion")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EliminadoEn)
                    .HasColumnName("eliminado_en")
                    .HasColumnType("datetime");

                entity.Property(e => e.Estado).HasColumnName("estado");

                entity.Property(e => e.ModificadoEn)
                    .HasColumnName("modificado_en")
                    .HasColumnType("datetime");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnName("nombre")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Tipo).HasColumnName("tipo");

                entity.Property(e => e.UsuarioCrea)
                    .IsRequired()
                    .HasColumnName("usuario_crea")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioElim)
                    .HasColumnName("usuario_elim")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioMod)
                    .HasColumnName("usuario_mod")
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Clientes>(entity =>
            {
                entity.HasKey(e => e.IdCliente);

                entity.ToTable("CLIENTES");

                entity.Property(e => e.IdCliente).HasColumnName("id_cliente");

                entity.Property(e => e.Apellidos)
                    .IsRequired()
                    .HasColumnName("apellidos")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CreadoEn)
                    .HasColumnName("creado_en")
                    .HasColumnType("datetime");

                entity.Property(e => e.Direccion)
                    .HasColumnName("direccion")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EliminadoEn)
                    .HasColumnName("eliminado_en")
                    .HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Estado).HasColumnName("estado");

                entity.Property(e => e.Identificacion)
                    .IsRequired()
                    .HasColumnName("identificacion")
                    .HasMaxLength(13)
                    .IsUnicode(false);

                entity.Property(e => e.ModificadoEn)
                    .HasColumnName("modificado_en")
                    .HasColumnType("datetime");

                entity.Property(e => e.Nombres)
                    .IsRequired()
                    .HasColumnName("nombres")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Telefono)
                    .HasColumnName("telefono")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.TipoCliente).HasColumnName("tipo_cliente");

                entity.Property(e => e.TipoIdentificacion)
                    .IsRequired()
                    .HasColumnName("tipo_identificacion")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.UsuarioCrea)
                    .IsRequired()
                    .HasColumnName("usuario_crea")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioElim)
                    .HasColumnName("usuario_elim")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioMod)
                    .HasColumnName("usuario_mod")
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Empleados>(entity =>
            {
                entity.HasKey(e => e.IdEmpleado);

                entity.ToTable("EMPLEADOS");

                entity.Property(e => e.IdEmpleado).HasColumnName("id_empleado");

                entity.Property(e => e.Apellidos)
                    .IsRequired()
                    .HasColumnName("apellidos")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.CreadoEn)
                    .HasColumnName("creado_en")
                    .HasColumnType("datetime");

                entity.Property(e => e.EliminadoEn)
                    .HasColumnName("eliminado_en")
                    .HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Estado).HasColumnName("estado");

                entity.Property(e => e.Identificacion)
                    .IsRequired()
                    .HasColumnName("identificacion")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ModificadoEn)
                    .HasColumnName("modificado_en")
                    .HasColumnType("datetime");

                entity.Property(e => e.Nombres)
                    .IsRequired()
                    .HasColumnName("nombres")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TipoEmpleado).HasColumnName("tipo_empleado");

                entity.Property(e => e.UsuarioCrea)
                    .IsRequired()
                    .HasColumnName("usuario_crea")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioElim)
                    .HasColumnName("usuario_elim")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioId).HasColumnName("usuario_id");

                entity.Property(e => e.UsuarioMod)
                    .HasColumnName("usuario_mod")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.Usuario)
                    .WithMany(p => p.Empleados)
                    .HasForeignKey(d => d.UsuarioId)
                    .HasConstraintName("FK_EMPLEADOS_USUARIOS");
            });

            modelBuilder.Entity<Entradas>(entity =>
            {
                entity.HasKey(e => e.IdEntrada);

                entity.ToTable("ENTRADAS");

                entity.Property(e => e.IdEntrada).HasColumnName("id_entrada");

                entity.Property(e => e.CreadoEn)
                    .HasColumnName("creado_en")
                    .HasColumnType("datetime");

                entity.Property(e => e.EliminadoEn)
                    .HasColumnName("eliminado_en")
                    .HasColumnType("datetime");

                entity.Property(e => e.Estado).HasColumnName("estado");

                entity.Property(e => e.FechaEmision)
                    .HasColumnName("fecha_emision")
                    .HasColumnType("datetime");

                entity.Property(e => e.ModificadoEn)
                    .HasColumnName("modificado_en")
                    .HasColumnType("datetime");

                entity.Property(e => e.NumComprobante)
                    .HasColumnName("num_comprobante")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Observaciones)
                    .HasColumnName("observaciones")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ProveedorId).HasColumnName("proveedor_id");

                entity.Property(e => e.TipoComprobante).HasColumnName("tipo_comprobante");

                entity.Property(e => e.TotalCompra)
                    .HasColumnName("total_compra")
                    .HasColumnType("decimal(10, 2)");

                entity.Property(e => e.UsuarioCrea)
                    .IsRequired()
                    .HasColumnName("usuario_crea")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioElimina)
                    .HasColumnName("usuario_elimina")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModifica)
                    .HasColumnName("usuario_modifica")
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EntradasDetalle>(entity =>
            {
                entity.HasKey(e => e.IdEntradaDetalle);

                entity.ToTable("ENTRADAS_DETALLE");

                entity.Property(e => e.IdEntradaDetalle).HasColumnName("id_entrada_detalle");

                entity.Property(e => e.Cantidad).HasColumnName("cantidad");

                entity.Property(e => e.EntradaId).HasColumnName("entrada_id");

                entity.Property(e => e.Estado).HasColumnName("estado");

                entity.Property(e => e.Precio)
                    .HasColumnName("precio")
                    .HasColumnType("decimal(10, 2)");

                entity.Property(e => e.ProductoId).HasColumnName("producto_id");

                entity.Property(e => e.StockActual).HasColumnName("stock_actual");

                entity.Property(e => e.StockNuevo).HasColumnName("stock_nuevo");

                entity.HasOne(d => d.Entrada)
                    .WithMany(p => p.EntradasDetalle)
                    .HasForeignKey(d => d.EntradaId)
                    .HasConstraintName("FK_ENTRADAS_DETALLE_ENTRADAS");

                entity.HasOne(d => d.Producto)
                    .WithMany(p => p.EntradasDetalle)
                    .HasForeignKey(d => d.ProductoId)
                    .HasConstraintName("FK_ENTRADAS_DETALLE_PRODUCTOS");
            });

            modelBuilder.Entity<EstadoCuenta>(entity =>
            {
                entity.HasKey(e => e.IdEstadoCuenta);

                entity.ToTable("ESTADO_CUENTA");

                entity.Property(e => e.IdEstadoCuenta)
                    .HasColumnName("id_estado_cuenta")
                    .ValueGeneratedNever();

                entity.Property(e => e.ClienteId).HasColumnName("cliente_id");

                entity.Property(e => e.DiasMora).HasColumnName("dias_mora");

                entity.Property(e => e.Fecha)
                    .HasColumnName("fecha")
                    .HasColumnType("datetime");

                entity.Property(e => e.TotalAbono)
                    .HasColumnName("total_abono")
                    .HasColumnType("decimal(10, 2)");

                entity.Property(e => e.TotalFactura)
                    .HasColumnName("total_factura")
                    .HasColumnType("decimal(10, 2)");

                entity.Property(e => e.TotalSaldo)
                    .HasColumnName("total_saldo")
                    .HasColumnType("decimal(10, 2)");

                entity.Property(e => e.UsuarioId).HasColumnName("usuario_id");

                entity.Property(e => e.Vence)
                    .HasColumnName("vence")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.Usuario)
                    .WithMany(p => p.EstadoCuenta)
                    .HasForeignKey(d => d.UsuarioId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ESTADO_CUENTA_USUARIOS");
            });

            modelBuilder.Entity<Factura>(entity =>
            {
                entity.HasKey(e => e.IdFactura);

                entity.ToTable("FACTURA");

                entity.HasIndex(e => e.IdFactura)
                    .HasName("UQ_SECUENCIAL_FACTURA")
                    .IsUnique();

                entity.Property(e => e.IdFactura).HasColumnName("id_factura");

                entity.Property(e => e.BaseImponible)
                    .HasColumnName("base_imponible")
                    .HasColumnType("decimal(10, 2)");

                entity.Property(e => e.Cambio)
                    .HasColumnName("cambio")
                    .HasColumnType("decimal(10, 2)");

                entity.Property(e => e.ClienteId).HasColumnName("cliente_id");

                entity.Property(e => e.CreadoEn)
                    .HasColumnName("creado_en")
                    .HasColumnType("datetime");

                entity.Property(e => e.Descuento)
                    .HasColumnName("descuento")
                    .HasColumnType("decimal(10, 2)");

                entity.Property(e => e.EliminadoEn)
                    .HasColumnName("eliminado_en")
                    .HasColumnType("datetime");

                entity.Property(e => e.Estado).HasColumnName("estado");

                entity.Property(e => e.Etapa).HasColumnName("etapa");

                entity.Property(e => e.FechaEmision)
                    .HasColumnName("fecha_emision")
                    .HasColumnType("datetime");

                entity.Property(e => e.FormaPago).HasColumnName("forma_pago");

                entity.Property(e => e.Iva)
                    .HasColumnName("iva")
                    .HasColumnType("decimal(10, 2)");

                entity.Property(e => e.MetodoPago).HasColumnName("metodo_pago");

                entity.Property(e => e.ModificadoEn)
                    .HasColumnName("modificado_en")
                    .HasColumnType("datetime");

                entity.Property(e => e.MotivoAnulacion)
                    .HasColumnName("motivo_anulacion")
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroPedido).HasColumnName("numero_pedido");

                entity.Property(e => e.PagoCliente)
                    .HasColumnName("pago_cliente")
                    .HasColumnType("decimal(10, 2)");

                entity.Property(e => e.Plazo).HasColumnName("plazo");

                entity.Property(e => e.Secuencial).HasColumnName("secuencial");

                entity.Property(e => e.Subtotal)
                    .HasColumnName("subtotal")
                    .HasColumnType("decimal(10, 2)");

                entity.Property(e => e.Subtotal0)
                    .HasColumnName("subtotal0")
                    .HasColumnType("decimal(10, 2)");

                entity.Property(e => e.Subtotal12)
                    .HasColumnName("subtotal12")
                    .HasColumnType("decimal(10, 2)");

                entity.Property(e => e.Total)
                    .HasColumnName("total")
                    .HasColumnType("decimal(10, 2)");

                entity.Property(e => e.UsuarioCrea)
                    .IsRequired()
                    .HasColumnName("usuario_crea")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioElim)
                    .HasColumnName("usuario_elim")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioId).HasColumnName("usuario_id");

                entity.Property(e => e.UsuarioMod)
                    .HasColumnName("usuario_mod")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.Cliente)
                    .WithMany(p => p.Factura)
                    .HasForeignKey(d => d.ClienteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FACTURA_CLIENTES");

                entity.HasOne(d => d.Usuario)
                    .WithMany(p => p.Factura)
                    .HasForeignKey(d => d.UsuarioId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FACTURA_USUARIOS");
            });

            modelBuilder.Entity<FacturaDetalle>(entity =>
            {
                entity.HasKey(e => e.IdFacturaDetalle);

                entity.ToTable("FACTURA_DETALLE");

                entity.Property(e => e.IdFacturaDetalle).HasColumnName("id_factura_detalle");

                entity.Property(e => e.Cantidad).HasColumnName("cantidad");

                entity.Property(e => e.Descuento)
                    .HasColumnName("descuento")
                    .HasColumnType("decimal(10, 2)");

                entity.Property(e => e.Estado).HasColumnName("estado");

                entity.Property(e => e.FacturaId).HasColumnName("factura_id");

                entity.Property(e => e.PrecioUnitario)
                    .HasColumnName("precio_unitario")
                    .HasColumnType("decimal(10, 2)");

                entity.Property(e => e.ProductoId).HasColumnName("producto_id");

                entity.HasOne(d => d.Factura)
                    .WithMany(p => p.FacturaDetalle)
                    .HasForeignKey(d => d.FacturaId)
                    .HasConstraintName("FK_FACTURA_DETALLE_FACTURA");

                entity.HasOne(d => d.Producto)
                    .WithMany(p => p.FacturaDetalle)
                    .HasForeignKey(d => d.ProductoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FACTURA_DETALLE_PRODUCTOS");
            });

            modelBuilder.Entity<Ingresos>(entity =>
            {
                entity.HasKey(e => e.IdIngresos);

                entity.ToTable("INGRESOS");

                entity.Property(e => e.IdIngresos).HasColumnName("id_ingresos");

                entity.Property(e => e.Estado).HasColumnName("estado");

                entity.Property(e => e.Fecha)
                    .HasColumnName("fecha")
                    .HasColumnType("datetime");

                entity.Property(e => e.FechaModifica)
                    .HasColumnName("fecha_modifica")
                    .HasColumnType("datetime");

                entity.Property(e => e.Observaciones)
                    .HasColumnName("observaciones")
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCrea)
                    .IsRequired()
                    .HasColumnName("usuario_crea")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioId).HasColumnName("usuario_id");

                entity.Property(e => e.UsuarioModifica)
                    .HasColumnName("usuario_modifica")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Valor)
                    .HasColumnName("valor")
                    .HasColumnType("decimal(10, 2)");

                entity.HasOne(d => d.Usuario)
                    .WithMany(p => p.Ingresos)
                    .HasForeignKey(d => d.UsuarioId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_INGRESOS_USUARIOS");
            });

            modelBuilder.Entity<ItemCatalogo>(entity =>
            {
                entity.HasKey(e => e.IdItemCatalogo);

                entity.ToTable("ITEM_CATALOGO");

                entity.Property(e => e.IdItemCatalogo).HasColumnName("id_item_catalogo");

                entity.Property(e => e.CatalogoId).HasColumnName("catalogo_id");

                entity.Property(e => e.Codigo)
                    .HasColumnName("codigo")
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.CreadoEn)
                    .HasColumnName("creado_en")
                    .HasColumnType("datetime");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasColumnName("descripcion")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EliminadoEn)
                    .HasColumnName("eliminado_en")
                    .HasColumnType("datetime");

                entity.Property(e => e.Estado).HasColumnName("estado");

                entity.Property(e => e.ModificadoEn)
                    .HasColumnName("modificado_en")
                    .HasColumnType("datetime");

                entity.Property(e => e.UsuarioCrea)
                    .IsRequired()
                    .HasColumnName("usuario_crea")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioElim)
                    .HasColumnName("usuario_elim")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioMod)
                    .HasColumnName("usuario_mod")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.Catalogo)
                    .WithMany(p => p.ItemCatalogo)
                    .HasForeignKey(d => d.CatalogoId)
                    .HasConstraintName("FK_CATALOGO_ITEM_CATALOGO");
            });

            modelBuilder.Entity<MovimientosCaja>(entity =>
            {
                entity.HasKey(e => e.IdMovimiento);

                entity.ToTable("MOVIMIENTOS_CAJA");

                entity.Property(e => e.IdMovimiento).HasColumnName("id_movimiento");

                entity.Property(e => e.Diferencia)
                    .HasColumnName("diferencia")
                    .HasColumnType("decimal(10, 2)");

                entity.Property(e => e.Fecha)
                    .HasColumnName("fecha")
                    .HasColumnType("datetime");

                entity.Property(e => e.MontoRecibido)
                    .HasColumnName("monto_recibido")
                    .HasColumnType("decimal(10, 2)");

                entity.Property(e => e.MontoSistema)
                    .HasColumnName("monto_sistema")
                    .HasColumnType("decimal(10, 2)");

                entity.Property(e => e.Observaciones)
                    .HasColumnName("observaciones")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.SaldoCaja)
                    .HasColumnName("saldo_caja")
                    .HasColumnType("decimal(10, 2)");

                entity.Property(e => e.TipoMovimiento).HasColumnName("tipo_movimiento");

                entity.Property(e => e.UsuarioId).HasColumnName("usuario_id");

                entity.HasOne(d => d.Usuario)
                    .WithMany(p => p.MovimientosCaja)
                    .HasForeignKey(d => d.UsuarioId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MOVIMIENTOS_CAJA_USUARIOS");
            });

            modelBuilder.Entity<NotaCredito>(entity =>
            {
                entity.HasKey(e => e.IdNotaCredito);

                entity.ToTable("NOTA_CREDITO");

                entity.HasIndex(e => e.IdNotaCredito)
                    .HasName("UQ_SECUENCIAL_NOTA_CREDITO")
                    .IsUnique();

                entity.Property(e => e.IdNotaCredito).HasColumnName("id_nota_credito");

                entity.Property(e => e.BaseImponible)
                    .HasColumnName("base_imponible")
                    .HasColumnType("decimal(10, 2)");

                entity.Property(e => e.ClienteId).HasColumnName("cliente_id");

                entity.Property(e => e.CreadoEn)
                    .HasColumnName("creado_en")
                    .HasColumnType("datetime");

                entity.Property(e => e.EliminadoEn)
                    .HasColumnName("eliminado_en")
                    .HasColumnType("datetime");

                entity.Property(e => e.Estado).HasColumnName("estado");

                entity.Property(e => e.FacturaId).HasColumnName("factura_id");

                entity.Property(e => e.FechaEmision)
                    .HasColumnName("fecha_emision")
                    .HasColumnType("datetime");

                entity.Property(e => e.Iva)
                    .HasColumnName("iva")
                    .HasColumnType("decimal(10, 2)");

                entity.Property(e => e.Iva0)
                    .HasColumnName("iva_0")
                    .HasColumnType("decimal(10, 2)");

                entity.Property(e => e.ModificadoEn)
                    .HasColumnName("modificado_en")
                    .HasColumnType("datetime");

                entity.Property(e => e.Secuencial).HasColumnName("secuencial");

                entity.Property(e => e.Subtotal)
                    .HasColumnName("subtotal")
                    .HasColumnType("decimal(10, 2)");

                entity.Property(e => e.Total)
                    .HasColumnName("total")
                    .HasColumnType("decimal(10, 2)");

                entity.Property(e => e.UsuarioCrea)
                    .IsRequired()
                    .HasColumnName("usuario_crea")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioElim)
                    .HasColumnName("usuario_elim")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioId).HasColumnName("usuario_id");

                entity.Property(e => e.UsuarioMod)
                    .HasColumnName("usuario_mod")
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<NotaCreditoDetalle>(entity =>
            {
                entity.HasKey(e => e.IdNotaCreditoDetalle);

                entity.ToTable("NOTA_CREDITO_DETALLE");

                entity.Property(e => e.IdNotaCreditoDetalle).HasColumnName("id_nota_credito_detalle");

                entity.Property(e => e.Cantidad).HasColumnName("cantidad");

                entity.Property(e => e.Estado).HasColumnName("estado");

                entity.Property(e => e.NotaCreditoId).HasColumnName("nota_credito_id");

                entity.Property(e => e.PrecioUnitario)
                    .HasColumnName("precio_unitario")
                    .HasColumnType("decimal(10, 2)");

                entity.Property(e => e.ProductoId).HasColumnName("producto_id");

                entity.Property(e => e.RazonModificacion)
                    .HasColumnName("razon_modificacion")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.ValorModificacion)
                    .HasColumnName("valor_modificacion")
                    .HasColumnType("decimal(10, 2)");

                entity.HasOne(d => d.NotaCredito)
                    .WithMany(p => p.NotaCreditoDetalle)
                    .HasForeignKey(d => d.NotaCreditoId)
                    .HasConstraintName("FK_NOTA_CREDITO_DETALLE_NOTA_CREDITO");
            });

            modelBuilder.Entity<Pagos>(entity =>
            {
                entity.HasKey(e => e.IdPago);

                entity.ToTable("PAGOS");

                entity.Property(e => e.IdPago).HasColumnName("id_pago");

                entity.Property(e => e.Cambio)
                    .HasColumnName("cambio")
                    .HasColumnType("decimal(10, 2)");

                entity.Property(e => e.FacturaId).HasColumnName("factura_id");

                entity.Property(e => e.Fecha)
                    .HasColumnName("fecha")
                    .HasColumnType("datetime");

                entity.Property(e => e.ImagenComprobante)
                    .HasColumnName("imagen_comprobante")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NumComprobantePago)
                    .HasColumnName("num_comprobante_pago")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Pago)
                    .HasColumnName("pago")
                    .HasColumnType("decimal(10, 2)");

                entity.Property(e => e.PagoReal)
                    .HasColumnName("pago_real")
                    .HasColumnType("decimal(10, 2)");

                entity.Property(e => e.Usuario)
                    .HasColumnName("usuario")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.Factura)
                    .WithMany(p => p.Pagos)
                    .HasForeignKey(d => d.FacturaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PAGOS_FACTURA");
            });

            modelBuilder.Entity<Parametros>(entity =>
            {
                entity.HasKey(e => e.IdParametro);

                entity.ToTable("PARAMETROS");

                entity.Property(e => e.IdParametro).HasColumnName("id_parametro");

                entity.Property(e => e.Estado)
                    .IsRequired()
                    .HasColumnName("estado")
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnName("nombre")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Valor)
                    .HasColumnName("valor")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PasswordReset>(entity =>
            {
                entity.HasKey(e => e.IdVerificacion);

                entity.ToTable("PASSWORD_RESET");

                entity.Property(e => e.IdVerificacion).HasColumnName("id_verificacion");

                entity.Property(e => e.ActualizadoEn)
                    .HasColumnName("actualizado_en")
                    .HasColumnType("datetime");

                entity.Property(e => e.CodigoVerificacion)
                    .IsRequired()
                    .HasColumnName("codigo_verificacion")
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.EmpleadoId).HasColumnName("empleado_id");

                entity.Property(e => e.Fecha)
                    .HasColumnName("fecha")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.Empleado)
                    .WithMany(p => p.PasswordReset)
                    .HasForeignKey(d => d.EmpleadoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PASSWORD_RESET_EMPLEADOS");
            });

            modelBuilder.Entity<Perfiles>(entity =>
            {
                entity.HasKey(e => e.IdPerfil);

                entity.ToTable("PERFILES");

                entity.Property(e => e.IdPerfil).HasColumnName("id_perfil");

                entity.Property(e => e.Estado).HasColumnName("estado");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnName("nombre")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Productos>(entity =>
            {
                entity.HasKey(e => e.IdProducto);

                entity.ToTable("PRODUCTOS");

                entity.Property(e => e.IdProducto).HasColumnName("id_producto");

                entity.Property(e => e.CantXBulto).HasColumnName("cant_x_bulto");

                entity.Property(e => e.CantXPaquete).HasColumnName("cant_x_paquete");

                entity.Property(e => e.CategoriaId).HasColumnName("categoria_id");

                entity.Property(e => e.Codigo)
                    .HasColumnName("codigo")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Costo)
                    .HasColumnName("costo")
                    .HasColumnType("decimal(10, 2)");

                entity.Property(e => e.CreadoEn)
                    .HasColumnName("creado_en")
                    .HasColumnType("datetime");

                entity.Property(e => e.Descripcion)
                    .HasColumnName("descripcion")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Descuento)
                    .HasColumnName("descuento")
                    .HasColumnType("decimal(10, 2)");

                entity.Property(e => e.EliminadoEn)
                    .HasColumnName("eliminado_en")
                    .HasColumnType("datetime");

                entity.Property(e => e.Estado).HasColumnName("estado");

                entity.Property(e => e.ModificadoEn)
                    .HasColumnName("modificado_en")
                    .HasColumnType("datetime");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnName("nombre")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PrecioVenta)
                    .HasColumnName("precio_venta")
                    .HasColumnType("decimal(10, 2)");

                entity.Property(e => e.ProveedorId).HasColumnName("proveedor_id");

                entity.Property(e => e.Stock).HasColumnName("stock");

                entity.Property(e => e.TarifaIva).HasColumnName("tarifa_iva");

                entity.Property(e => e.UsuarioCrea)
                    .IsRequired()
                    .HasColumnName("usuario_crea")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioElim)
                    .HasColumnName("usuario_elim")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioMod)
                    .HasColumnName("usuario_mod")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.Proveedor)
                    .WithMany(p => p.Productos)
                    .HasForeignKey(d => d.ProveedorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PRODUCTOS_PROVEEDORES");
            });

            modelBuilder.Entity<Proveedores>(entity =>
            {
                entity.HasKey(e => e.IdProveedor)
                    .HasName("PK__PROVEEDO__88BBADA44760A057");

                entity.ToTable("PROVEEDORES");

                entity.Property(e => e.IdProveedor).HasColumnName("id_proveedor");

                entity.Property(e => e.CreadoEn)
                    .HasColumnName("creado_en")
                    .HasColumnType("datetime");

                entity.Property(e => e.Direccion)
                    .IsRequired()
                    .HasColumnName("direccion")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EliminadoEn)
                    .HasColumnName("eliminado_en")
                    .HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Estado).HasColumnName("estado");

                entity.Property(e => e.ModificadoEn)
                    .HasColumnName("modificado_en")
                    .HasColumnType("datetime");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnName("nombre")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ProveedorDefault).HasColumnName("proveedor_default");

                entity.Property(e => e.Ruc)
                    .IsRequired()
                    .HasColumnName("ruc")
                    .HasMaxLength(13)
                    .IsUnicode(false);

                entity.Property(e => e.Telefono)
                    .HasColumnName("telefono")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCrea)
                    .IsRequired()
                    .HasColumnName("usuario_crea")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioElim)
                    .HasColumnName("usuario_elim")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioMod)
                    .HasColumnName("usuario_mod")
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Secuenciales>(entity =>
            {
                entity.HasKey(e => e.IdSecuencial);

                entity.ToTable("SECUENCIALES");

                entity.Property(e => e.IdSecuencial).HasColumnName("id_secuencial");

                entity.Property(e => e.Nombre)
                    .HasColumnName("nombre")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Secuencial).HasColumnName("secuencial");
            });

            modelBuilder.Entity<Usuarios>(entity =>
            {
                entity.HasKey(e => e.IdUsuario);

                entity.ToTable("USUARIOS");

                entity.HasIndex(e => e.Usuario)
                    .HasName("UQ_USUARIOS")
                    .IsUnique();

                entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");

                entity.Property(e => e.Clave)
                    .IsRequired()
                    .HasColumnName("clave")
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.CodigoVerificacion)
                    .IsRequired()
                    .HasColumnName("codigo_verificacion")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.CreadoEn)
                    .HasColumnName("creado_en")
                    .HasColumnType("datetime");

                entity.Property(e => e.EliminadoEn)
                    .HasColumnName("eliminado_en")
                    .HasColumnType("datetime");

                entity.Property(e => e.Estado).HasColumnName("estado");

                entity.Property(e => e.ModificadoEn)
                    .HasColumnName("modificado_en")
                    .HasColumnType("datetime");

                entity.Property(e => e.PerfilId).HasColumnName("perfil_id");

                entity.Property(e => e.Usuario)
                    .IsRequired()
                    .HasColumnName("usuario")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCrea)
                    .IsRequired()
                    .HasColumnName("usuario_crea")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioElim)
                    .HasColumnName("usuario_elim")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioMod)
                    .HasColumnName("usuario_mod")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.VerificadoEn)
                    .HasColumnName("verificado_en")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.Perfil)
                    .WithMany(p => p.Usuarios)
                    .HasForeignKey(d => d.PerfilId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_USUARIOS_PERFILES");
            });

            modelBuilder.Entity<Visitas>(entity =>
            {
                entity.HasKey(e => e.IdVisita);

                entity.ToTable("VISITAS");

                entity.Property(e => e.IdVisita).HasColumnName("id_visita");

                entity.Property(e => e.Fecha)
                    .HasColumnName("fecha")
                    .HasColumnType("datetime");

                entity.Property(e => e.Observacion)
                    .IsRequired()
                    .HasColumnName("observacion")
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.OrderId).HasColumnName("order_id");

                entity.Property(e => e.UsuarioVisita)
                    .IsRequired()
                    .HasColumnName("usuario_visita")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.Visitas)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_VISITAS_FACTURA");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
