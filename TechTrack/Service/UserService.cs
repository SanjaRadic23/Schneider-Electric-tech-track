using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTrack.Domain.IRepository;
using TechTrack.Domain.Model;

namespace TechTrack.Service
{
    public class UserService
    {
        public IUserRepository UserRepository { get; set; }
        public UserService(IUserRepository userRepository)
        {
            UserRepository = userRepository;
        }
        public static UserService GetInstance()
        {
            return App._serviceProvider.GetRequiredService<UserService>();
        }
        public void Add(User newUser)
        {
            UserRepository.Add(newUser);
        }
        public User? GetById(int Id)
        {
            return UserRepository.GetById(Id);
        }
        public List<User> GetAll()
        {
            return UserRepository.GetAll();
        }
        public bool Delete(int id)
        {
            return UserRepository.Delete(id);
        }
    }
}
