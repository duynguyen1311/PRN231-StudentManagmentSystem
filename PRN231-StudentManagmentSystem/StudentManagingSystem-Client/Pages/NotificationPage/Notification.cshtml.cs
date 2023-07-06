using BusinessObject.Model;
using BusinessObject.Utility;
using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentManagingSystem_Client.Services;
using StudentManagingSystem_Client.ViewModel;
using System.Data;

namespace StudentManagingSystem_Client.Pages.NotificationPage
{
    [Authorize(Roles = RoleConstant.ADMIN)]
    public class NotificationModel : PageModel
    {
       
        public PagedList<Notification> ListNoti { get; set; }
        public int PageIndex { get; set; } = 1;
        public int TotalPage { get; set; }
        public async Task<IActionResult> OnGetAsync(int pageIndex, int pagesize)
        {
            var client = new ClientService(HttpContext);
            if (pageIndex == 0) pageIndex = 1;
            PageIndex = pageIndex;
            pagesize = 4;

            var requestModel = new NotifySearchRequest
            {
                page = pageIndex,
                pagesize = pagesize
            };

            ListNoti = await client.PostSearch<PagedList<Notification>>("/api/Notify/search", requestModel);
            TotalPage = (int)(Math.Ceiling(ListNoti.TotalCount / (double)pagesize));
            return Page();
        }
    }
}
