using Microsoft.Extensions.Configuration;
using StoringDataExamples.Business.Models;
using StoringDataExamples.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoringDataExamples.Business.TextFileStorage
{
    public class TextFileRepository
    {
        public List<Book> Books { get; set; } = new List<Book>();

        private readonly string path;

        public TextFileRepository(IConfiguration configuration)
        {
            path = configuration["textFileStorage:path"];

            if (!File.Exists(path))
                File.Create(path).Close();

            string[] lines = File.ReadAllLines(path);
            var booksList = Books.ToList();

            foreach (string line in lines.AsSpan())
            {
                var splitLine = line.Split(',');

                int.TryParse(splitLine[0], out int id);

                Book book = new()
                {
                    Id = id,
                    Name = splitLine[1],
                    Author = splitLine[2]
                };

                booksList.Add(book);
            }

            Books = booksList;
        }


        public async Task SaveChangesAsync()
        {
            using StreamWriter file = new(path);
            foreach (var book in Books)
            {
                await file.WriteLineAsync($"{book.Id},{book.Name},{book.Author}");
            }
        }
    }
}
