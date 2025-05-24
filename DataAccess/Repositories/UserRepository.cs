using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independiente.DataAccess.Repositories
{
    public interface IUserRepository
    {
        DataAccess.User GetUser(User user);
    }
    public class UserRepository
    {
        //public DataAccess.User GetUser(User user)
        //{

        //    //return userData;
        //}
    }
}
