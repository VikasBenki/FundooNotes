using BusinessLayer.Interfaces;
using CommonLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.FundooNotesContext;
using System;
using System.Linq;

namespace FundooNotes.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        IUserBL userBL;
        
        public UserController(IUserBL userBL, FundooContext fundoo)
        {
            this.userBL = userBL;
           

        }
        [HttpPost("register")]
        public ActionResult RegisterUser(UserPostModel user)
        {
            try
            {
                

                var result = this.userBL.AddUser(user);
                if (result != null)
                {
                    return this.Ok(new { success = true, message = $"Register Successful {result}" });

                }
                return this.BadRequest(new { success = false, message = $"Register Failed" });


            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        [HttpPost("Login/{email}/{password}")]
        public ActionResult LoginUser(string email, string password)
        {
            try
            {
                var result = this.userBL.LoginUser(email, password);
                if (result != null)
                {
                    return this.Ok(new
                    {
                        success = true,
                        message = $"Login Successful " +
                       $" token:  {result}"
                    });                                       

                }

                return this.BadRequest(new { success = false, message = $"Login Failed" });

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        [HttpPost("ForgetPassword/{email}")]
        public ActionResult ForgetPassword(string email)
        {
            try
            {


                var result = this.userBL.ForgetPassword(email);
                if (result != false)
                {
                    return this.Ok(new
                    {
                        success = true,
                        message = $"Mail Sent Successfully " +
                        $" token:  {result}"
                    });

                }
                return this.BadRequest(new { success = false, message = $"mail not sent" });

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        [Authorize]
        [HttpPut("ChangePassword/{email}")]
        public ActionResult ChangePassword(string email, PasswordValidation valid)
        {
            try
            {


                var result = this.userBL.ChangePassword(email, valid);
                if (result != false)
                {
                    return this.Ok(new
                    {
                        success = true,
                        message = $"password changed successfuly " +
                        $" token:  {result}"
                    });

                }
                return this.BadRequest(new { success = false, message = $"login failure" });

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


    }
}
