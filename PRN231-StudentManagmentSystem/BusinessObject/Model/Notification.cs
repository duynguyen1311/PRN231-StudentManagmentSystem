namespace BusinessObject.Model
{
    public class Notification : BaseEntity<Guid>
    {
        public string Title { get; set; }
        public string? SubTitle { get; set; }
        public string Content { get; set; }
        public bool? Status { get; set; }
        public string? Link { get; set; }
        public bool? IsRead { get; set; }
        
    }
}
