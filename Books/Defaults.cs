using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELibrary.Books;
using System.Threading;
using ELibrary.Library;

namespace ELibrary.Defaults
{
    public struct Book : IBook
    {
        public BookAuthor BookAuthor { get; set; }

        public BookSatus BookSatus { get; set; }

        public BookType BookType { get; set; }

        public int Id { get; set; }

        public string Name { get; set; }

        IBookAuthor IBook.Author
        {
            get
            {
                return this.BookAuthor;
            }
        }
    }

    public struct BookAuthor : IBookAuthor
    {
        public string FirstName { get; set; }

        public int Id { get; set; }

        public string LastName { get; set; }
    }

    public class BookReposytory : Library.IBookReposytory
    {
        private List<IBook> _collection;

        public BookReposytory()
        {
            _collection = new List<IBook>();
            var a1 = new BookAuthor() { Id = 1, FirstName = "Henry", LastName = "Kuttner" };
            var a2 = new BookAuthor() { Id = 2, FirstName = "Ray", LastName = "Bradbury" };
            var a3 = new BookAuthor() { Id = 3, FirstName = "Howard Phillips", LastName = "Lovecraft" };
            var a4 = new BookAuthor() { Id = 4, FirstName = "Stephen", LastName = "King" };


            _collection.Add(new Book() { Id = 1, BookSatus = BookSatus.InLibrary, BookType = BookType.Book, BookAuthor = a1, Name = "Clash by Night" });
            _collection.Add(new Book() { Id = 2, BookSatus = BookSatus.InLibrary, BookType = BookType.Book, BookAuthor = a1, Name = "Fury" });
            _collection.Add(new Book() { Id = 3, BookSatus = BookSatus.InLibrary, BookType = BookType.Book, BookAuthor = a1, Name = "The World Is Mine" });
            _collection.Add(new Book() { Id = 4, BookSatus = BookSatus.InLibrary, BookType = BookType.Book, BookAuthor = a1, Name = "Time Locker" });
            _collection.Add(new Book() { Id = 5, BookSatus = BookSatus.InLibrary, BookType = BookType.Book, BookAuthor = a1, Name = "The Proud Robot" });
            _collection.Add(new Book() { Id = 6, BookSatus = BookSatus.InLibrary, BookType = BookType.Book, BookAuthor = a1, Name = "Gallegher Plus" });
            _collection.Add(new Book() { Id = 7, BookSatus = BookSatus.InLibrary, BookType = BookType.Book, BookAuthor = a1, Name = "Ex Machina" });
            _collection.Add(new Book() { Id = 8, BookSatus = BookSatus.InLibrary, BookType = BookType.Book, BookAuthor = a1, Name = "Hollywood on the Moon" });
            _collection.Add(new Book() { Id = 9, BookSatus = BookSatus.InLibrary, BookType = BookType.Book, BookAuthor = a1, Name = "The Citadel of Darkness" });
            _collection.Add(new Book() { Id = 10, BookSatus = BookSatus.InLibrary, BookType = BookType.Book, BookAuthor = a1, Name = "Absalom" });

            _collection.Add(new Book() { Id = 11, BookSatus = BookSatus.InLibrary, BookType = BookType.Book, BookAuthor = a2, Name = "Fahrenheit 451" });
            _collection.Add(new Book() { Id = 12, BookSatus = BookSatus.InLibrary, BookType = BookType.Book, BookAuthor = a2, Name = "The Halloween Tree" });
            _collection.Add(new Book() { Id = 13, BookSatus = BookSatus.InLibrary, BookType = BookType.Book, BookAuthor = a2, Name = "The Million Year Picnic" });
            _collection.Add(new Book() { Id = 14, BookSatus = BookSatus.InLibrary, BookType = BookType.Book, BookAuthor = a2, Name = "The Dragon" });
            _collection.Add(new Book() { Id = 15, BookSatus = BookSatus.InLibrary, BookType = BookType.Book, BookAuthor = a2, Name = "The Machineries of Joy" });

            _collection.Add(new Book() { Id = 16, BookSatus = BookSatus.InLibrary, BookType = BookType.Book, BookAuthor = a3, Name = "The Call of Cthulhu" });
            _collection.Add(new Book() { Id = 17, BookSatus = BookSatus.InLibrary, BookType = BookType.Book, BookAuthor = a3, Name = "The Dream-Quest of Unknown Kadath" });
            _collection.Add(new Book() { Id = 18, BookSatus = BookSatus.InLibrary, BookType = BookType.Book, BookAuthor = a3, Name = "Dagon" });
            _collection.Add(new Book() { Id = 19, BookSatus = BookSatus.InLibrary, BookType = BookType.Book, BookAuthor = a3, Name = "The Shadow Out of Time" });
            _collection.Add(new Book() { Id = 20, BookSatus = BookSatus.InLibrary, BookType = BookType.Book, BookAuthor = a3, Name = "The Tree on the Hill" });

            _collection.Add(new Book() { Id = 21, BookSatus = BookSatus.InLibrary, BookType = BookType.Book, BookAuthor = a4, Name = "Black House" });
            _collection.Add(new Book() { Id = 22, BookSatus = BookSatus.InLibrary, BookType = BookType.Book, BookAuthor = a4, Name = "The Stand" });
            _collection.Add(new Book() { Id = 23, BookSatus = BookSatus.InLibrary, BookType = BookType.Book, BookAuthor = a4, Name = "The Waste Lands" });
            _collection.Add(new Book() { Id = 24, BookSatus = BookSatus.InLibrary, BookType = BookType.Book, BookAuthor = a4, Name = "Rage" });
        }

        public IEnumerable<IBook> Collection { get { return this._collection; } }

        public void UpdateBookStatus(IBook Book, BookSatus NewStatus, BookSatus CheckStatus)
        {
            lock(this._collection)
            {
                int index = _collection.IndexOf(Book);
                if(_collection[index].BookSatus == CheckStatus)
                {
                    _collection[index].BookSatus = NewStatus;

                    //emulating DB read/write working
                    System.Threading.Thread.Sleep(3500);
                }
                else
                {
                    throw new Exception(string.Format("Could not set new status to current Book. Status check Faild"));
                }
            }
        }
    }

    public struct BookSearchFilter : IBookSearchFilter
    {
        public IEnumerable<string> Authors { get; set; }

        public BookSatus? BookSatus { get; set; }

        public BookType? BookType { get; set; }

        public string BookNameOrPartOfName { get; set; }
    }
}
