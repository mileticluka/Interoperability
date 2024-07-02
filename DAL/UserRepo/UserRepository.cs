using System;
using DAL.Model;

namespace DAL.UserRepo
{
	public class UserRepository : IUserRepository
	{
		private readonly DataContext _dataContext;

		public UserRepository()
		{
			_dataContext = DataContext.Instance;
		}

        public bool login(User user)
        {
            User? existingUser = _dataContext.users.FirstOrDefault(u => u.username == user.username);
            if (existingUser == null)
            {
                return false;
            }
            if(existingUser.password != user.password)
            {
                return false;
            }

            return true;
        }

        public bool register(User user)
        {
            if(string.IsNullOrEmpty(user.username)  || string.IsNullOrEmpty(user.password))
            {
                return false;
            }

            if(_dataContext.users.FirstOrDefault(u=>u.username == user.username) != null)
            {
                return false;
            }


            _dataContext.users.Add(user);
            return true;
        }
    }
}

