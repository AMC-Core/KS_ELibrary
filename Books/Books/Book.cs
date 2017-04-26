using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELibrary.Books
{
    public interface IBook
    {
        int Id { get; }

        IBookAuthor Author { get; }

        string Name { get; }

        BookType BookType { get; }

        BookSatus BookSatus { get; set; }
    }
}
