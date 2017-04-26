using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELibrary.Books
{
    public interface IBookAuthor
    {
        int Id { get; }

        string FirstName { get; }

        string LastName { get; }
    }


}
