using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MoviePlanetLibrary.Models;

public partial class MoviePlanetDbContext : DbContext
{
    public MoviePlanetDbContext()
    {
    }

    public MoviePlanetDbContext(DbContextOptions<MoviePlanetDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CompanyInfo> CompanyInfos { get; set; }

    public virtual DbSet<Movies> MovieInfos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=mssqlserver.caztckpohutb.us-east-1.rds.amazonaws.com,1433;Initial Catalog=MoviePlanetDB;User ID=Gurvir;Password=password;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CompanyInfo>(entity =>
        {
            entity.HasKey(e => e.CompanyId).HasName("PK__CompanyI__2D971CAC45FC1304");

            entity.ToTable("CompanyInfo");

            entity.Property(e => e.CompanyName).HasMaxLength(50);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Headquarters).HasMaxLength(150);
        });

        modelBuilder.Entity<Movies>(entity =>
        {
            entity.HasKey(e => e.MovieEidr).HasName("PK__MovieInf__043BD9955DF56F71");

            entity.ToTable("MovieInfo");

            entity.Property(e => e.MovieEidr)
                .ValueGeneratedNever()
                .HasColumnName("MovieEIDR");
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.Director).HasMaxLength(150);
            entity.Property(e => e.Genre).HasMaxLength(150);
            entity.Property(e => e.MovieTitle).HasMaxLength(100);
            entity.Property(e => e.ReleaseDate).HasColumnType("date");
            entity.Property(e => e.WorldwideProfit).HasColumnType("decimal(15, 0)");

            entity.HasOne(d => d.Company).WithMany(p => p.Movies)
                .HasForeignKey(d => d.CompanyId)
                .HasConstraintName("FK_Movies_Companies_CompanyId");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
