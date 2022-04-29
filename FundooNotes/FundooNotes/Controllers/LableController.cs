using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FundooNotes.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LableController : ControllerBase
    {
        ILableBL lableBL;

        public LableController(ILableBL lableBL)
        {
            this.lableBL = lableBL;

        }
        //HTTP method to handle add label request
        [Authorize]
        [HttpPost("Addlabel/{NoteId}/{LabelName}")]
        public async Task<ActionResult> AddLable(int NoteId, string LableName)
        {
            try
            {
                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("UserId", StringComparison.InvariantCultureIgnoreCase));
                int UserId = Int32.Parse(userid.Value);
                await this.lableBL.AddLable(UserId, NoteId, LableName);
                return this.Ok(new { success = true, message = $"Lable added successfully" });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //HTTP method to handle get label request
        [Authorize]
        [HttpGet("GetLable")]
        public async Task<ActionResult> GetLable()
        {
            try
            {
                List<Lable> list = new List<Lable>();
                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("UserId", StringComparison.InvariantCultureIgnoreCase));
                int UserId = Int32.Parse(userid.Value);
                list = await this.lableBL.GetLable(UserId);
                if (list == null)
                {
                    return this.BadRequest(new { success = false, message = "Failed to get lable" });
                }
                return this.Ok(new { success = true, message = $"Lable get successfully", data = list });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //HTTP method to handle get label request
        [Authorize]
        [HttpGet("GetLableByNoteId/{NoteId}")]
        public async Task<ActionResult> GetLableByNoteId(int NoteId)
        {
            try
            {
                List <Lable> list = new List<Lable>();
                list = await this.lableBL.GetLable(NoteId);
                if (list == null)
                {
                    return this.BadRequest(new { success = true, message = "Failed to get label" });
                }
                return this.Ok(new { success = true, message = $"Label get successfully", data = list });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //HTTP method to handle update label request
        [Authorize]
        [HttpPut("UpdateLable/{LableId}/{LableName}")]
        public async Task<ActionResult> UpdateLable(string LableName, int LableId)
        {
            try
            {
                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("userId", StringComparison.InvariantCultureIgnoreCase));
                int userId = Int32.Parse(userid.Value);
                var result = await this.lableBL.UpdateLable(userId, LableId, LableName);
                if (result == null)
                {
                    return this.BadRequest(new { success = false, message = "Updation of Label failed" });
                }
                return this.Ok(new { success = true, message = $"Label updated successfully", data = result });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //HTTP method to handle delete label request
        [Authorize]
        [HttpDelete("DeleteLable/{LableId}")]
        public async Task<ActionResult> DeleteLable(int LableId)
        {
            try
            {
                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("userId", StringComparison.InvariantCultureIgnoreCase));
                int userId = Int32.Parse(userid.Value);
                await this.lableBL.DeleteLable(LableId, userId);
                return this.Ok(new { success = true, message = $"Lable Deleted successfully" });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

