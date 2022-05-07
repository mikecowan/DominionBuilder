using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DominionBuilder.Data
{
    public partial class DominionContext : DbContext
    {
        public DominionContext()
        {
        }

        public DominionContext(DbContextOptions<DominionContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CardCategories> CardCategories { get; set; }
        public virtual DbSet<CardCategoryLinks> CardCategoryLinks { get; set; }
        public virtual DbSet<CardTypeLinks> CardTypeLinks { get; set; }
        public virtual DbSet<CardTypes> CardTypes { get; set; }
        public virtual DbSet<Cards> Cards { get; set; }
        public virtual DbSet<ExtraCards> ExtraCards { get; set; }
        public virtual DbSet<PeripheralLinks> PeripheralLinks { get; set; }
        public virtual DbSet<Peripherals> Peripherals { get; set; }
        public virtual DbSet<PromoCards> PromoCards { get; set; }
        public virtual DbSet<Sets> Sets { get; set; }
        public virtual DbSet<SpecialCostTypes> SpecialCostTypes { get; set; }
        public virtual DbSet<SpecialCosts> SpecialCosts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:DefaultSchema", "dominion");

            modelBuilder.Entity<CardCategories>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CardCategoryLinks>(entity =>
            {
                entity.HasOne(d => d.Card)
                    .WithMany(p => p.CardCategoryLinks)
                    .HasForeignKey(d => d.CardId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CardCategoryLinks_Cards");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.CardCategoryLinks)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CardCategoryLinks_CardCategories");
            });

            modelBuilder.Entity<CardTypeLinks>(entity =>
            {
                entity.HasOne(d => d.Card)
                    .WithMany(p => p.CardTypeLinks)
                    .HasForeignKey(d => d.CardId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CardTypeLinks_Cards");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.CardTypeLinks)
                    .HasForeignKey(d => d.TypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CardTypeLinks_CardTypes");
            });

            modelBuilder.Entity<CardTypes>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Cards>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Set)
                    .WithMany(p => p.Cards)
                    .HasForeignKey(d => d.SetId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Cards_Sets");
            });

            modelBuilder.Entity<ExtraCards>(entity =>
            {
                entity.HasOne(d => d.ExtraCard)
                    .WithMany(p => p.ExtraCardsExtraCard)
                    .HasForeignKey(d => d.ExtraCardId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ExtraCards_Cards1");

                entity.HasOne(d => d.KingdomCard)
                    .WithMany(p => p.ExtraCardsKingdomCard)
                    .HasForeignKey(d => d.KingdomCardId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ExtraCards_Cards");
            });

            modelBuilder.Entity<PeripheralLinks>(entity =>
            {
                entity.HasOne(d => d.Card)
                    .WithMany(p => p.PeripheralLinks)
                    .HasForeignKey(d => d.CardId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PeripheralLinks_Cards");

                entity.HasOne(d => d.Peripheral)
                    .WithMany(p => p.PeripheralLinks)
                    .HasForeignKey(d => d.PeripheralId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PeripheralLinks_Peripherals");
            });

            modelBuilder.Entity<Peripherals>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PromoCards>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Sets>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SpecialCostTypes>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SpecialCosts>(entity =>
            {
                entity.HasOne(d => d.Card)
                    .WithMany(p => p.SpecialCosts)
                    .HasForeignKey(d => d.CardId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SpecialCosts_Cards");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.SpecialCosts)
                    .HasForeignKey(d => d.TypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SpecialCosts_SpecialCostTypes");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
