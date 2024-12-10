using Microsoft.EntityFrameworkCore;
using TestTask.Data;
using TestTask.Models;
using TestTask.Services.Interfaces;

namespace TestTask.Services.Implementations
{
    /// <summary>
    /// Implementation of the IAuthorService interface.
    /// Provides methods to interact with author data.
    /// </summary>
    public class AuthorService : IAuthorService
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorService"/> class.
        /// </summary>
        /// <param name="context">The application database context.</param>
        public AuthorService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets the author who has written the book with the longest title.
        /// If multiple authors have written books with the same title length,
        /// the author with the smallest ID is returned.
        /// </summary>
        /// <returns>The <see cref="Author"/> with the longest book title, or null if no authors are found.</returns>
        public async Task<Author> GetAuthor()
        {
            int maxLength = await _context.Books.MaxAsync(b => b.Title.Length);

            var author = await _context.Authors
                .Where(a => a.Books.Any(b => b.Title.Length == maxLength))
                .OrderBy(a => a.Id)
                .FirstOrDefaultAsync();

            return author;
        }


        /// <summary>
        /// Gets the list of authors who have written an even number of books
        /// published after the year 2015.
        /// </summary>
        /// <returns>A list of <see cref="Author"/> objects.</returns>
        public async Task<List<Author>> GetAuthors()
        {
            var authorIds = await _context.Books
                .Where(b => b.PublishDate.Year > 2015)
                .GroupBy(b => b.AuthorId)
                .Where(g => g.Count() % 2 == 0)
                .Select(g => g.Key)
                .ToListAsync();
            var authors = await _context.Authors
                .Where(a => authorIds.Contains(a.Id))
                .ToListAsync();
            return authors;
        }
    }
}
