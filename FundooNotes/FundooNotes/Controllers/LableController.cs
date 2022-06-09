using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundooNotes.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LableController : ControllerBase
    {
        ILableBL lableBL;
        private readonly IMemoryCache memoryCache;
        private readonly IDistributedCache distributedCache;
        private string keyName = "VikasBenki";

        public LableController(ILableBL lableBL, IMemoryCache memoryCache, IDistributedCache distributedCache)
        {
            this.lableBL = lableBL;
            this.memoryCache = memoryCache;
            this.distributedCache = distributedCache;

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
                List<Lable> list = new List<Lable>();
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

        [HttpGet("GetlabelByRedisCache")]
        public async Task<ActionResult> GetlabelByRedisCache()
        {
            try
            {
                string serializeLabelList;
                var labelList = new List<Lable>();
                var redisLabelList = await distributedCache.GetAsync(keyName);
                if (redisLabelList != null)
                {
                    serializeLabelList = Encoding.UTF8.GetString(redisLabelList);
                    labelList = JsonConvert.DeserializeObject<List<Lable>>(serializeLabelList);
                }
                else
                {
                    labelList = await this.lableBL.GetlabelByRedisCache();
                    serializeLabelList = JsonConvert.SerializeObject(labelList);
                    redisLabelList = Encoding.UTF8.GetBytes(serializeLabelList);
                    var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2));
                    await distributedCache.SetAsync(keyName, redisLabelList, options);
                }
                var result = this.Ok(new { success = true, message = $"Get note successful!!!", data = labelList });
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

