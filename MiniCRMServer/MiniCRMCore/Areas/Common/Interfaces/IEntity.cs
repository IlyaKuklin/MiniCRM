using System;
using System.ComponentModel.DataAnnotations;

namespace MiniCRMCore.Areas.Common.Interfaces
{
	internal interface IEntity
	{
		[Key]
		int Id { get; set; }

		DateTime Created { get; set; }
		DateTime Changed { get; set; }
	}
}