namespace TRACE.Models
{
    public class HearingsInHearingType
    {
        public long HearingId { get; set; }
        public long HearingTypeId { get; set; }

        public virtual Hearing Hearing { get; set; } = null!;
        public virtual HearingType HearingType { get; set; } = null!;
    }
}
