using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTrack.Domain.Model;

namespace TechTrack.Domain.IRepository
{
    public interface IUserAccountRepository
    {
        List<UserAccount> GetAll();
        void Add(UserAccount userAccount);
        UserAccount? GetById(int Id);
        int NextId();
        bool Delete(int id);
        UserAccount GetByUsername(string username);
    }
}
