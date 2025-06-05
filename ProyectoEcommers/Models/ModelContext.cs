namespace ProyectoEcommers.Models


{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using System.Collections.Generic;
    using System.Reflection.Emit;

    public partial class ModelContext : DbContext, IModelContext
    {
        private readonly IConfiguration _configuration;

        public ModelContext(DbContextOptions<ModelContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }

        public DbSet<Ctr_usuarios> Ctr_usuarios { get; set; } = null!;
        public DbSet<ctr_roles> ctr_roles { get; set; } = null!;
        public DbSet<ctr_roles_user> ctr_roles_user { get; set; } = null!;
        public DbSet<v_slider> v_slider { get; set; } = null!;
        public DbSet<ctr_menu_roles> ctr_menu_roles { get; set; } = null!;
        public DbSet<ctr_menu> ctr_menu { get; set; } = null!;
        public DbSet<imagenes> imagenes { get; set; } = null!;
        public DbSet<imagenesizquierda> imagenesizquierda { get; set; } = null!;
        public DbSet<productos> productos { get; set; } = null!;
        public DbSet<imagenes_producto> imagenes_producto { get; set; } = null!;
        public DbSet<ImagenesInferior> ImagenesInferior { get; set; } = null!;
        public DbSet<imagenesComentarios> imagenesComentarios { get; set; } = null!;
        public DbSet<ctrl_dominios> ctrl_dominios { get; set; } = null!;
        public DbSet<comentarios_clientes> comentarios_clientes { get; set; } = null!;
        public DbSet<mi_carrito> mi_carrito { get; set; } = null!;
        public DbSet<ctr_paises> ctr_paises { get; set; } = null!;
        public DbSet<ctr_mensajes> ctr_mensajes { get; set; } = null!;
        public DbSet<mi_compra_realizada> mi_compra_realizada { get; set; } = null!;
        public DbSet<envio_correo> envio_correo { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Obtener la cadena de conexión desde el archivo de configuración (appsettings.json o appsettings.Production.json)
                var connectionString = _configuration.GetConnectionString("MySqlConnection");
                optionsBuilder.UseMySQL(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Definir el esquema y la collation por defecto
            modelBuilder.HasDefaultSchema("distri21_bdtienda").UseCollation("utf8mb4_unicode_ci");

            // Configuración de las entidades
            modelBuilder.Entity<Ctr_usuarios>(entity =>
            {
                entity.ToTable("ctr_usuarios");
                entity.Property(e => e.IdUsuario).HasColumnType("NUMBER").HasColumnName("ID_USUARIO");
                entity.Property(e => e.Identificacion).HasMaxLength(100).HasColumnName("IDENTIFICACION");
                entity.Property(e => e.Username).HasMaxLength(100).IsUnicode(false).HasColumnName("USERNAME");
                entity.Property(e => e.Vigente).HasMaxLength(100).HasColumnName("VIGENTE");
            });

            // Otros mapeos y configuraciones de tus entidades
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
