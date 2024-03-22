using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SanctionScannerAppLibrary.Models.Entities
{
    public partial class DBLIBRARYContext : DbContext
    {
        public DBLIBRARYContext()
        {
        }

        public DBLIBRARYContext(DbContextOptions<DBLIBRARYContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Book> Books { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Employee> Employees { get; set; } = null!;
        public virtual DbSet<Punishment> Punishments { get; set; } = null!;
        public virtual DbSet<Transaction> Transactions { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<Writer> Writers { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional:true, reloadOnChange:true).Build();
                optionsBuilder.UseSqlServer(configuration.GetConnectionString("SqlConnection"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>(entity =>
            {
                entity.Property(e => e.InsertTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.InsertedBy)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('Anonymous')");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Page).HasMaxLength(4);

                entity.Property(e => e.Picture).HasMaxLength(250);

                entity.Property(e => e.PublicationYear)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Publisher).HasMaxLength(50);

                entity.HasOne(d => d.CategoryNoNavigation)
                    .WithMany(p => p.Books)
                    .HasForeignKey(d => d.CategoryNo)
                    .HasConstraintName("FK_Books_Categories");

                entity.HasOne(d => d.WriterNoNavigation)
                    .WithMany(p => p.Books)
                    .HasForeignKey(d => d.WriterNo)
                    .HasConstraintName("FK_Books_Writers");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.InsertTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.InsertedBy)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('Anonymous')");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Employee1)
                    .HasMaxLength(50)
                    .HasColumnName("Employee");

                entity.Property(e => e.InsertTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.InsertedBy)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('Anonymous')");
            });

            modelBuilder.Entity<Punishment>(entity =>
            {
                entity.Property(e => e.EndDate).HasColumnType("smalldatetime");

                entity.Property(e => e.InsertTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.InsertedBy)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('Anonymous')");

                entity.Property(e => e.Money).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.StartDate).HasColumnType("smalldatetime");

                entity.HasOne(d => d.TransactionNoNavigation)
                    .WithMany(p => p.Punishments)
                    .HasForeignKey(d => d.TransactionNo)
                    .HasConstraintName("FK_Punishments_Transactions");

                entity.HasOne(d => d.UserNoNavigation)
                    .WithMany(p => p.Punishments)
                    .HasForeignKey(d => d.UserNo)
                    .HasConstraintName("FK_Punishments_Users");
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.Property(e => e.InsertTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.InsertedBy)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('Anonymous')");

                entity.Property(e => e.PurchaseDate).HasColumnType("smalldatetime");

                entity.Property(e => e.ReturnDate).HasColumnType("smalldatetime");

                entity.Property(e => e.UserReturnBook).HasColumnType("smalldatetime");

                entity.HasOne(d => d.BookNoNavigation)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.BookNo)
                    .HasConstraintName("FK_Transactions_Books");

                entity.HasOne(d => d.EmployeeNoNavigation)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.EmployeeNo)
                    .HasConstraintName("FK_Transactions_Employees");

                entity.HasOne(d => d.UserNoNavigation)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.UserNo)
                    .HasConstraintName("FK_Transactions_Users");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.FirstName).HasMaxLength(20);

                entity.Property(e => e.InsertTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.InsertedBy)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('Anonymous')");

                entity.Property(e => e.LastName).HasMaxLength(20);

                entity.Property(e => e.Mail).HasMaxLength(50);

                entity.Property(e => e.Password).HasMaxLength(20);

                entity.Property(e => e.Phone).HasMaxLength(20);

                entity.Property(e => e.Picture).HasMaxLength(250);

                entity.Property(e => e.School).HasMaxLength(100);

                entity.Property(e => e.UserName).HasMaxLength(20);
            });

            modelBuilder.Entity<Writer>(entity =>
            {
                entity.Property(e => e.Detail).HasMaxLength(150);

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.InsertTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.InsertedBy)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('Anonymous')");

                entity.Property(e => e.LastName).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
