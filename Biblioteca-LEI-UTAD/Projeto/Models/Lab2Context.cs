using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Trabalho.Areas.Data;

namespace Trabalho.Models;

public partial class Lab2Context : IdentityDbContext<AppUser>
{
    public Lab2Context()
    {
    }

    public Lab2Context(DbContextOptions<Lab2Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Administradore> Administradores { get; set; }

    public virtual DbSet<Autenticado> Autenticados { get; set; }

    public virtual DbSet<Autor> Autors { get; set; }

    public virtual DbSet<Biblioteca> Bibliotecas { get; set; }

    public virtual DbSet<Bibliotecario> Bibliotecarios { get; set; }

    public virtual DbSet<Bloquear> Bloquears { get; set; }

    public virtual DbSet<CriaAdmin> CriaAdmins { get; set; }

    public virtual DbSet<EntregasAtraso> EntregasAtrasos { get; set; }

    public virtual DbSet<HistoricoRequ> HistoricoRequs { get; set; }

    public virtual DbSet<Leitore> Leitores { get; set; }

    public virtual DbSet<Livro> Livros { get; set; }

    public virtual DbSet<MotivoBloq> MotivoBloqs { get; set; }

    public virtual DbSet<Requisicao> Requisicaos { get; set; }

    public virtual DbSet<ValidarRegisto> ValidarRegistos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Administradore>(entity =>
        {
            entity.HasKey(e => e.UsernameAdmin).HasName("PK__Administ__266ED3E6575F5872");

            entity.Property(e => e.UsernameAdmin)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Username_Admin");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Nome)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Autenticado>(entity =>
        {
            entity.HasKey(e => e.Username).HasName("PK__Autentic__536C85E503316A59");

            entity.ToTable("Autenticado");

            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Contacto).HasMaxLength(255);
            entity.Property(e => e.Contacto2).HasMaxLength(255);
            entity.Property(e => e.Contacto3).HasMaxLength(255);
            entity.Property(e => e.Nome)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Autor>(entity =>
        {
            entity.HasKey(e => e.IdAutor).HasName("PK__Autor__9626AD264DB6FFA5");

            entity.ToTable("Autor");

            entity.Property(e => e.IdAutor)
                .ValueGeneratedNever()
                .HasColumnName("ID_Autor");
            entity.Property(e => e.NameAutor)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Name_Autor");
        });

        modelBuilder.Entity<Biblioteca>(entity =>
        {
            entity.HasKey(e => e.IdBiblioteca).HasName("PK__Bibliote__906E4B4CC3FA4858");

            entity.ToTable("Biblioteca");

            entity.Property(e => e.IdBiblioteca)
                .ValueGeneratedNever()
                .HasColumnName("ID_Biblioteca");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Localizacao)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Nome)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Bibliotecario>(entity =>
        {
            entity.HasKey(e => e.UsernameBib).HasName("PK__Bibliote__1E4CD8D2F5865935");

            entity.ToTable("Bibliotecario");

            entity.Property(e => e.UsernameBib)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Username_Bib");

            entity.HasOne(d => d.UsernameBibNavigation).WithOne(p => p.Bibliotecario)
                .HasForeignKey<Bibliotecario>(d => d.UsernameBib)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Bibliotec__Usern__3C69FB99");
        });

        modelBuilder.Entity<Bloquear>(entity =>
        {
            entity.HasKey(e => e.IdBloqueio).HasName("PK__Bloquear__6267F0075C931E99");

            entity.ToTable("Bloquear");

            entity.Property(e => e.IdBloqueio)
                .ValueGeneratedNever()
                .HasColumnName("ID_Bloqueio");
            entity.Property(e => e.DataBloqueio)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime")
                .HasColumnName("Data_Bloqueio");
            entity.Property(e => e.IdMotivoBloq).HasColumnName("ID_MotivoBloq");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UsernameAdmin)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Username_Admin");

            entity.HasOne(d => d.IdMotivoBloqNavigation).WithMany(p => p.Bloquears)
                .HasForeignKey(d => d.IdMotivoBloq)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Bloquear__ID_Mot__4BAC3F29");

            entity.HasOne(d => d.UsernameNavigation).WithMany(p => p.Bloquears)
                .HasForeignKey(d => d.Username)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Bloquear__Userna__4AB81AF0");

            entity.HasOne(d => d.UsernameAdminNavigation).WithMany(p => p.Bloquears)
                .HasForeignKey(d => d.UsernameAdmin)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Bloquear__Userna__49C3F6B7");
        });

        modelBuilder.Entity<CriaAdmin>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Cria_Admin");

            entity.Property(e => e.UsernameAdmin)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Username_Admin");
            entity.Property(e => e.UsernameNovoAdmin)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Username_NovoAdmin");

            entity.HasOne(d => d.UsernameAdminNavigation).WithMany()
                .HasForeignKey(d => d.UsernameAdmin)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Cria_Admi__Usern__693CA210");

            entity.HasOne(d => d.UsernameNovoAdminNavigation).WithMany()
                .HasForeignKey(d => d.UsernameNovoAdmin)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Cria_Admi__Usern__6A30C649");
        });

        modelBuilder.Entity<EntregasAtraso>(entity =>
        {
            entity.HasKey(e => e.IdEntregas).HasName("PK__Entregas__0FB6D86D817DB75A");

            entity.ToTable("EntregasAtraso");

            entity.Property(e => e.IdEntregas)
                .ValueGeneratedNever()
                .HasColumnName("ID_Entregas");
            entity.Property(e => e.IdRequisicao).HasColumnName("ID_Requisicao");

            entity.HasOne(d => d.IdRequisicaoNavigation).WithMany(p => p.EntregasAtrasos)
                .HasForeignKey(d => d.IdRequisicao)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EntregasA__ID_Re__6E01572D");
        });

        modelBuilder.Entity<HistoricoRequ>(entity =>
        {
            entity.HasKey(e => e.IdHistorico).HasName("PK__Historic__ECA88795D04FBAF5");

            entity.ToTable("HistoricoRequ");

            entity.Property(e => e.IdHistorico)
                .ValueGeneratedNever()
                .HasColumnName("ID_Historico");
            entity.Property(e => e.IdRequisicao).HasColumnName("ID_Requisicao");

            entity.HasOne(d => d.IdRequisicaoNavigation).WithMany(p => p.HistoricoRequs)
                .HasForeignKey(d => d.IdRequisicao)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Historico__ID_Re__71D1E811");
        });

        modelBuilder.Entity<Leitore>(entity =>
        {
            entity.HasKey(e => e.UsernameLei).HasName("PK__Leitores__15E92569CF5B3BB7");

            entity.Property(e => e.UsernameLei)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Username_Lei");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Morada)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.UsernameLeiNavigation).WithOne(p => p.Leitore)
                .HasForeignKey<Leitore>(d => d.UsernameLei)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Leitores__Userna__3F466844");
        });

        modelBuilder.Entity<Livro>(entity =>
        {
            entity.HasKey(e => e.Isbn).HasName("PK__Livro__447D36EB33DDD9E3");

            entity.ToTable("Livro");

            entity.Property(e => e.Isbn)
                .ValueGeneratedNever()
                .HasColumnName("ISBN");
            entity.Property(e => e.Genero)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.IdAutor).HasColumnName("ID_Autor");
            entity.Property(e => e.IdBiblioteca).HasColumnName("ID_Biblioteca");
            entity.Property(e => e.NExemplares).HasColumnName("N_Exemplares");
            entity.Property(e => e.Preco).HasColumnType("money");
            entity.Property(e => e.Titulo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UsernameBibAdd)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Username_BibAdd");

            entity.HasOne(d => d.IdAutorNavigation).WithMany(p => p.Livros)
                .HasForeignKey(d => d.IdAutor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Livro__ID_Autor__5DCAEF64");

            entity.HasOne(d => d.IdBibliotecaNavigation).WithMany(p => p.Livros)
                .HasForeignKey(d => d.IdBiblioteca)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Livro__ID_Biblio__5EBF139D");

            entity.HasOne(d => d.UsernameBibAddNavigation).WithMany(p => p.Livros)
                .HasForeignKey(d => d.UsernameBibAdd)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Livro__Username___5CD6CB2B");
        });

        modelBuilder.Entity<MotivoBloq>(entity =>
        {
            entity.HasKey(e => e.IdMotivoBloq).HasName("PK__MotivoBl__FD6AC615A6A67AE0");

            entity.ToTable("MotivoBloq");

            entity.Property(e => e.IdMotivoBloq)
                .ValueGeneratedNever()
                .HasColumnName("ID_MotivoBloq");
            entity.Property(e => e.Descricao)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Requisicao>(entity =>
        {
            entity.HasKey(e => e.IdRequisicao).HasName("PK__Requisic__7B7D23BB3CB5D882");

            entity.ToTable("Requisicao");

            entity.Property(e => e.IdRequisicao)
                .ValueGeneratedNever()
                .HasColumnName("ID_Requisicao");
            entity.Property(e => e.DataDevolucao)
                .HasColumnType("smalldatetime")
                .HasColumnName("Data_Devolucao");
            entity.Property(e => e.DataRequisicao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime")
                .HasColumnName("Data_Requisicao");
            entity.Property(e => e.Isbn).HasColumnName("ISBN");
            entity.Property(e => e.UsernameBib)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Username_Bib");
            entity.Property(e => e.UsernameLei)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Username_Lei");

            entity.HasOne(d => d.IsbnNavigation).WithMany(p => p.Requisicaos)
                .HasForeignKey(d => d.Isbn)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Requisicao__ISBN__6754599E");

            entity.HasOne(d => d.UsernameBibNavigation).WithMany(p => p.Requisicaos)
                .HasForeignKey(d => d.UsernameBib)
                .HasConstraintName("FK__Requisica__Usern__656C112C");

            entity.HasOne(d => d.UsernameLeiNavigation).WithMany(p => p.Requisicaos)
                .HasForeignKey(d => d.UsernameLei)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Requisica__Usern__66603565");
        });

        modelBuilder.Entity<ValidarRegisto>(entity =>
        {
            entity.HasKey(e => e.IdValid).HasName("PK__ValidarR__7DA04BE5E1DF26D3");

            entity.ToTable("ValidarRegisto");

            entity.Property(e => e.IdValid)
                .ValueGeneratedNever()
                .HasColumnName("ID_Valid");
            entity.Property(e => e.DataValid)
                .HasColumnType("smalldatetime")
                .HasColumnName("Data_Valid");
            entity.Property(e => e.UsernameAdmin)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Username_Admin");
            entity.Property(e => e.UsernameBib)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Username_Bib");

            entity.HasOne(d => d.UsernameAdminNavigation).WithMany(p => p.ValidarRegistos)
                .HasForeignKey(d => d.UsernameAdmin)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ValidarRe__Usern__5070F446");

            entity.HasOne(d => d.UsernameBibNavigation).WithMany(p => p.ValidarRegistos)
                .HasForeignKey(d => d.UsernameBib)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ValidarRe__Usern__5165187F");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
