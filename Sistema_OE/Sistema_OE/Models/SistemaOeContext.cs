using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Sistema_OE.Models;

public partial class SistemaOeContext : DbContext
{
    public SistemaOeContext()
    {
    }

    public SistemaOeContext(DbContextOptions<SistemaOeContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Administrativo> Administrativos { get; set; }

    public virtual DbSet<Citum> Cita { get; set; }

    public virtual DbSet<Equipointerdiciplinario> Equipointerdiciplinarios { get; set; }

    public virtual DbSet<Estudiante> Estudiantes { get; set; }

    public virtual DbSet<Estudianteseccionanio> Estudianteseccionanios { get; set; }

    public virtual DbSet<Expediente> Expedientes { get; set; }

    public virtual DbSet<Impartematerium> Impartemateria { get; set; }

    public virtual DbSet<Materiacursada> Materiacursadas { get; set; }

    public virtual DbSet<Materium> Materia { get; set; }

    public virtual DbSet<Profesor> Profesors { get; set; }

    public virtual DbSet<Seccion> Seccions { get; set; }

    public virtual DbSet<Seccionasignadum> Seccionasignada { get; set; }

    public virtual DbSet<Telefonoadmin> Telefonoadmins { get; set; }

    public virtual DbSet<Telefonoencargado> Telefonoencargados { get; set; }

    public virtual DbSet<Telefonoequipo> Telefonoequipos { get; set; }

    public virtual DbSet<Telefonoprofesor> Telefonoprofesors { get; set; }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
    //    => optionsBuilder.UseSqlServer("server=LAPBL-N165582\\SQLEXPRESS; database=SistemaOE; integrated security=true; TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Administrativo>(entity =>
        {
            entity.HasKey(e => e.IdAdministrativo).HasName("PK__ADMINIST__E2120FB8CD420D2D");

            entity.ToTable("ADMINISTRATIVO");

            entity.Property(e => e.IdAdministrativo).HasColumnName("idAdministrativo");
            entity.Property(e => e.Apellidos)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("apellidos");
            entity.Property(e => e.Domicilio)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("domicilio");
            entity.Property(e => e.Email)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Nombre)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("password");
        });

        modelBuilder.Entity<Citum>(entity =>
        {
            entity.HasKey(e => e.NumCita).HasName("PK__CITA__7AD3B95DB11EBA40");

            entity.ToTable("CITA");

            entity.Property(e => e.NumCita)
                .ValueGeneratedNever()
                .HasColumnName("numCita");
            entity.Property(e => e.Administrativo).HasColumnName("administrativo");
            entity.Property(e => e.EquipoInterdiciplinario).HasColumnName("equipoInterdiciplinario");
            entity.Property(e => e.Estado)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("estado");
            entity.Property(e => e.Estudiante).HasColumnName("estudiante");
            entity.Property(e => e.FechaAsignada)
                .HasColumnType("datetime")
                .HasColumnName("fechaAsignada");
            entity.Property(e => e.Motivo)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("motivo");
            entity.Property(e => e.Observacion)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("observacion");
            entity.Property(e => e.Tiempo).HasColumnName("tiempo");
            entity.Property(e => e.Tipo)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("tipo");

            entity.HasOne(d => d.AdministrativoNavigation).WithMany(p => p.Cita)
                .HasForeignKey(d => d.Administrativo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CITA__administra__44FF419A");

            entity.HasOne(d => d.EquipoInterdiciplinarioNavigation).WithMany(p => p.Cita)
                .HasForeignKey(d => d.EquipoInterdiciplinario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CITA__equipoInte__4316F928");

            entity.HasOne(d => d.EstudianteNavigation).WithMany(p => p.Cita)
                .HasForeignKey(d => d.Estudiante)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CITA__estudiante__440B1D61");
        });

        modelBuilder.Entity<Equipointerdiciplinario>(entity =>
        {
            entity.HasKey(e => e.Cedula).HasName("PK__EQUIPOIN__415B7BE4B7C5AA24");

            entity.ToTable("EQUIPOINTERDICIPLINARIO");

            entity.Property(e => e.Cedula)
                .ValueGeneratedNever()
                .HasColumnName("cedula");
            entity.Property(e => e.Domicilio)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("domicilio");
            entity.Property(e => e.Email)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Especialidad)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("especialidad");
            entity.Property(e => e.Nombre)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Estudiante>(entity =>
        {
            entity.HasKey(e => e.Cedula).HasName("PK__ESTUDIAN__415B7BE426237E5A");

            entity.ToTable("ESTUDIANTE");

            entity.Property(e => e.Cedula)
                .ValueGeneratedNever()
                .HasColumnName("cedula");
            entity.Property(e => e.Apellidos)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("apellidos");
            entity.Property(e => e.Domicilio)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("domicilio");
            entity.Property(e => e.Email)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Genero)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("genero");
            entity.Property(e => e.Nombre)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Estudianteseccionanio>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ESTUDIAN__3213E83F2D340780");

            entity.ToTable("ESTUDIANTESECCIONANIO");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Ano).HasColumnName("ano");
            entity.Property(e => e.CedulaEstudiante).HasColumnName("cedulaEstudiante");
            entity.Property(e => e.NumSeccion).HasColumnName("numSeccion");

            entity.HasOne(d => d.CedulaEstudianteNavigation).WithMany(p => p.Estudianteseccionanios)
                .HasForeignKey(d => d.CedulaEstudiante)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ESTUDIANT__cedul__3F466844");

            entity.HasOne(d => d.NumSeccionNavigation).WithMany(p => p.Estudianteseccionanios)
                .HasForeignKey(d => d.NumSeccion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ESTUDIANT__numSe__403A8C7D");
        });

        modelBuilder.Entity<Expediente>(entity =>
        {
            entity.HasKey(e => e.NumExpediente).HasName("PK__EXPEDIEN__67B2C94FFD4EBD26");

            entity.ToTable("EXPEDIENTE");

            entity.HasIndex(e => e.CedulaEncargado, "UQ__EXPEDIEN__061F92A8455010C2").IsUnique();

            entity.HasIndex(e => e.Estudiante, "UQ__EXPEDIEN__92AA6D36B7291EFD").IsUnique();

            entity.Property(e => e.NumExpediente)
                .ValueGeneratedNever()
                .HasColumnName("numExpediente");
            entity.Property(e => e.CedulaEncargado).HasColumnName("cedulaEncargado");
            entity.Property(e => e.EmailEncargado)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("emailEncargado");
            entity.Property(e => e.EncargadoLegal)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("encargadoLegal");
            entity.Property(e => e.Estudiante).HasColumnName("estudiante");
            entity.Property(e => e.Observaciones)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("observaciones");

            entity.HasOne(d => d.EstudianteNavigation).WithOne(p => p.Expediente)
                .HasForeignKey<Expediente>(d => d.Estudiante)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EXPEDIENT__estud__49C3F6B7");
        });

        modelBuilder.Entity<Impartematerium>(entity =>
        {
            entity.HasKey(e => e.IdImparteMateria).HasName("PK__IMPARTEM__C282A83A70B6483A");

            entity.ToTable("IMPARTEMATERIA");

            entity.Property(e => e.IdImparteMateria).HasColumnName("idImparteMateria");
            entity.Property(e => e.Fecha)
                .HasColumnType("date")
                .HasColumnName("fecha");
            entity.Property(e => e.Horario)
                .HasColumnType("datetime")
                .HasColumnName("horario");
            entity.Property(e => e.NumMateria).HasColumnName("numMateria");
            entity.Property(e => e.ProfesorCedula).HasColumnName("profesorCedula");

            entity.HasOne(d => d.NumMateriaNavigation).WithMany(p => p.Impartemateria)
                .HasForeignKey(d => d.NumMateria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__IMPARTEMA__numMa__5165187F");

            entity.HasOne(d => d.ProfesorCedulaNavigation).WithMany(p => p.Impartemateria)
                .HasForeignKey(d => d.ProfesorCedula)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__IMPARTEMA__profe__5070F446");
        });

        modelBuilder.Entity<Materiacursada>(entity =>
        {
            entity.HasKey(e => e.IdMateriaCursadas).HasName("PK__MATERIAC__6F3CE28667DE7A97");

            entity.ToTable("MATERIACURSADAS");

            entity.Property(e => e.IdMateriaCursadas).HasColumnName("idMateriaCursadas");
            entity.Property(e => e.Estudiante).HasColumnName("estudiante");
            entity.Property(e => e.Fecha)
                .HasColumnType("date")
                .HasColumnName("fecha");
            entity.Property(e => e.NumMateria).HasColumnName("numMateria");

            entity.HasOne(d => d.EstudianteNavigation).WithMany(p => p.Materiacursada)
                .HasForeignKey(d => d.Estudiante)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MATERIACU__estud__5535A963");

            entity.HasOne(d => d.NumMateriaNavigation).WithMany(p => p.Materiacursada)
                .HasForeignKey(d => d.NumMateria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MATERIACU__numMa__5441852A");
        });

        modelBuilder.Entity<Materium>(entity =>
        {
            entity.HasKey(e => e.NumMateria).HasName("PK__MATERIA__BB8FAADE946B1166");

            entity.ToTable("MATERIA");

            entity.Property(e => e.NumMateria)
                .ValueGeneratedNever()
                .HasColumnName("numMateria");
            entity.Property(e => e.Nombre)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Profesor>(entity =>
        {
            entity.HasKey(e => e.Cedula).HasName("PK__PROFESOR__415B7BE4B26D262A");

            entity.ToTable("PROFESOR");

            entity.Property(e => e.Cedula)
                .ValueGeneratedNever()
                .HasColumnName("cedula");
            entity.Property(e => e.Apellidos)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("apellidos");
            entity.Property(e => e.Domicilio)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("domicilio");
            entity.Property(e => e.Email)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Nombre)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Seccion>(entity =>
        {
            entity.HasKey(e => e.NumSeccion).HasName("PK__SECCION__B134B244F81128E9");

            entity.ToTable("SECCION");

            entity.Property(e => e.NumSeccion).HasColumnName("numSeccion");
            entity.Property(e => e.CantidadEstudiante)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("cantidadEstudiante");
        });

        modelBuilder.Entity<Seccionasignadum>(entity =>
        {
            entity.HasKey(e => e.IdseccionAsignada).HasName("PK__SECCIONA__B986D186328453A4");

            entity.ToTable("SECCIONASIGNADA");

            entity.Property(e => e.IdseccionAsignada).HasColumnName("idseccionAsignada");
            entity.Property(e => e.Fecha)
                .HasColumnType("date")
                .HasColumnName("fecha");
            entity.Property(e => e.NumSeccion).HasColumnName("numSeccion");
            entity.Property(e => e.ProfesorCedula).HasColumnName("profesorCedula");

            entity.HasOne(d => d.NumSeccionNavigation).WithMany(p => p.Seccionasignada)
                .HasForeignKey(d => d.NumSeccion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SECCIONAS__numSe__59063A47");

            entity.HasOne(d => d.ProfesorCedulaNavigation).WithMany(p => p.Seccionasignada)
                .HasForeignKey(d => d.ProfesorCedula)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SECCIONAS__profe__5812160E");
        });

        modelBuilder.Entity<Telefonoadmin>(entity =>
        {
            entity.HasKey(e => e.IdTelefono).HasName("PK__TELEFONO__39C142DFE45ECED8");

            entity.ToTable("TELEFONOADMIN");

            entity.Property(e => e.IdTelefono).HasColumnName("idTelefono");
            entity.Property(e => e.IdAdministrativo).HasColumnName("idAdministrativo");
            entity.Property(e => e.Numero).HasColumnName("numero");
            entity.Property(e => e.Tipo)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("tipo");

            entity.HasOne(d => d.IdAdministrativoNavigation).WithMany(p => p.Telefonoadmins)
                .HasForeignKey(d => d.IdAdministrativo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TELEFONOA__idAdm__5BE2A6F2");
        });

        modelBuilder.Entity<Telefonoencargado>(entity =>
        {
            entity.HasKey(e => e.IdTelefono).HasName("PK__TELEFONO__39C142DF6E48642C");

            entity.ToTable("TELEFONOENCARGADO");

            entity.Property(e => e.IdTelefono).HasColumnName("idTelefono");
            entity.Property(e => e.NumExpediente).HasColumnName("numExpediente");
            entity.Property(e => e.Numero).HasColumnName("numero");
            entity.Property(e => e.Tipo)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("tipo");

            entity.HasOne(d => d.NumExpedienteNavigation).WithMany(p => p.Telefonoencargados)
                .HasForeignKey(d => d.NumExpediente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TELEFONOE__numEx__5EBF139D");
        });

        modelBuilder.Entity<Telefonoequipo>(entity =>
        {
            entity.HasKey(e => e.IdTelefono).HasName("PK__TELEFONO__39C142DF61EBF561");

            entity.ToTable("TELEFONOEQUIPO");

            entity.Property(e => e.IdTelefono).HasColumnName("idTelefono");
            entity.Property(e => e.EquipoInterdiciplinario).HasColumnName("equipoInterdiciplinario");
            entity.Property(e => e.Numero).HasColumnName("numero");
            entity.Property(e => e.Tipo)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("tipo");

            entity.HasOne(d => d.EquipoInterdiciplinarioNavigation).WithMany(p => p.Telefonoequipos)
                .HasForeignKey(d => d.EquipoInterdiciplinario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TELEFONOE__equip__619B8048");
        });

        modelBuilder.Entity<Telefonoprofesor>(entity =>
        {
            entity.HasKey(e => e.IdTelefono).HasName("PK__TELEFONO__39C142DFAE8A53B7");

            entity.ToTable("TELEFONOPROFESOR");

            entity.Property(e => e.IdTelefono).HasColumnName("idTelefono");
            entity.Property(e => e.Numero).HasColumnName("numero");
            entity.Property(e => e.ProfesorCedula).HasColumnName("profesorCedula");
            entity.Property(e => e.Tipo)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("tipo");

            entity.HasOne(d => d.ProfesorCedulaNavigation).WithMany(p => p.Telefonoprofesors)
                .HasForeignKey(d => d.ProfesorCedula)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TELEFONOP__profe__6477ECF3");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
