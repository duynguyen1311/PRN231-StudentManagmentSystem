using AutoMapper;
using BusinessObject.Model;
using BusinessObject.Utility;
using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentManagingSystem_Client.Services;
using StudentManagingSystem_Client.ViewModel;
using System.Data;

namespace StudentManagingSystem_Client.Pages.SubjectPage
{
    [Authorize(Roles = RoleConstant.ADMIN)]
    public class AddSubjectModel : PageModel
    {
       
        [BindProperty]
        public SubjectAddRequest SubjectAddRequest { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var client = new ClientService(HttpContext);
                await client.PostAdd("/api/Subject/add", SubjectAddRequest);
                return RedirectToPage("/SubjectPage/Subject");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToPage("/Error");
            }
        }
    }
}
