using GymSystem.BLL.Services.Interfaces;
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
    }
}
