using StoringDataExamples.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoringDataExamples.Business.Interfaces
{
    public interface IRepository
    {
        public Task<BookDTO> GetAsync(int id);
        public Task<IEnumerable<BookDTO>> GetAllAsync();
        public Task<BookDTO> CreateAsync(BookDTO book);
        public Task<BookDTO> PutAsync(int id, BookDTO book);
        public Task<bool> DeleteAsync(int id);
    }
}
