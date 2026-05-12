using GymSystem.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace GymSystem.Controllers
{
    public class PlanController : Controller
    {

        private readonly GymDbContext dbContext = new GymDbContext();
        public async Task<IActionResult> Index()
        {
            var plans = await dbContext.Plans.ToListAsync();

            return View(plans);
        }

        public async Task<IActionResult> Details(int id)
        {
            var plan = await dbContext.Plans.FirstOrDefaultAsync(p => p.Id == id);

            if (plan == null)
               return  RedirectToAction(nameof(Index));
            else
                return View(plan);
         }   
}
    }
