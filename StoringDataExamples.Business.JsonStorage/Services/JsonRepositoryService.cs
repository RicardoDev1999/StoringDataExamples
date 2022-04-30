using StoringDataExamples.Business.Interfaces;
using StoringDataExamples.Business.Models;
using StoringDataExamples.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoringDataExamples.Business.JsonStorage.Services
{
    public class JsonRepositoryService : IRepository
    {
        private readonly JsonRepository _jsonRepository;

        public JsonRepositoryService(JsonRepository jsonRepository)
        {
            _jsonRepository = jsonRepository;
        }

        public async Task<BookDTO> CreateAsync(BookDTO book)
        {
            var booksList = _jsonRepository.Books;

            if (booksList == null) return null;

            var calculatedId = booksList.Any() ? booksList.Max(x => x.Id) + 1 : 0;

            var newBook = new Book()
            {
                Id = calculatedId,
                Name = book.Name,
                Author = book.Author
            };

            booksList?.Add(newBook);

            await _jsonRepository.SaveChangesAsync();

            return new BookDTO()
            {
                Id = newBook.Id,
                Name = newBook.Name,
                Author = newBook.Author
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var booksList = _jsonRepository.Books;
            var book = booksList.FirstOrDefault(b => b.Id == id);

            if (book == null)
                return false;

            booksList.Remove(book);

            await _jsonRepository.SaveChangesAsync();

            return true;
        }

        public async Task<BookDTO> GetAsync(int id)
        {
            var booksList = _jsonRepository.Books;

            var book = booksList.FirstOrDefault(b => b.Id == id);

            if (book == null) return null;

            return await Task.FromResult(new BookDTO()
            {
                Id = book.Id,
                Name = book.Name,
                Author = book.Author
            });
        }

        public async Task<IEnumerable<BookDTO>> GetAllAsync()
        {
            return await Task.FromResult(_jsonRepository.Books.Select(x => new BookDTO()
            {
                Id = x.Id,
                Name = x.Name,
                Author = x.Author
            }));
        }

        public async Task<BookDTO> PutAsync(int id, BookDTO book)
        {
            var booksList = _jsonRepository.Books;
            var dbBook = booksList.FirstOrDefault(b => b.Id == id);

            if (dbBook == null)
                return null;

            dbBook.Name = book.Name;
            dbBook.Author = book.Author;

            return await Task.FromResult(new BookDTO()
            {
                Id = book.Id,
                Name = book.Name,
                Author = book.Author
            });
        }
    }
}
