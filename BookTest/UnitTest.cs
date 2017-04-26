using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BookTest
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void UserAutorise()
        {
            ELibrary.Users.User.Authenticate(1);

            Assert.AreEqual(false, ELibrary.Users.User.CurrentUser.UserStatus.HasFlag(ELibrary.Users.UserStatus.Librarian));
            Assert.AreEqual(true, ELibrary.Users.User.CurrentUser.UserStatus.HasFlag(ELibrary.Users.UserStatus.Anonymus));
        }

        [TestMethod]
        public void UserSeniorLibrarianAutorise()
        {
            ELibrary.Users.User.Authenticate(7);

            Assert.AreEqual(true, ELibrary.Users.User.CurrentUser.UserStatus.HasFlag(ELibrary.Users.UserStatus.Librarian));
            Assert.AreEqual(true, ELibrary.Users.User.CurrentUser.UserStatus.HasFlag(ELibrary.Users.UserStatus.SeniorLibrarian));
            Assert.AreEqual(false, ELibrary.Users.User.CurrentUser.UserStatus.HasFlag(ELibrary.Users.UserStatus.Anonymus));
        }

        [TestMethod]
        public void CreateLibrary()
        {
            ELibrary.Library.BookLibraryWork lib = new ELibrary.Library.BookLibraryWork(new ELibrary.Defaults.BookReposytory());
        }


        [TestMethod]
        public void LibrarySearch()
        {
            ELibrary.Library.BookLibraryWork lib = new ELibrary.Library.BookLibraryWork(new ELibrary.Defaults.BookReposytory());

            var result = lib.SearchForBook(new ELibrary.Defaults.BookSearchFilter() { Authors = new string[2] { "kuttner", "king" } });
            Assert.AreEqual(14, result.Count());

            result = lib.SearchForBook(new ELibrary.Defaults.BookSearchFilter() { Authors = new string[1] { "kuttner" }, BookSatus = ELibrary.Books.BookSatus.InLibrary, BookNameOrPartOfName = "cthuLhu" });
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void LibraryGiveBook()
        {
            ELibrary.Users.User.Authenticate(3);

            ELibrary.Library.BookLibraryWork lib = new ELibrary.Library.BookLibraryWork(new ELibrary.Defaults.BookReposytory());

            var result = lib.SearchForBook(new ELibrary.Defaults.BookSearchFilter() { Authors = new string[1] { "lovecraft" }, BookSatus = ELibrary.Books.BookSatus.InLibrary, BookNameOrPartOfName = "Cthulhu" });
            Assert.AreEqual(1, result.Count());

            var book = result.First();

            var status = lib.CheckBookStatus(book);
            Assert.AreEqual(true, status == ELibrary.Books.BookSatus.InLibrary);

            lib.GiveBook(book);

            status = lib.CheckBookStatus(book);
            Assert.AreEqual(true, status == ELibrary.Books.BookSatus.AtUser);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Only Users with status Librarian (or higher) can Give Book")]
        public void LibraryGiveBookAccessError()
        {
            ELibrary.Users.User.Authenticate(42);

            ELibrary.Library.BookLibraryWork lib = new ELibrary.Library.BookLibraryWork(new ELibrary.Defaults.BookReposytory());

            var result = lib.SearchForBook(new ELibrary.Defaults.BookSearchFilter() { Authors = new string[1] { "lovecraft" }, BookSatus = ELibrary.Books.BookSatus.InLibrary, BookNameOrPartOfName = "Cthulhu" });
            Assert.AreEqual(1, result.Count());

            var book = result.First();

            var status = lib.CheckBookStatus(book);
            Assert.AreEqual(true, status == ELibrary.Books.BookSatus.InLibrary);

            lib.GiveBook(book);
        }

        [TestMethod]
        public void LibraryTakeGiveArchiveBook()
        {
            ELibrary.Users.User.Authenticate(3);

            ELibrary.Library.BookLibraryWork lib = new ELibrary.Library.BookLibraryWork(new ELibrary.Defaults.BookReposytory());

            var result = lib.SearchForBook(new ELibrary.Defaults.BookSearchFilter() { Authors = new string[1] { "king" }, BookSatus = ELibrary.Books.BookSatus.InLibrary, BookNameOrPartOfName = "Stand" });
            Assert.AreEqual(1, result.Count());

            var book = result.First();

            var status = lib.CheckBookStatus(book);
            Assert.AreEqual(true, status == ELibrary.Books.BookSatus.InLibrary);

            lib.GiveBook(book);

            status = lib.CheckBookStatus(book);
            Assert.AreEqual(true, status == ELibrary.Books.BookSatus.AtUser);

            ELibrary.Users.User.Authenticate(42);

            lib.TakeBook(book);

            status = lib.CheckBookStatus(book);
            Assert.AreEqual(true, status == ELibrary.Books.BookSatus.InLibrary);

            ELibrary.Users.User.Authenticate(7);

            lib.ArchiveBook(book);

            status = lib.CheckBookStatus(book);
            Assert.AreEqual(true, status == ELibrary.Books.BookSatus.Archived);
        }
    }
}
