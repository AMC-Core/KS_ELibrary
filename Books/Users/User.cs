using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace ELibrary.Users
{
    enum UserStatusBasis
    {
        Anonymus            = 1 << 0, //1
        Librarian           = 1 << 1, //2
        SeniorLibrarian     = 1 << 2, //4
        TraineeLibrarian    = 1 << 3, //8
    }

    [Flags]
    public enum UserStatus
    {
        Anonymus            = UserStatusBasis.Anonymus,                     //1
        TraineeLibrarian    = UserStatusBasis.TraineeLibrarian,             //8
        Librarian           = TraineeLibrarian | UserStatusBasis.Librarian, //10
        SeniorLibrarian     = Librarian | UserStatusBasis.SeniorLibrarian   //14
    }

    public sealed class User : IIdentity
    {
        private static class UserRepository
        {
            public static readonly User _anonymous; 

            private static readonly List<User> _userCollection;

            static UserRepository()
            {
                _anonymous = new User() { Id = -1, UserStatus = UserStatus.Anonymus, Name = string.Empty };
                _userCollection = new List<User>(4);
                _userCollection.Add(_anonymous);
                _userCollection.Add(new User() { Id = 3, UserStatus = UserStatus.Librarian, Name = "Jhon Smith" });
                _userCollection.Add(new User() { Id = 7, UserStatus = UserStatus.SeniorLibrarian, Name = "Albert Shdradman" });
                _userCollection.Add(new User() { Id = 42, UserStatus = UserStatus.TraineeLibrarian, Name = "Alan Aktrat" });
            }

            public static void Authenticate(int UserId)
            {
                User user = UserId > 0 && _userCollection.Any(u => u.Id == UserId) ? _userCollection.First(u => u.Id == UserId) : _userCollection.First(u => u.Id == -1);
                IPrincipal principal = new GenericPrincipal(user, new string[0]);

                System.Threading.Thread.CurrentPrincipal = principal;
            }
        }

        public int Id { get; private set; }

        public string Name { get; private set; }

        public UserStatus UserStatus { get; private set; }

        private User() { }

        #region IIdentity Members

        string IIdentity.Name
        {
            get
            {
                return string.Format("User({0})", this.Id);
            }
        }

        string IIdentity.AuthenticationType
        {
            get
            {
                return null;
            }
        }

        bool IIdentity.IsAuthenticated
        {
            get
            {
                return this.Id > 0;
            }
        }

        #endregion

        public static void Authenticate(int UserId)
        {
            UserRepository.Authenticate(UserId);
        }

        public static User CurrentUser
        {
            get
            {
                return (System.Threading.Thread.CurrentPrincipal.Identity as User) ?? UserRepository._anonymous;
            }
        }
    }
}
