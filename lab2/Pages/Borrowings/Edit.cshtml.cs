using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using lab2.Data;
using lab2.models;

namespace lab2.Pages.Borrowings
{
    public class EditModel : PageModel
    {
        private readonly lab2.Data.lab2Context _context;

        public EditModel(lab2.Data.lab2Context context)
        {
            _context = context;
        }

        [BindProperty]
        public Borrowing Borrowing { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var borrowing =  await _context.Borrowing
                .Include(b => b.Member)
                .Include(b => b.Book).ThenInclude(bk => bk.Author)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (borrowing == null)
            {
                return NotFound();
            }
            Borrowing = borrowing;

            // Member select list: show FullName
            var members = await _context.Member
                .Select(m => new { m.ID, Display = (m.FirstName ?? "") + " " + (m.LastName ?? "") })
                .ToListAsync();
            ViewData["MemberID"] = new SelectList(members, "ID", "Display", Borrowing.MemberID);

            // Book select list: show "Title - AuthorFullName"
            var books = await _context.Book
                .Include(b => b.Author)
                .Select(b => new { b.ID, Display = b.Title + (b.Author != null ? " - " + (b.Author.FirstName ?? "") + " " + (b.Author.LastName ?? "") : "") })
                .ToListAsync();
            ViewData["BookID"] = new SelectList(books, "ID", "Display", Borrowing.BookID);

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                // Re-populate select lists before returning page
                var members = await _context.Member
                    .Select(m => new { m.ID, Display = (m.FirstName ?? "") + " " + (m.LastName ?? "") })
                    .ToListAsync();
                ViewData["MemberID"] = new SelectList(members, "ID", "Display", Borrowing.MemberID);

                var books = await _context.Book
                    .Include(b => b.Author)
                    .Select(b => new { b.ID, Display = b.Title + (b.Author != null ? " - " + (b.Author.FirstName ?? "") + " " + (b.Author.LastName ?? "") : "") })
                    .ToListAsync();
                ViewData["BookID"] = new SelectList(books, "ID", "Display", Borrowing.BookID);

                return Page();
            }

            _context.Attach(Borrowing).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BorrowingExists(Borrowing.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool BorrowingExists(int id)
        {
            return _context.Borrowing.Any(e => e.ID == id);
        }
    }
}
