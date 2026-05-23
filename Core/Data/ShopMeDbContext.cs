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

            base.OnModelCreating(modelBuilder);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(ISoftDeletable).IsAssignableFrom(entityType.ClrType))
                {
                    modelBuilder.Entity(entityType.ClrType)
                        .HasQueryFilter(CreateSoftDeleteFilter(entityType.ClrType));
                }
            }
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
            foreach (var entry in ChangeTracker.Entries<ISoftDeletable>().Where(e => e.State == EntityState.Deleted))
            {
                entry.State = EntityState.Modified;
                entry.Entity.IsActive = false;
            }
        }

        private static System.Linq.Expressions.LambdaExpression CreateSoftDeleteFilter(Type entityType)
        {
            var parameter = System.Linq.Expressions.Expression.Parameter(entityType, "entity");
            var property = System.Linq.Expressions.Expression.Property(parameter, nameof(ISoftDeletable.IsActive));
            var body = System.Linq.Expressions.Expression.Equal(
                property,
                System.Linq.Expressions.Expression.Constant(true));

            return System.Linq.Expressions.Expression.Lambda(body, parameter);
        }
    }
}
