using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Intranet.Models.Shop;
using Intranet.Models.CMS;

namespace Intranet.Data
{
    public class IntranetContext : DbContext
    {
        public IntranetContext (DbContextOptions<IntranetContext> options)
            : base(options)
        {
        }

        public DbSet<Intranet.Models.Shop.Product> Product { get; set; } = default!;
        public DbSet<Intranet.Models.CMS.Category> Category { get; set; } = default!;
        public DbSet<Intranet.Models.CMS.ProductDetails> ProductDetails { get; set; } = default!;
    }
}
