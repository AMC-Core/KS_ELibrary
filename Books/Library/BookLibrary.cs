using ELibrary.Books;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace ELibrary.Library
{
    public interface IBookSearchFilter
    {
        IEnumerable<string> Authors { get; }

        BookSatus? BookSatus { get; }

        BookType? BookType { get; }

        string BookNameOrPartOfName { get; }
    }

    public interface IBookReposytory
    {
        IEnumerable<IBook> Collection { get; }

        void UpdateBookStatus(IBook Book, BookSatus NewStatus);
    }

    public sealed class BookLibraryWork
    {
        private readonly IBookReposytory _repository;

        public BookLibraryWork(IBookReposytory repository)
        {
            if (repository == null)
                throw new Exception("IBookReposytory must be set");
            _repository = repository;
        }

        public BookSatus CheckBookStatus(IBook Book)
        {
            if (!_repository.Collection.Any(b => b.Id == Book.Id))
                throw new Exception("Book not found");
            return _repository.Collection.First(b => b.Id == Book.Id).BookSatus;
        }

        public IEnumerable<IBook> SearchForBook(IBookSearchFilter filter)
        {
            if (filter == null) return new List<IBook>(0);
            if (filter.Authors.Any(a => string.IsNullOrWhiteSpace(a)))
                throw new Exception("BookSearchFilter wrong set");

            List<IBook> result = new List<IBook>();
            Parallel.ForEach(_repository.Collection, (le) =>
            {
                if (filter.BookSatus.HasValue && le.BookSatus != filter.BookSatus.Value)
                {
                    return;
                }

                if(filter.BookType.HasValue && le.BookType != filter.BookType.Value)
                {
                    return;
                }

                if(!string.IsNullOrWhiteSpace(filter.BookNameOrPartOfName) && le.Name.IndexOf(filter.BookNameOrPartOfName, StringComparison.InvariantCultureIgnoreCase) < 0)
                {
                    return;
                }

                if (!filter.Authors.Any(a => le.Author.LastName.IndexOf(a, StringComparison.InvariantCultureIgnoreCase) > -1 || le.Author.FirstName.IndexOf(a, StringComparison.InvariantCultureIgnoreCase) > -1))
                {
                    return;
                }

                result.Add(le);
            });

            return result;
        }

        public void GiveBook(IBook Book)
        {
            if(Users.User.CurrentUser.UserStatus.HasFlag(Users.UserStatus.Librarian))
            {
                var status = CheckBookStatus(Book);
                if (status != BookSatus.InLibrary)
                    throw new Exception("Could not give Book that is not in library");

                _repository.UpdateBookStatus(Book, BookSatus.AtUser);
                return;
            }
            throw new Exception("Only Users with status Librarian (or higher) can Give Book");
        }

        public void TakeBook(IBook Book)
        {
            if (Users.User.CurrentUser.UserStatus.HasFlag(Users.UserStatus.TraineeLibrarian))
            {
                var status = CheckBookStatus(Book);
                if (status != BookSatus.AtUser)
                    throw new Exception("Could not take Book that is not at User");

                _repository.UpdateBookStatus(Book, BookSatus.AtUser);
                return;
            }
            throw new Exception("Only Users with status Trainee Librarian (or higher) can Take Book");
        }

        public void ArchiveBook(IBook Book)
        {
            if (Users.User.CurrentUser.UserStatus.HasFlag(Users.UserStatus.SeniorLibrarian))
            {
                var status = CheckBookStatus(Book);
                if (status != BookSatus.InLibrary)
                    throw new Exception("Could not archive Book that is not in library");

                _repository.UpdateBookStatus(Book, BookSatus.AtUser);
                return;
            }
            throw new Exception("Only Users with status Senior Librarian can Archive Book");
        }
    }
}
