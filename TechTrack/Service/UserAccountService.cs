using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTrack.Domain.IRepository;
using TechTrack.Domain.Model;
using TechTrack.Helpers;

namespace TechTrack.Service
{
    public class UserAccountService
    {
        private IUserAccountRepository UserAccountRepository { get; set; }
        public UserAccountService(IUserAccountRepository userAccountRepository)
        {
            UserAccountRepository = userAccountRepository;
        }
        public static UserAccountService GetInstance()
        {
            return App._serviceProvider.GetRequiredService<UserAccountService>();
        }
        public void Add(UserAccount newUser)
        {
            UserAccountRepository.Add(newUser);
        }
        public UserAccount? GetById(int Id)
        {
            return UserAccountRepository.GetById(Id);
        }
        public UserAccount GetByUsername(string username)
        {
            return UserAccountRepository.GetByUsername(username);
        }
        public List<UserAccount> GetAll()
        {
            return UserAccountRepository.GetAll();
        }
        public bool Delete(int id)
        {
            return UserAccountRepository.Delete(id);
        }
        public bool ValidateUser(string username, string password)
        {
            var userAccount = UserAccountRepository.GetByUsername(username);
            if (userAccount != null)
            {
                return PasswordHasher.VerifyPassword(userAccount.Password, password);
            }
            return false;
        }
    }
}
