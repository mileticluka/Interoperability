using System;
using DAL.Model;

namespace DAL.UserRepo
{
	public interface IUserRepository
	{
		public bool login(User user);
		public bool register(User user);
	}
}

