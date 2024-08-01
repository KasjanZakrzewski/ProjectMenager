using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectMenager.Data;
using ProjectMenager.Models;

namespace ProjectMenager.Controllers
{
    [Authorize(Roles = "Administrator, ProjectManager, Employee")]
    public class TasksController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<IdentityUser> _UserManager;

        public TasksController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _UserManager = userManager;
        }

        // GET: Tasks
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Task.Include(t => t.Employee).Include(t => t.Item);
            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> UserTasks()
        {
            var Id = _UserManager.GetUserId(User);
            var applicationDbContext = _context.Task.Include(t => t.Employee).Include(t => t.Item);
            return View(await applicationDbContext.Where(t => t.EmployeeId == Id).ToListAsync());
        }

        // GET: Tasks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Task == null)
            {
                return NotFound();
            }

            var task = await _context.Task
                .Include(t => t.Employee)
                .Include(t => t.Item)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (task == null)
            {
                return NotFound();
            }

            var notes = await _context.Notes
                .Where(m => m.TaskId == id)
                .ToListAsync();
            if (notes == null)
            {
                return NotFound();
            }

            MyModel<Models.Task, List<Notes>> model = new MyModel<Models.Task, List<Notes>>();
            model.Obj1 = task;
            model.Obj2 = notes;

            return View(model);
        }

        // GET: Tasks/Create
        [Authorize(Roles = "Administrator")]
        public IActionResult Create(int? Id)
        {
            ViewData["EmployeeId"] = new SelectList(_context.Users, "Id", "Id");
            if (Id != null)
            {
                ViewData["ItemId"] = new SelectList(_context.Item, "Id", "Id", Id);
            }
            else
            {
                ViewData["ItemId"] = new SelectList(_context.Item, "Id", "Id");
            }
            
            return View();
        }

        // POST: Tasks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create([Bind("Name,Description,Status,ItemId,EmployeeId")] Models.Task task)
        {
            if (ModelState.IsValid)
            {
                _context.Add(task);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmployeeId"] = new SelectList(_context.Users, "Id", "Id", task.EmployeeId);
            ViewData["ItemId"] = new SelectList(_context.Item, "Id", "Id", task.ItemId);
            return View(task);
        }

        // GET: Tasks/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Task == null)
            {
                return NotFound();
            }

            var task = await _context.Task.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }
            ViewData["EmployeeId"] = new SelectList(_context.Users, "Id", "Id", task.EmployeeId);
            ViewData["ItemId"] = new SelectList(_context.Item, "Id", "Id", task.ItemId);
            return View(task);
        }

        // POST: Tasks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Status,ItemId,EmployeeId")] Models.Task task)
        {
            if (id != task.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(task);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskExists(task.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmployeeId"] = new SelectList(_context.Users, "Id", "Id", task.EmployeeId);
            ViewData["ItemId"] = new SelectList(_context.Item, "Id", "Id", task.ItemId);
            return View(task);
        }

        // GET: Tasks/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Task == null)
            {
                return NotFound();
            }

            var task = await _context.Task
                .Include(t => t.Employee)
                .Include(t => t.Item)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        // POST: Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Task == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Task'  is null.");
            }
            var task = await _context.Task.FindAsync(id);
            if (task != null)
            {
                _context.Task.Remove(task);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Tasks/Edit/5
        public async Task<IActionResult> Aprove(int? id)
        {
            if (id == null || _context.Task == null)
            {
                return NotFound();
            }

            var task = await _context.Task.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            if (User.IsInRole("ProjectManager"))
            {
                if(task.Status != Status.Review)
                {
                    // komunikat
                    return RedirectToAction("Details", "Items", new { id = id });
                }
                task.Status = Status.Completed;
            }

            if (User.IsInRole("Employee"))
            {
                if (task.Status != Status.InProgress)
                {
                    // komunikat
                    return RedirectToAction("UserTasks");
                }
                task.Status = Status.Review;
            }

            try
            {
                _context.Update(task);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

            }

            if (User.IsInRole("ProjectManager"))
            {
                return RedirectToAction("Details", "Items", new { id = id });
            }

            if (User.IsInRole("Employee"))
            {
                return RedirectToAction("UserTasks");
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Tasks/Edit/5
        public async Task<IActionResult> Begin(int? id)
        {
            if (id == null || _context.Task == null)
            {
                return NotFound();
            }

            var task = await _context.Task.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            if (task.Status != Status.New)
            {
                // komunikat
                return RedirectToAction("UserTasks");
            }
            task.Status = Status.InProgress;

            try
            {
                _context.Update(task);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

            }

            return RedirectToAction("UserTasks");
        }

        // GET: Tasks/Edit/5
        [Authorize(Roles = "Administrator, ProjectManager")]
        public async Task<IActionResult> Busted(int? id)
        {
            if (id == null || _context.Task == null)
            {
                return NotFound();
            }

            var task = await _context.Task.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            if (task.Status != Status.Review)
            {
                // komunikat
                return RedirectToAction("Details", "Items", new { id = id });
            }
            task.Status = Status.InProgress;

            try
            {
                _context.Update(task);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

            }

            return RedirectToAction("Details", "Items", new { id = id });
        }

        private bool TaskExists(int id)
        {
          return (_context.Task?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
