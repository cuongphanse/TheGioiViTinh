using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TheGioiViTinh.Models.Entities;

namespace TheGioiViTinh.Data
{
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{
		}
		public DbSet<Category> Categories { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<Brand> Brands { get; set; }
		public DbSet<ProductImage> ProductImages { get; set; }
		public DbSet<Discount> Discounts { get; set; }
		public DbSet<ApplicationUser> ApplicationUsers { get; set; }
		public DbSet<ProductDiscount> ProductDiscounts { get; set; }
		public DbSet<ShoppingCart> ShoppingCarts { get; set; }
		public DbSet<OrderHeader> OrderHeaders { get; set; }
		public DbSet<OrderDetails> OrderDetails { get; set; }
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Category>().HasData(
				new Category { Id = 1, Name = "Laptop", DisplayOrder = 1 },
				new Category { Id = 2, Name = "PC", DisplayOrder = 2 },
				new Category { Id = 3, Name = "Điện thoại", DisplayOrder = 3 },
				new Category { Id = 4, Name = "Phụ kiện", DisplayOrder = 4 }
			);

			modelBuilder.Entity<Product>()
				.Property(p => p.Price)
				.HasPrecision(18, 0);

			modelBuilder.Entity<ProductImage>()
				.HasOne(pi => pi.Product)
				.WithMany(p => p.ProductImages)
				.HasForeignKey(pi => pi.ProductId)
				.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<ProductDiscount>()
				.HasKey(pd => new
				{
					pd.ProductId,
					pd.DiscountId
				});

			modelBuilder.Entity<ProductDiscount>()
				.HasOne(pd => pd.Product)
				.WithMany(p => p.ProductDiscounts)
				.HasForeignKey(pd => pd.ProductId);

			modelBuilder.Entity<ProductDiscount>()
				.HasOne(pd => pd.Discount)
				.WithMany(d => d.ProductDiscounts)
				.HasForeignKey(pd => pd.DiscountId);
			modelBuilder.Entity<OrderDetails>()
			.Property(o => o.Price)
			.HasPrecision(18, 0);
		}
	}
}
