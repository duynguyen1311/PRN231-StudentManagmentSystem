using System.Text.Json.Serialization;

namespace StudentManagingSystem_Client.ViewModel
{
    public class NotifySearchRequest
    {
        public int page { get; set; }
        public int pagesize { get; set; }
    }

    public class NotifyAddRequest
    {
        public string Title { get; set; }
        public string? SubTitle { get; set; }
        public string Content { get; set; }
        public bool? Status { get; set; }
        public string? Link { get; set; }
        public bool? IsRead { get; set; }
        [JsonIgnore]
        public string? CreatedBy { get; set; }
        [JsonIgnore]
        public DateTime? CreatedDate { get; set; }
    }
}
