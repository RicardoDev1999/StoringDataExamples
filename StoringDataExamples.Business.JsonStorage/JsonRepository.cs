using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StoringDataExamples.Business.Models;
using StoringDataExamples.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoringDataExamples.Business.JsonStorage
{
    public class JsonRepository
    {
        public List<Book> Books { get; set; } = new List<Book>();

        private readonly string path;

        public JsonRepository(IConfiguration configuration)
        {
            path = configuration["jsonStorage:path"];

            if (!File.Exists(path))
                File.Create(path).Close();

            var booksList = new List<Book>();

            try
            {
                booksList = JsonFileHelper.ReadAsync<List<Book>>(path).GetAwaiter().GetResult();
            }
            catch (System.Text.Json.JsonException e)
            {
                Books = new List<Book>();
            }

            Books = booksList ?? new List<Book>();
        }

        public async Task SaveChangesAsync()
        {
            await JsonFileHelper.WriteAsync(path, Books);
        }
    }
}
