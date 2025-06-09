namespace TRACE.Models
{
    public class DistributionUtility
    {
        public long DuId { get; set; }

        public string DuName { get; set; } = null!;

        public string AddressLine1 { get; set; } = null!;

        public string? AddressLine2 { get; set; }

        public long? CityId { get; set; }

        public string? ZipCode { get; set; }

        public long EntityCategoryId { get; set; }

        public string? ShortName { get; set; }

        public string? Region { get; set; }
    }
}
