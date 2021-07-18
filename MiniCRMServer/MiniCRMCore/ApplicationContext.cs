using Microsoft.EntityFrameworkCore;
using MiniCRMCore.Areas.Auth.Models;

namespace MiniCRMCore
{
	public class ApplicationContext : DbContext
	{
		public ApplicationContext(DbContextOptions options) : base(options)
		{
		}

		public DbSet<User> Users { get; set; }
	}
}