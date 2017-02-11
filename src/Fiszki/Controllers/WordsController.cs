using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Fiszki.Data;
using Fiszki.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace Fiszki.Controllers
{
    public class WordsController : Controller
    {
        private readonly WordContext _context;
        private static int score = 0;
        private static int hitPoints = 3;
        const string SessionKeyScore = "_Score";
        const string SessionKeyHitPoints = "_HitPoints";

        public WordsController(WordContext context)
        {
            _context = context;    
        }

        // GET: Words
        public async Task<IActionResult> Index()
        {
            return View(await _context.Words.ToListAsync());
        }

        public async Task<IActionResult> Fiszki()
        {
            // Convert session to nullable Int32 variable.
            int? number = HttpContext.Session.GetInt32(SessionKeyScore);


            // Convert value from nullable int to int.
            if (number.HasValue) 
                score = number.Value;

            // Convert session to nullable Int32 variable.
            int? numberHP = HttpContext.Session.GetInt32(SessionKeyHitPoints);

            // Convert value from nullable int to int.
            if (numberHP.HasValue)
                hitPoints = numberHP.Value;

            // Check, if hitPoints == 0, then you lose game and program go to this block of code.
            if (hitPoints ==0)
            {

                int tempScore = score;
                score = 0;
                hitPoints = 3;
                HttpContext.Session.SetInt32(SessionKeyScore, score);   // Write local variable value(0) to session.
                HttpContext.Session.SetInt32(SessionKeyHitPoints, hitPoints); // Write local variable value(3) to session.

                // Use REdirectToAction with new object as parametr, that object is used to crete or edit user ranking.
                if (User.Identity.IsAuthenticated)
                {
                    string Text = User.Identity.Name;   // Initialize string that is already logged user Email.
                    return RedirectToAction("CreateRank", "Words", new { Email = Text, Points = tempScore });
                }
                
            }

            //Create ViewBags to transfer value to the view.
            ViewBag.Score = score.ToString();  
            ViewBag.hitPoints = hitPoints.ToString();

            //Return view, paramerer is a list of all objects from database table Words.
            return View(await _context.Words.ToListAsync());
        }


        // Action created to support create new rank value when user lose 3 chances and the game.
        public async Task<IActionResult> CreateRank([Bind("ID,Email,Points")] Rank rank)
        {
            // If model that we get as a parameter is valid, we can add new record to database table.
            if (ModelState.IsValid)
            {
                _context.Add(rank);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Fiszki");
        }


        public ActionResult FiszkiAfterYes()
        {
            // Convert session to nullable Int32 variable.
            int? number = HttpContext.Session.GetInt32(SessionKeyScore);

            // Convert value from nullable int to int.
            if (number.HasValue)
                score = number.Value;

            score++;  // Increment score var value

            // Write local variable value to session.
            HttpContext.Session.SetInt32(SessionKeyScore, score);

            // increment score local variable
            score++;
            //Return action Fiszki in controller Words.
            return RedirectToAction("Fiszki", "Words");
        }
        public ActionResult FiszkiAfterNo()
        {
            // Convert session to nullable Int32 variable.
            int? numberHP = HttpContext.Session.GetInt32(SessionKeyHitPoints);

            // Convert value from nullable int to int.
            if (numberHP.HasValue)
                hitPoints = numberHP.Value;

            hitPoints--;  // Increment score var value

            // Write local variable value(3) to the session.
            HttpContext.Session.SetInt32(SessionKeyHitPoints, hitPoints);

            //Return action Fiszki in controller Words.
            return RedirectToAction("Fiszki", "Words");
        }


        // GET: Words/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var word = await _context.Words.SingleOrDefaultAsync(m => m.ID == id);
            if (word == null)
            {
                return NotFound();
            }

            return View(word);
        }


        // GET: Words/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Words/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,EnglishWord,PolishWord")] Word word)
        {
            if (ModelState.IsValid)
            {
                _context.Add(word);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(word);
        }

        // GET: Words/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var word = await _context.Words.SingleOrDefaultAsync(m => m.ID == id);
            if (word == null)
            {
                return NotFound();
            }
            return View(word);
        }

        // POST: Words/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,EnglishWord,PolishWord")] Word word)
        {
            if (id != word.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(word);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WordExists(word.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(word);
        }

        // GET: Words/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var word = await _context.Words.SingleOrDefaultAsync(m => m.ID == id);
            if (word == null)
            {
                return NotFound();
            }

            return View(word);
        }

        // POST: Words/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var word = await _context.Words.SingleOrDefaultAsync(m => m.ID == id);
            _context.Words.Remove(word);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool WordExists(int id)
        {
            return _context.Words.Any(e => e.ID == id);
        }
    }
}
