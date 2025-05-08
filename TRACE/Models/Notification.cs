namespace TRACE.Models
{
    public class Notification
    {
        public int NotificationID { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string RecipientUserID { get; set; }
        public long CaseID { get; set; }
        public bool IsRead { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? ReadAt { get; set; }
        public string NotificationType { get; set; }
    }
}
