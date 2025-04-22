namespace TRACE.Models
{
    public class HearingsInHearingType
    {
        public long HearingID { get; set; }
        public long HearingTypeID { get; set; }

        public Hearing Hearing { get; set; }
        public HearingType HearingType { get; set; }
    }
}