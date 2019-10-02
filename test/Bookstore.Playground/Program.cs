using ConsoleDump;
using Rhetos.Configuration.Autofac;
using Rhetos.Dom.DefaultConcepts;
using Rhetos.Logging;
using Rhetos.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Bookstore.Playground
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleLogger.MinLevel = EventType.Info; // Use "Trace" for more detailed log.
            var rhetosServerPath = @"..\..\..\..\dist\RhetosRadionicaRhetosServer";
            Directory.SetCurrentDirectory(rhetosServerPath);
            // Set commitChanges parameter to COMMIT or ROLLBACK the data changes.
            using (var container = new RhetosTestContainer(commitChanges: true))
            {
                var context = container.Resolve<Common.ExecutionContext>();
                var repository = context.Repository;

                var booksLoad = repository.Bookstore.Book.Load(b => b.Title.Contains("curiosity"));
                booksLoad.Dump();

                var booksQueryString = repository.Bookstore.Book.Query(b => b.Title.Contains("curiosity")).ToString();
                booksQueryString.Dump();

                var filter = new Bookstore.CommonMisspelling();
                //var booksQuery = repository.Bookstore.Book.Query(filter).ToSimple();
                //booksQuery.Dump();

                var id = new Guid("77AC7BF5-DBCE-401C-BE0C-B8146A79EB7D");
                repository.Bookstore.Book.Query(new[] { id}).ToSimple().Dump();

                repository.Bookstore.Book.Insert(new[] { new Book { NumberOfPages = 100, Title = "Insert preko konzole", Code = "B+++" } });
                var existingBook=repository.Bookstore.Book.Query(new[] { id }).Single();
                existingBook.Title = "Editovana knjiga";
                repository.Bookstore.Book.Update(existingBook);

                var booksForRemoval = repository.Bookstore.Book.Query(b => b.Title == "Insert preko konzole");
                repository.Bookstore.Book.Delete(booksForRemoval);

                Console.WriteLine("Done.");
            }
        }
    }
}
