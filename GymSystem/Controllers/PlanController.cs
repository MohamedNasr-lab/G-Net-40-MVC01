using GymSystem.DAL.Contexts;
using GymSystem.DAL.Entities;
using GymSystem.DAL.Repositories.Classes;
using GymSystem.DAL.Repositories.Interfaces;
using GymSystemG03.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace GymSystem.Controllers
{
    public class PlanController : Controller
    {

        private readonly IGenericRepository<Plan> planRepository;

        public PlanController(IGenericRepository<Plan> _planRepository )
        {
            planRepository = _planRepository;

        }
   
        public async Task<IActionResult> Index(CancellationToken token)
        {
            var plans = await planRepository.GetAll(false, token);

            return View(plans);
        }

        public async Task<IActionResult> Details(int id, CancellationToken token)
        {
            var plan = await planRepository.GetById(id, token);

            if (plan == null)
               return  RedirectToAction(nameof(Index));
            else
                return View(plan);
         }   
}
    }
