using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace cineSistem.Models;

public partial class CineSistemContext : IdentityDbContext
{
    public CineSistemContext()
    {
    }

    public CineSistemContext(DbContextOptions<CineSistemContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Pelicula> Peliculas { get; set; }

    public virtual DbSet<RankingPelicula> RankingPeliculas { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Initial Catalog=cineSistem;integrated security=True; TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Pelicula>(entity =>
        {
            entity.HasKey(e => e.IdPeli).HasName("PK__pelicula__3D78D11A11A30BBD");

            entity.ToTable("pelicula");

            entity.Property(e => e.IdPeli).HasColumnName("idPel");
            entity.Property(e => e.Description)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.Director)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("director");
            entity.Property(e => e.Gender)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("genero");
            entity.Property(e => e.Image)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("imagen");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<RankingPelicula>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__rankingP__3214EC071A9437B9");

            entity.ToTable("rankingPelicula");

            entity.Property(e => e.IdPeli).HasColumnName("idPel");
            entity.Property(e => e.Ranking).HasColumnName("ranking");

            entity.HasOne(d => d.IdPelNavigation).WithMany(p => p.RankingPeliculas)
                .HasForeignKey(d => d.IdPeli)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_peliculaRank");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUser).HasName("PK__usuario__3717C982048E11CA");

            entity.ToTable("usuario");

            entity.Property(e => e.IdUser).HasColumnName("idUser");
            entity.Property(e => e.Email)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Password)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("password");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
