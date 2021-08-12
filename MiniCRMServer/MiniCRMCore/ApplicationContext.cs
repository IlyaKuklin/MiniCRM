using Microsoft.EntityFrameworkCore;
using MiniCRMCore.Areas.Auth.Models;
using MiniCRMCore.Areas.Clients.Models;
using MiniCRMCore.Areas.Common;
using MiniCRMCore.Areas.Offers.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MiniCRMCore
{
	public class ApplicationContext : DbContext
	{
		public ApplicationContext(DbContextOptions options) : base(options)
		{
		}

		public DbSet<User> Users { get; set; }

		//public DbSet<FileDatum> FileData { get; set; }

		public DbSet<Client> Clients { get; set; }
		public DbSet<ClientCommunicationReport> ClientCommunicationReports { get; set; }

		public DbSet<Offer> Offers { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Client>(e =>
			{
				e.HasKey(x => x.Id);
				e.HasMany(x => x.Offers).WithOne(x => x.Client).OnDelete(DeleteBehavior.Cascade);
				e.HasMany(x => x.CommunicationReports).WithOne(x => x.Client).OnDelete(DeleteBehavior.Cascade);
			});

			modelBuilder.Entity<Offer>(e =>
			{
				e.HasKey(x => x.Id);
			});
		}

		#region override

		/// <summary>
		/// Синхронное внесение изменение в БД.
		/// </summary>
		/// <returns>The number of state entries written to the database.</returns>
		public override int SaveChanges()
		{
			this.UpdateEntities();
			return base.SaveChanges();
		}

		/// <summary>
		/// Синхронное внесение изменение в БД.
		/// Indicates whether Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.AcceptAllChanges
		///  is called after the changes have been sent successfully to the database.
		/// </summary>
		/// <param name="acceptAllChangesOnSuccess"></param>
		/// <returns>The number of state entries written to the database.</returns>
		public override int SaveChanges(bool acceptAllChangesOnSuccess)
		{
			this.UpdateEntities();
			return base.SaveChanges(acceptAllChangesOnSuccess);
		}

		/// <summary>
		/// Асинхронное внесение изменение в БД.
		/// </summary>
		/// <param name="cancellationToken"></param>
		/// <returns>A task that represents the asynchronous save operation. The task result contains the number of state entries written to the database.</returns>
		public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			this.UpdateEntities();
			return base.SaveChangesAsync(cancellationToken);
		}

		/// <summary>
		/// Асинхронное внесение изменение в БД.
		/// </summary>
		/// <param name="acceptAllChangesOnSuccess"></param>
		/// <param name="cancellationToken"></param>
		/// <returns>A task that represents the asynchronous save operation. The task result contains the number of state entries written to the database.</returns>
		public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
		{
			this.UpdateEntities();
			return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
		}

		private void UpdateEntities()
		{
			var modifiedEntries = this.ChangeTracker.Entries()
				.Where(x => x.Entity is BaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));

			foreach (var entry in modifiedEntries)
			{
				var entity = (BaseEntity)entry.Entity;

				var now = DateTime.UtcNow;
				if (entry.State == EntityState.Added)
					entity.Created = now;
				else
					base.Entry(entity).Property(x => x.Created).IsModified = false;
				entity.Changed = now;
			}
		}

		#endregion override
	}
}