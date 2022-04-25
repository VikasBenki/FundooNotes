using BusinessLayer.Interfaces;
using CommonLayer;
using RepositoryLayer.Entity;
using RepositoryLayer.UserInterface;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Services
{
    public class UserBL : IUserBL
    {
        //IUserRL userRL;
        private readonly IUserRL userRL;
        public UserBL(IUserRL userRL)
        {
            this.userRL = userRL;
        }
        public User AddUser(UserPostModel user)
        {
            try
            {
               return this.userRL.AddUser(user);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

       

        public string LoginUser(string email, string password)
        {
            try
            {
                return userRL.LoginUser(email, password);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool ForgetPassword(string email)
        {
            try
            {
                return this.userRL.ForgetPassword(email);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public bool ChangePassword(string email, PasswordValidation valid)
        {
            try
            {
                return this.userRL.ChangePassword(email,valid);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
