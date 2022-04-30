using StoringDataExamples.Business.Interfaces;
using StoringDataExamples.Business.Models;
using StoringDataExamples.Data.Models;

namespace StoringDataExamples.Business.TextFileStorage.Services
{
    public class TextFileRepositoryService : IRepository
    {
        private readonly TextFileRepository _textFileRepository;

        public TextFileRepositoryService(TextFileRepository textFileRepository)
        {
            _textFileRepository = textFileRepository;
        }

        public async Task<BookDTO> CreateAsync(BookDTO book)
        {
            var booksList = _textFileRepository.Books;

            if (booksList == null) return null;

            var calculatedId = booksList.Any() ? booksList.Max(x => x.Id) + 1 : 0;

            var newBook = new Book() { 
                Id = calculatedId,
                Name = book.Name,
                Author = book.Author
            };

            booksList?.Add(newBook);

            await _textFileRepository.SaveChangesAsync();

            return new BookDTO()
            {
                Id = newBook.Id,
                Name = newBook.Name,
                Author = newBook.Author
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var booksList = _textFileRepository.Books;
            var book = booksList.FirstOrDefault(b => b.Id == id);

            if (book == null)
                return false;

            booksList.Remove(book);

            await _textFileRepository.SaveChangesAsync();

            return true;
        }

        public async Task<BookDTO> GetAsync(int id)
        {
            var booksList = _textFileRepository.Books;

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
            return await Task.FromResult(_textFileRepository.Books.Select(x => new BookDTO()
            {
                Id = x.Id,
                Name = x.Name,
                Author = x.Author
            }));
        }

        public async Task<BookDTO> PutAsync(int id, BookDTO book)
        {
            var booksList = _textFileRepository.Books;
            var dbBook = booksList.FirstOrDefault(b => b.Id == id);

            if (dbBook == null)
                return null;

            dbBook.Name = book.Name;
            dbBook.Author = book.Author;

            await _textFileRepository.SaveChangesAsync();

            return await Task.FromResult(new BookDTO()
            {
                Id = book.Id,
                Name = book.Name,
                Author = book.Author
            });
        }
    }
}
