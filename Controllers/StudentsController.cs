using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentApp.Data;

namespace StudentApp.Controllers;

[Authorize]
public class StudentsController : Controller
{
    private readonly AppDbContext _db;

    public StudentsController(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index()
    {
        var students = await _db.Students.OrderBy(s => s.FullName).ToListAsync();
        return View(students);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var student = await _db.Students.FindAsync(id);
        if (student != null)
        {
            _db.Students.Remove(student);
            await _db.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}
