using BusinessObject.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using StudentManagingSystem_Client.Services;

namespace StudentManagingSystem_Client.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public List<Notification> listNoti { get; set; }
        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var client = new ClientService(HttpContext);
            listNoti = await client.GetAll<List<Notification>>("/api/Notify/getall");
            return Page();
        }
        public async Task<IActionResult> Details(Guid id)
        {
            // load data for the item with the specified ID
            var client = new ClientService(HttpContext);
            var item = await client.GetDetail<Notification>("/api/Notify/detail",$"?id={id}");

            return Partial("_NotificationPartial", item);
        }
    }
}