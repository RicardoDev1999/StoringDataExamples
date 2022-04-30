using Microsoft.EntityFrameworkCore;
using StoringDataExamples.Business.Interfaces;
using StoringDataExamples.Business.Models;
using StoringDataExamples.Business.SQLStorage.Persistence;
using StoringDataExamples.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoringDataExamples.Business.SQLStorage.Services
{
    public class SQLRepositoryService : IRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public SQLRepositoryService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<BookDTO> CreateAsync(BookDTO book)
        {
            var newBook = new Book()
            {
                Name = book.Name,
                Author = book.Author
            };

            _dbContext.Books.Add(newBook);
            await _dbContext.SaveChangesAsync();

            return new BookDTO { Id = newBook.Id, Name = newBook.Name, Author = newBook.Author };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var book = await _dbContext.Books.FindAsync(id);

            if (book == null)
                return false;

            _dbContext.Remove(book);
            return (await _dbContext.SaveChangesAsync()) > 0;
        }

        public async Task<IEnumerable<BookDTO>> GetAllAsync()
        {
            var books = await _dbContext.Books
                .Select(x => new BookDTO()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Author = x.Author
                }).ToListAsync();

            return books;
        }

        public async Task<BookDTO> GetAsync(int id)
        {
            var book = await _dbContext.Books.FindAsync(id);

            if(book == null) 
                return null;

            return new BookDTO() { 
                Id = book.Id,
                Name= book.Name,
                Author = book.Author
            };
        }

        public async Task<BookDTO> PutAsync(int id, BookDTO book)
        {
            var dbBook = await _dbContext.Books.FindAsync(id);

            if (book == null)
                return null;

            dbBook.Name = book.Name;
            dbBook.Author = book.Author;

            await _dbContext.SaveChangesAsync();

            return book;
        }
    }
}
