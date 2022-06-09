using BusinessLayer.Interfaces;
using CommonLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using RepositoryLayer.Entity;
using RepositoryLayer.FundooNotesContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FundooNotes.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        IUserBL userBL;
        private readonly IMemoryCache memoryCache;
        private readonly IDistributedCache distributedCache;
        private string keyName = "VikasBenki";
        FundooContext fundoo;
        public UserController(IUserBL userBL, FundooContext fundoo, IMemoryCache memoryCache, IDistributedCache distributedCache)
        {
            this.userBL = userBL;
           this.memoryCache = memoryCache;
            this.distributedCache = distributedCache;
            this.fundoo = fundoo;
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
                        message = $"{result}"

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
        [HttpPut("ChangePassword")]
        public ActionResult ChangePassword(PasswordValidation valid)
        {
            try
            {
                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("userId", StringComparison.InvariantCultureIgnoreCase));
                int userId = Int32.Parse(userid.Value);
                var result = fundoo.Users.Where(u => u.userID == userId).FirstOrDefault();
                string email = result.email.ToString();

                bool res = this.userBL.ChangePassword(email,valid); 
                if (res != false)
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
        [HttpGet("getallusers")]
        public ActionResult GetAllUsers()
        {
            try
            {
                var result = this.userBL.GetAllUsers();
                return this.Ok(new { success = true, message = $"Below are the User data", data = result });
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpGet("getallusers_Radis")]
        public ActionResult GetAllUsers_Redis()
        {
            try
            {
                string serializeUserList;
                var userList = new List<User>();
                var redisUserList = distributedCache.Get(keyName);
                if (redisUserList != null)
                {
                    serializeUserList = Encoding.UTF8.GetString(redisUserList);
                    userList = JsonConvert.DeserializeObject<List<User>>(serializeUserList);
                }
                else
                {
                    userList = this.userBL.GetAllUsers();
                    serializeUserList = JsonConvert.SerializeObject(userList);
                    redisUserList = Encoding.UTF8.GetBytes(serializeUserList);
                    var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2));
                    distributedCache.Set(keyName, redisUserList, options);
                }
                return this.Ok(new { success = true, message = "Get note successful!!!", data = userList });
            }
            catch (Exception e)
            {
                throw e;
            }
        }


    }
}
