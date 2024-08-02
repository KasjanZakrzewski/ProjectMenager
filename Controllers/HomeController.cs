using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectMenager.Data;
using ProjectMenager.Models;
using ProjectMenager.Models.DTO;
using System.Diagnostics;

namespace ProjectMenager.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            if (User.IsInRole("Employee"))
            {
                return RedirectToAction("UserTasks", "Tasks");
            }
            if (User.IsInRole("ProjectManager") || User.IsInRole("Administrator"))
            {
                return View();
            }
            return RedirectToAction("Guest");
        }

        public async Task<IActionResult> Guest()
        {
            var project = await _context.Projects
                .Include(p => p.Manager)
                .ToListAsync();
            if (project == null)
            {
                return NotFound();
            }
            List<ProjectDTO> model = new List<ProjectDTO>();
            ProjectDTO temp;
            foreach (var p in project) {
                temp = new ProjectDTO();
                temp.Name = p.Name;
                temp.Status = p.Status;
                temp.StartDate = p.StartDate;
                temp.EndDate = p.EndDate;
                temp.Manager = p.Manager.UserName;
                model.Add(temp);
            }

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}