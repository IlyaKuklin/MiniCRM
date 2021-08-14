using MiniCRMCore.Areas.Common;

namespace MiniCRMCore.Areas.Offers.Models
{
	public class OfferFileDatum : BaseEntity
	{
		public FileDatum FileDatum { get; set; }
		public int FileDatumId { get; set; }

		public Offer Offer { get; set; }
		public int OfferId { get; set; }

		public OfferFileType Type { get; set; }

		public class Dto : BaseDto
		{
			public string Path { get; set; }
			public string Name { get; set; }
			public OfferFileType Type { get; set; }
		}
	}
}