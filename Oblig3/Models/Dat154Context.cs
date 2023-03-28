using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Oblig3.Models;

public partial class Dat154Context : DbContext
{
    public Dat154Context()
    {
    }

    public Dat154Context(DbContextOptions<Dat154Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Grade> Grades { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("data source=dat154.hvl.no,1443;initial catalog=dat154;user id=dat154_rw;password=dat154_rw;Encrypt=false");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Danish_Norwegian_CI_AS");

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.Coursecode).HasName("pk_course");

            entity.ToTable("course");

            entity.Property(e => e.Coursecode)
                .HasMaxLength(6)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("coursecode");
            entity.Property(e => e.Coursename)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("coursename");
            entity.Property(e => e.Semester)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("semester");
            entity.Property(e => e.Teacher)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("teacher");
        });

        modelBuilder.Entity<Grade>(entity =>
        {
            entity.HasKey(e => new { e.Coursecode, e.Studentid }).HasName("pk_grade");

            entity.ToTable("grade");

            entity.Property(e => e.Coursecode)
                .HasMaxLength(6)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("coursecode");
            entity.Property(e => e.Studentid).HasColumnName("studentid");
            entity.Property(e => e.Grade1)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("grade");

            entity.HasOne(d => d.CoursecodeNavigation).WithMany(p => p.Grades)
                .HasForeignKey(d => d.Coursecode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_course");

            entity.HasOne(d => d.Student).WithMany(p => p.Grades)
                .HasForeignKey(d => d.Studentid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_student");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Primary");

            entity.ToTable("student");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Studentage).HasColumnName("studentage");
            entity.Property(e => e.Studentname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("studentname");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
