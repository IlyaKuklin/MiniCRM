using System;
using System.ComponentModel.DataAnnotations;

namespace MiniCRMCore.Areas.Common
{
	public class BaseEntity
	{
		[Key]
		public int Id { get; set; }
		public DateTime Created { get; set; }
		public DateTime Changed { get; set; }

		public class BaseDto
		{
			public int Id { get; set; }
			public DateTime Created { get; set; }
			public DateTime Changed { get; set; }
		}
	}
}