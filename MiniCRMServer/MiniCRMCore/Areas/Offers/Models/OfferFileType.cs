using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MiniCRMCore.Areas.Offers.Models
{
	public enum OfferFileType
	{
		Undefined = 0,
		NotSet = 1,

		Photo,
		Certificate,
		TechPassport,
		Card
	}

	public enum OfferSectionType
    {
        Undefined = 0,
        NotSet = 1,

		Description,
		SystemType,
		ShortDescription,
		Case,
		Recommendations,
		SimilarCases,
		RestDocuments,
		CoveringLetter,
		TechPassport,
		Certificates,
		BusinessCard
	}
}