using GymSystem.BLL.Services.Interfaces;
using GymSystem.BLL.ViewModels.MembersViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymSystem.Controllers
{
    public class MemberController : Controller
    {
        private readonly IMemberServices memberServices;

        public MemberController(IMemberServices memberServices) 
        {
            this.memberServices = memberServices;
        }
        //GET ALL MEMEBERS
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            
            var memebers=await memberServices.GetAllMembersAsync(ct);
            return View(memebers);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Createmember(CreateMemberViewModel model,CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return View(nameof(Create), model);

          var Result=await memberServices.CreateMemberAsync(model, ct);
            if (Result)
                TempData["Success"] = "Member Created Successfully";
            else
                TempData["Failed"] = "Failed to Create Member";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> MemberDetails(int id,CancellationToken ct)
        {
            var member=await memberServices.GetMemberDetailsAsync(id,ct);
            if(member is null)
            {
                TempData["ErrorMessage"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }
        [HttpGet]
        public async Task<IActionResult> HealthRecordDetails(int id, CancellationToken ct)
        {
            var healrecord = await memberServices.GetMemberHealthRecordAsync(id, ct);
            if (healrecord is null)
            {
                TempData["ErrorMessage"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(healrecord);
        }
    }
}
