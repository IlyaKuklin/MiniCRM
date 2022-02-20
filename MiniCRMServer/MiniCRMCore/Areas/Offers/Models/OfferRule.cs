using MiniCRMCore.Areas.Common;
using System;
using System.ComponentModel.DataAnnotations;

namespace MiniCRMCore.Areas.Offers.Models
{
	public class OfferRule : BaseEntity
	{
		public Offer Offer { get; set; }
		public int OfferId { get; set; }

		public bool Completed { get; set; }
		public string Task { get; set; }
		public string Report { get; set; }
		public DateTime Deadline { get; set; }
        public OfferCheckStatus CheckStatus { get; set; }
        public string CheckRemark { get; set; }

        public class Dto : BaseDto
		{
			public int OfferId { get; set; }
			
			[Required]
			public bool Completed { get; set; }
			public string Task { get; set; }
			public string Report { get; set; }
			public DateTime Deadline { get; set; }
			public OfferCheckStatus CheckStatus { get; set; }
			public string CheckRemark { get; set; }
		}

		public class CompleteDto
		{
			public int Id { get; set; }
			public string Report { get; set; }
		}

		public class RejectDto
        {
            public int Id { get; set; }
            public string Remarks { get; set; }
        }
	}
}