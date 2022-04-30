using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StoringDataExamples.Business.Interfaces;
using StoringDataExamples.Business.Models;
using StoringDataExamples.Business.Enums;

#region StorageDI
using StoringDataExamples.Business.JsonStorage;
using StoringDataExamples.Business.TextFileStorage;
using StoringDataExamples.Business.SQLStorage;
#endregion

List<BookDTO> books = new List<BookDTO>();

var hostBuilder = CreateHostBuilder(args);
var host = hostBuilder.Build();

static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(app =>
            {
                app.AddJsonFile("appsettings.json");
            })
            .ConfigureServices((hostContext, services) =>
            {
                switch (GetStorageType(args))
                {
                    case "text":
                        services.UseTextFileStorage();
                        break;
                    case "json":
                        services.UseJsonStorage();
                        break;
                    case "sql":
                        services.UseSQLStorage(hostContext.Configuration);
                        break;
                }
            });

var repository = host.Services.GetRequiredService<IRepository>();

var exitProgram = false;

while (!exitProgram)
{
    Console.Clear();

    Console.ForegroundColor = ConsoleColor.Green;

    switch (GetStorageType(args))
    {
        case "text":
            Console.WriteLine("SYSTEM: USING TEXT STORAGE"); 
            break;
        case "json":
            Console.WriteLine("SYSTEM: USING JSON STORAGE");
            break;
        case "sql":
            Console.WriteLine("SYSTEM: USING SQL STORAGE");
            break;
    }

    Console.ResetColor();
    Console.WriteLine();

    Console.WriteLine(" --- Hello good sir, please choose an option... --- ");
    Console.WriteLine();
    Console.WriteLine(" ----------------------");
    Console.WriteLine(" --- 1. List Books  --- ");
    Console.WriteLine(" --- 2. Add Book    --- ");
    Console.WriteLine(" --- 3. Edit Book   --- ");
    Console.WriteLine(" --- 4. Remove Book --- ");
    Console.WriteLine(" ----------------------");
    Console.WriteLine();

    var menuInput = Console.ReadKey();

    switch (menuInput.KeyChar)
    {
        case '1':
            await ShowBooksAsync();
            break;
        case '2':
            await PromptCreateBook();
            break;
        case '3':
            await PromptEditBook();
            break;
        case '4':
            await PromptDeleteBook();
            break;
    }
}

async Task PromptCreateBook()
{
    Console.Clear();
    Console.WriteLine("--- We shall add your book, good sir ---");
    Console.WriteLine();

    BookDTO bookDTO = new BookDTO();

    Console.Write("Name: ");
    bookDTO.Name = Console.ReadLine();

    Console.Write("Author: ");
    bookDTO.Author = Console.ReadLine();

    var newBook = await repository.CreateAsync(bookDTO);

    Console.Clear();

    if (newBook == null)
    {
        Console.WriteLine("I couldn't create your book, good sir!");
        Console.WriteLine("Please try again later.");
        Console.ReadKey();
        return;
    }

    Console.WriteLine("I created your book, good sir!");
    Console.WriteLine();

    Console.WriteLine(" --- Here is your new book, ma lord --- ");
    Console.WriteLine();
    WriteBookToScreen(newBook);
    Console.WriteLine();
    Console.WriteLine(" -------------------------------------- ");
    Console.WriteLine();

    PromptToContinue();
}

async Task PromptEditBook()
{
    bool insertedValidId = false;
    int id = 0;
    BookDTO bookDTO = new BookDTO();

    while (!insertedValidId)
    {
        Console.Clear();
        Console.WriteLine("--- We shall edit your book, good sir ---");
        Console.WriteLine();

        Console.Write("What is the ");

        Console.ForegroundColor = ConsoleColor.White;
        Console.BackgroundColor = ConsoleColor.DarkBlue;
        Console.Write("id");
        Console.ResetColor();

        Console.Write(" of the book you are looking for sir: ");

        var idInput = Console.ReadLine();

        if (!int.TryParse(idInput, out id))
        {
            Console.Clear();
            Console.WriteLine("That is not a valid id sir!");
            PromptToContinue();
            continue;
        }

        bookDTO = await repository.GetAsync(id);

        if (bookDTO == null)
        {
            Console.Clear();
            Console.WriteLine("I couldn't find your book sir!");
            PromptToContinue();
            continue;
        }

        insertedValidId = true;
    }

    Console.Clear();
    Console.WriteLine(" --- I found this book for you, good sir! ---");
    Console.WriteLine();
    WriteBookToScreen(bookDTO!);
    Console.WriteLine();
    Console.WriteLine(" ---------------------------------------------");

    Console.WriteLine();

    Console.Write("Name: ");
    bookDTO!.Name = Console.ReadLine();

    Console.Write("Author: ");
    bookDTO!.Author = Console.ReadLine();

    bookDTO = await repository.PutAsync(id, bookDTO!);

    Console.Clear();

    if (bookDTO == null)
    {
        Console.WriteLine("I couldn't edit your book, good sir!");
        Console.WriteLine("Please try again later.");
        Console.ReadKey();
        return;
    }

    Console.WriteLine("I edited your book, good sir!");
    Console.WriteLine();

    Console.WriteLine(" --- Here is your edited book, ma lord --- ");
    Console.WriteLine();
    WriteBookToScreen(bookDTO);
    Console.WriteLine();
    Console.WriteLine(" -------------------------------------- ");
    Console.WriteLine();

    PromptToContinue();
}

async Task PromptDeleteBook()
{
    bool insertedValidId = false;
    int id = 0;
    BookDTO bookDTO = new BookDTO();

    while (!insertedValidId)
    {
        Console.Clear();
        Console.WriteLine("--- We shall remove your book, good sir ---");
        Console.WriteLine();

        Console.Write("What is the ");

        Console.ForegroundColor = ConsoleColor.White;
        Console.BackgroundColor = ConsoleColor.DarkBlue;
        Console.Write("id");
        Console.ResetColor();

        Console.Write(" of the book you are looking for sir: ");

        var idInput = Console.ReadLine();

        if (!int.TryParse(idInput, out id))
        {
            Console.Clear();
            Console.WriteLine("That is not a valid id sir!");
            PromptToContinue();
            continue;
        }

        bookDTO = await repository.GetAsync(id);

        if (bookDTO == null)
        {
            Console.Clear();
            Console.WriteLine("I couldn't find your book sir!");
            PromptToContinue();
            continue;
        }

        insertedValidId = true;
    }

    Console.Clear();
    Console.WriteLine(" --- I found this book for you, good sir! ---");
    Console.WriteLine();
    WriteBookToScreen(bookDTO!);
    Console.WriteLine();
    Console.WriteLine(" ---------------------------------------------");

    Console.WriteLine();

    Console.Write("Are u sure u want to delete it, sir? (y/n): ");
    var key = Console.ReadKey();

    if (string.Compare(key.KeyChar.ToString(), 'y'.ToString(), true) != 0)
    {
        Console.Clear();
        PromptToContinue();
        return;
    }

    await repository.DeleteAsync(id);

    Console.Clear();
    Console.WriteLine("Your book was deleted, good sir!");

    PromptToContinue();
}

async Task ShowBooksAsync()
{
    Console.Clear();

    books = (await repository.GetAllAsync()).ToList();

    Console.WriteLine("--- My Book Collection ---");
    Console.WriteLine();
    foreach (var book in books)
    {
        WriteBookToScreen(book);
    }
    Console.WriteLine();
    Console.WriteLine("--------------------------");

    Console.WriteLine();
    PromptToContinue();
}

void WriteBookToScreen(BookDTO book)
{
    Console.WriteLine($"Id: {book.Id} | Name: {book.Name} | Author: {book.Author}");
}

void PromptToContinue()
{
    Console.WriteLine("Click any key to continue, good sir...");
    Console.ReadKey();
}

static string GetStorageType(string[] args)
{
    string storageType = args?.FirstOrDefault(x => x.Contains("/storageType")) ?? string.Empty;

    if (!string.IsNullOrEmpty(storageType))
    {
        storageType = storageType.Split("=", StringSplitOptions.TrimEntries).Last();
    }

    return storageType;
}