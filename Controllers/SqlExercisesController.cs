using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SqlLearningApp.Models;

namespace SQLLearningApp.Controllers
{
    public class SqlQueryRequest
    {
        public string Query { get; set; } = null!;
    }

    public class SqlExercisesController : Controller
    {
        private readonly SqlLearningAppContext _context;

        public SqlExercisesController(SqlLearningAppContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("api/run-sql-query")]
        public async Task<IActionResult> RunSqlQuery([FromBody] SqlQueryRequest request)
        {
            if (string.IsNullOrEmpty(request.Query))
                return BadRequest(new { success = false, error = "Запит не може бути порожнім." });

            try
            {
                // Отримати завдання з правильним запитом (LessonId = 1 - для уроку 1)
                var exercise = await _context.SqlExercises.FirstOrDefaultAsync(e => e.LessonId == 7);

                if (exercise == null)
                    return NotFound(new { success = false, error = "Завдання для Lesson1 не знайдено." });

                // Виконати правильний запит
                var correctResult = await ExecuteQueryAsync(exercise.CorrectQuery);

                // Виконати запит користувача
                var userResult = await ExecuteQueryAsync(request.Query);

                // Порівняти результати
                bool isCorrect = CompareResults(correctResult, userResult);

                return Ok(new { success = true, isCorrect, result = userResult });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }

        // Метод для виконання запиту та отримання результатів
        private async Task<List<Dictionary<string, object>>> ExecuteQueryAsync(string query)
        {
            var results = new List<Dictionary<string, object>>();

            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = query;
                await _context.Database.OpenConnectionAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var row = new Dictionary<string, object>();

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            row[reader.GetName(i)] = reader.GetValue(i);
                        }

                        results.Add(row);
                    }
                }

                await _context.Database.CloseConnectionAsync();
            }

            return results;
        }

        // Метод для порівняння результатів
        private bool CompareResults(List<Dictionary<string, object>> correctResult, List<Dictionary<string, object>> userResult)
        {
            if (correctResult.Count != userResult.Count)
                return false;

            for (int i = 0; i < correctResult.Count; i++)
            {
                foreach (var key in correctResult[i].Keys)
                {
                    if (!userResult[i].ContainsKey(key) || !Equals(correctResult[i][key], userResult[i][key]))
                        return false;
                }
            }

            return true;
        }


        // GET: SqlExercises
        public async Task<IActionResult> Index()
        {
            var sqlLearningAppContext = _context.SqlExercises.Include(s => s.Lesson);
            return View(await sqlLearningAppContext.ToListAsync());
        }

        // GET: SqlExercises/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sqlExercise = await _context.SqlExercises
                .Include(s => s.Lesson)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sqlExercise == null)
            {
                return NotFound();
            }

            return View(sqlExercise);
        }

        // GET: SqlExercises/Create
        public IActionResult Create()
        {
            ViewData["LessonId"] = new SelectList(_context.Lessons, "Id", "Title");
            return View();
        }

        // POST: SqlExercises/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,CorrectQuery,LessonId")] SqlExercise sqlExercise)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sqlExercise);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LessonId"] = new SelectList(_context.Lessons, "Id", "Title", sqlExercise.LessonId);
            return View(sqlExercise);
        }

        // GET: SqlExercises/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sqlExercise = await _context.SqlExercises.FindAsync(id);
            if (sqlExercise == null)
            {
                return NotFound();
            }
            ViewData["LessonId"] = new SelectList(_context.Lessons, "Id", "Title", sqlExercise.LessonId);
            return View(sqlExercise);
        }

        // POST: SqlExercises/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,CorrectQuery,LessonId")] SqlExercise sqlExercise)
        {
            if (id != sqlExercise.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sqlExercise);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SqlExerciseExists(sqlExercise.Id))
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
            ViewData["LessonId"] = new SelectList(_context.Lessons, "Id", "Title", sqlExercise.LessonId);
            return View(sqlExercise);
        }

        // GET: SqlExercises/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sqlExercise = await _context.SqlExercises
                .Include(s => s.Lesson)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sqlExercise == null)
            {
                return NotFound();
            }

            return View(sqlExercise);
        }

        // POST: SqlExercises/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sqlExercise = await _context.SqlExercises.FindAsync(id);
            if (sqlExercise != null)
            {
                _context.SqlExercises.Remove(sqlExercise);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SqlExerciseExists(int id)
        {
            return _context.SqlExercises.Any(e => e.Id == id);
        }
    }
}
