using Microsoft.EntityFrameworkCore;
using TestTask.Data;
using TestTask.Models;
using TestTask.Services.Interfaces;

namespace TestTask.Services.Implementations
{
    /// <summary>
    /// Implementation of the IBookService interface.
    /// Provides methods to interact with book data.
    /// </summary>
    public class BookService : IBookService
    {
        private readonly ApplicationDbContext _context;
        private readonly DateTime _carolusRexDate = new DateTime(2012, 5, 25);

        /// <summary>
        /// Initializes a new instance of the <see cref="BookService"/> class.
        /// </summary>
        /// <param name="context">The application database context.</param>
        public BookService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets the book with the highest published cost (price multiplied by quantity published).
        /// </summary>
        /// <returns>The <see cref="Book"/> with the highest published cost, or null if no books are found.</returns>
        public async Task<Book> GetBook()
        {
            return await _context.Books
                .OrderByDescending(b => b.Price * b.QuantityPublished)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Receives a list of books whose title contains the word "Red" and 
        /// which were published after the release date of the album "Carolus Rex" by the Sabaton group.
        /// </summary>
        /// <returns>A list of <see cref="Book"/> objects.</returns>
        public async Task<List<Book>> GetBooks()
        {
            return await _context.Books
                .Where(b => b.Title.Contains("Red") && b.PublishDate > _carolusRexDate)
                .ToListAsync();
        }
    }
}
