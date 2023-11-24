using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LinkShortener.Models;
using System.Text;
using System.Security.Cryptography;
using System.Diagnostics;
using System.ComponentModel;
using Newtonsoft.Json.Linq;
using System.Security.Policy;

namespace LinkShortener.Controllers
{
    public class LinksController : Controller
    {
        private readonly LinkContext _context;

        public LinksController(LinkContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Links.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var link = await _context.Links
                .FirstOrDefaultAsync(m => m.Id == id);
            if (link == null)
            {
                return NotFound();
            }

            return View(link);
        }

        public IActionResult CreateEdit()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Link link)
        {
            var existingLink = await _context.Links.FirstOrDefaultAsync(i => i.LongUrl == link.LongUrl);
            if (existingLink == null)
            {
                if (ModelState.IsValid)
                {
                    _context.Add(link);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            ModelState.Clear();
            ViewData["CreateError"] = "This url already exists";
            return View("CreateEdit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Id,LongUrl")] Link link)
        {
            var existingLink = await _context.Links.FirstOrDefaultAsync(i => i.LongUrl == link.LongUrl);
            if (existingLink == null)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(link);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!LinkExists(link.Id))
                        {
                            return RedirectToAction(nameof(CreateEdit));
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }
            }
            ModelState.Clear();
            ViewData["EditError"] = "This url already exists";
            return View("CreateEdit");
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var link = await _context.Links
                .FirstOrDefaultAsync(m => m.Id == id);
            if (link == null)
            {
                return NotFound();
            }

            return View(link);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var link = await _context.Links.FindAsync(id);
            if (link != null)
            {
                _context.Links.Remove(link);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<List<Link>> GetLinks()
        {
            return await _context.Links.ToListAsync();
        }


        private bool LinkExists(int id)
        {
            return _context.Links.Any(e => e.Id == id);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Route("{shortUrl}")]
        public async Task<IActionResult> RedirectToLongUrl(string shortUrl)
        {
            var link = await _context.Links.FirstOrDefaultAsync(i => i.ShortUrl == shortUrl);
            if (link != null)
            {
                link.Clicks++;
                await _context.SaveChangesAsync();
                if (link.LongUrl.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                {
                    return Redirect($"{link.LongUrl}");
                }
                else
                {
                    return Redirect($"https://{link.LongUrl}");
                }
            }
            return NotFound();
        }

    }
}
