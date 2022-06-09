using CommonLayer;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interfaces
{
  public interface IUserBL
    {
        public User AddUser(UserPostModel user);
        public string LoginUser(string email, string password);
        public bool ForgetPassword(string email);
        public bool ChangePassword(string email, PasswordValidation password);
        public List<User> GetAllUsers();
    }
}
