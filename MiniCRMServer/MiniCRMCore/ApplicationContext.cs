using Microsoft.EntityFrameworkCore;
using MiniCRMCore.Areas.Auth.Models;
using MiniCRMCore.Areas.Clients.Models;
using MiniCRMCore.Areas.Offers.Models;

namespace MiniCRMCore
{
	public class ApplicationContext : DbContext
	{
		public ApplicationContext(DbContextOptions options) : base(options)
		{
		}

		public DbSet<User> Users { get; set; }

		public DbSet<Client> Clients { get; set; }

		public DbSet<Offer> Offers { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Client>(e =>
			{
				e.HasKey(x => x.Id);
				e.HasMany(x => x.Offers).WithOne(x => x.Client).OnDelete(DeleteBehavior.Cascade);
			});

			modelBuilder.Entity<Offer>(e =>
			{
				e.HasKey(x => x.Id);
			});

		}
	}
}