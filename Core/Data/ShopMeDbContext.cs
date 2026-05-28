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
        public DbSet<ArticleCategory> ArticleCategory { get; set; } = default!;
        public DbSet<Article> Article { get; set; } = default!;
        public DbSet<ArticleTag> ArticleTag { get; set; } = default!;
        public DbSet<ArticleTagAssignment> ArticleTagAssignment { get; set; } = default!;
        public DbSet<Ranking> Ranking { get; set; } = default!;
        public DbSet<RankingItem> RankingItem { get; set; } = default!;
        public DbSet<Page> Page { get; set; } = default!;
        public DbSet<PageSection> PageSection { get; set; } = default!;

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

            modelBuilder.Entity<ArticleTagAssignment>()
                .HasKey(ata => new { ata.ArticleId, ata.ArticleTagId });

            modelBuilder.Entity<ArticleTagAssignment>()
                .HasOne(ata => ata.Article)
                .WithMany(a => a.ArticleTagAssignments)
                .HasForeignKey(ata => ata.ArticleId);

            modelBuilder.Entity<ArticleTagAssignment>()
                .HasOne(ata => ata.ArticleTag)
                .WithMany(at => at.ArticleTagAssignments)
                .HasForeignKey(ata => ata.ArticleTagId);

            modelBuilder.Entity<Article>()
                .HasOne(a => a.ArticleCategory)
                .WithMany(ac => ac.Articles)
                .HasForeignKey(a => a.ArticleCategoryId);

            modelBuilder.Entity<RankingItem>()
                .HasOne(ri => ri.Ranking)
                .WithMany(r => r.RankingItems)
                .HasForeignKey(ri => ri.RankingId);

            modelBuilder.Entity<PageSection>()
                .HasOne(ps => ps.Page)
                .WithMany(p => p.PageSections)
                .HasForeignKey(ps => ps.PageId);

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
            modelBuilder.Entity<ArticleCategory>().HasQueryFilter(ac => ac.IsActive);
            modelBuilder.Entity<Article>().HasQueryFilter(a => a.IsActive);
            modelBuilder.Entity<ArticleTag>().HasQueryFilter(at => at.IsActive);
            modelBuilder.Entity<ArticleTagAssignment>().HasQueryFilter(ata => ata.IsActive);
            modelBuilder.Entity<Ranking>().HasQueryFilter(r => r.IsActive);
            modelBuilder.Entity<RankingItem>().HasQueryFilter(ri => ri.IsActive);
            modelBuilder.Entity<Page>().HasQueryFilter(p => p.IsActive);
            modelBuilder.Entity<PageSection>().HasQueryFilter(ps => ps.IsActive);

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

            foreach (var entry in ChangeTracker.Entries<ArticleCategory>().Where(e => e.State == EntityState.Deleted))
            {
                entry.State = EntityState.Modified;
                entry.Entity.IsActive = false;
            }

            foreach (var entry in ChangeTracker.Entries<Article>().Where(e => e.State == EntityState.Deleted))
            {
                entry.State = EntityState.Modified;
                entry.Entity.IsActive = false;
            }

            foreach (var entry in ChangeTracker.Entries<ArticleTag>().Where(e => e.State == EntityState.Deleted))
            {
                entry.State = EntityState.Modified;
                entry.Entity.IsActive = false;
            }

            foreach (var entry in ChangeTracker.Entries<ArticleTagAssignment>().Where(e => e.State == EntityState.Deleted))
            {
                entry.State = EntityState.Modified;
                entry.Entity.IsActive = false;
            }

            foreach (var entry in ChangeTracker.Entries<Ranking>().Where(e => e.State == EntityState.Deleted))
            {
                entry.State = EntityState.Modified;
                entry.Entity.IsActive = false;
            }

            foreach (var entry in ChangeTracker.Entries<RankingItem>().Where(e => e.State == EntityState.Deleted))
            {
                entry.State = EntityState.Modified;
                entry.Entity.IsActive = false;
            }

            foreach (var entry in ChangeTracker.Entries<Page>().Where(e => e.State == EntityState.Deleted))
            {
                entry.State = EntityState.Modified;
                entry.Entity.IsActive = false;
            }

            foreach (var entry in ChangeTracker.Entries<PageSection>().Where(e => e.State == EntityState.Deleted))
            {
                entry.State = EntityState.Modified;
                entry.Entity.IsActive = false;
            }
        }
    }
}
