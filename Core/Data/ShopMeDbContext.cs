using Core.Models.CMS;
using Microsoft.EntityFrameworkCore;

namespace Core.Data
{
    public class ShopMeDbContext(DbContextOptions<ShopMeDbContext> options) : DbContext(options)
    {
        public DbSet<Product> Product { get; set; } = default!;
        public DbSet<Category> Category { get; set; } = default!;
        public DbSet<ProductDetails> ProductDetails { get; set; } = default!;
        public DbSet<Tag> Tag { get; set; } = default!;
        public DbSet<ProductTag> ProductTag { get; set; } = default!;
        public DbSet<User> User { get; set; } = default!;
        public DbSet<AuditLog> AuditLog { get; set; } = default!;
        public DbSet<ProductReview> ProductReview { get; set; } = default!; // Added DbSet for ProductReview

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // many-to-many relationship between Product and Tag
            modelBuilder.Entity<ProductTag>()
                .HasKey(pt => new { pt.ProductId, pt.TagId });

            modelBuilder.Entity<ProductTag>()
                .HasOne(pt => pt.Product)
                .WithMany(p => p.ProductTags)
                .HasForeignKey(pt => pt.ProductId);

            modelBuilder.Entity<ProductTag>()
                .HasOne(pt => pt.Tag)
                .WithMany(t => t.ProductTags)
                .HasForeignKey(pt => pt.TagId);

            // one-to-many relationship between Product and ProductReview
            modelBuilder.Entity<ProductReview>()
                .HasOne(pr => pr.Product)
                .WithMany(p => p.ProductReviews)
                .HasForeignKey(pr => pr.ProductId);

            // Optional: one-to-one but ef did it for me already ...
            // modelBuilder.Entity<Product>()
            //     .HasOne(p => p.ProductDetails)
            //     .WithOne(pd => pd.Product)
            //     .HasForeignKey<ProductDetails>(pd => pd.ProductId);

            // auto add to all queries
            modelBuilder.Entity<Product>().HasQueryFilter(p => p.IsActive);
            modelBuilder.Entity<Category>().HasQueryFilter(c => c.IsActive);
            modelBuilder.Entity<ProductDetails>().HasQueryFilter(pd => pd.IsActive);
            modelBuilder.Entity<Tag>().HasQueryFilter(t => t.IsActive);
            modelBuilder.Entity<ProductTag>().HasQueryFilter(pt => pt.IsActive);
            modelBuilder.Entity<User>().HasQueryFilter(u => u.IsActive);
            modelBuilder.Entity<AuditLog>().HasQueryFilter(a => a.IsActive);
            modelBuilder.Entity<ProductReview>().HasQueryFilter(pr => pr.IsActive);

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            ApplySoftDelete();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplySoftDelete();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void ApplySoftDelete()
        {
            foreach (var entry in ChangeTracker.Entries<Product>().Where(e => e.State == EntityState.Deleted))
            {
                entry.State = EntityState.Modified;
                entry.Entity.IsActive = false;
            }

            foreach (var entry in ChangeTracker.Entries<Category>().Where(e => e.State == EntityState.Deleted))
            {
                entry.State = EntityState.Modified;
                entry.Entity.IsActive = false;
            }

            foreach (var entry in ChangeTracker.Entries<ProductDetails>().Where(e => e.State == EntityState.Deleted))
            {
                entry.State = EntityState.Modified;
                entry.Entity.IsActive = false;
            }

            foreach (var entry in ChangeTracker.Entries<Tag>().Where(e => e.State == EntityState.Deleted))
            {
                entry.State = EntityState.Modified;
                entry.Entity.IsActive = false;
            }

            foreach (var entry in ChangeTracker.Entries<ProductTag>().Where(e => e.State == EntityState.Deleted))
            {
                entry.State = EntityState.Modified;
                entry.Entity.IsActive = false;
            }

            foreach (var entry in ChangeTracker.Entries<User>().Where(e => e.State == EntityState.Deleted))
            {
                entry.State = EntityState.Modified;
                entry.Entity.IsActive = false;
            }

            foreach (var entry in ChangeTracker.Entries<AuditLog>().Where(e => e.State == EntityState.Deleted))
            {
                entry.State = EntityState.Modified;
                entry.Entity.IsActive = false;
            }

            foreach (var entry in ChangeTracker.Entries<ProductReview>().Where(e => e.State == EntityState.Deleted))
            {
                entry.State = EntityState.Modified;
                entry.Entity.IsActive = false;
            }
        }
    }
}
