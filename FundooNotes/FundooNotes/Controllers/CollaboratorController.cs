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
using System.Threading.Tasks;

namespace FundooNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollaboratorController : ControllerBase
    {
        ICollabBL collabBL;
        FundooContext fundoo;
        private readonly IMemoryCache memoryCache;
        private readonly IDistributedCache distributedCache;
        private string keyName = "VikasBenki";

        //Creating Constructor
        public CollaboratorController(ICollabBL collabBL, FundooContext fundoo, IMemoryCache memoryCache, IDistributedCache distributedCache)
        {
            this.collabBL = collabBL;
            this.fundoo = fundoo;
            this.memoryCache = memoryCache;
            this.distributedCache = distributedCache;

        }

        //HTTP method to handle add collaborator request
        [Authorize]
        [HttpPost("AddCollabrator/{NoteId}/{email}")]
        public async Task<ActionResult> AddCollaborator(int NoteId, CollabValidation collab)
        {
            try
            {
                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("userId", StringComparison.InvariantCultureIgnoreCase));
                int userId = Int32.Parse(userid.Value);
                var Id = fundoo.Notes.Where(x => x.NoteId == NoteId && x.UserId == userId).FirstOrDefault();
                if (Id == null)
                {
                    return this.BadRequest(new { success = false, message = $"Note doesn't exists" });
                }
                var result = await this.collabBL.AddCollaborator(userId, NoteId, collab);
                if (result != null)
                {
                    return this.Ok(new { success = true, message = $"Collaborator added successfully", data = result });
                }
                return this.BadRequest(new { success = false, message = $"Failed to add collaborator", data = result });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //HTTP method to handle delete collaborator request
        [Authorize]
        [HttpDelete("RemoveCollaborator/{NoteId}")]
        public async Task<ActionResult> RemoveCollaborator(int NoteId, int collaboratorId)
        {
            try
            {
                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("userId", StringComparison.InvariantCultureIgnoreCase));
                int userId = Int32.Parse(userid.Value);
                var re = fundoo.Collabrators.Where(x => x.userId == userId && x.NoteId == NoteId).FirstOrDefault();
                if (re == null)
                {
                    return this.BadRequest(new { success = false, message = $"Note doesn't exists" });
                }
                bool result = await this.collabBL.RemoveCollaborator(userId, NoteId, collaboratorId);
                if (result == true)
                {
                    return this.Ok(new { success = true, message = $"Collaborator removed successfully" });
                }
                return this.BadRequest(new { success = false, message = $"Failed to remove collaborator" });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //HTTP method to handle get collaborator request
        [Authorize]
        [HttpGet("GetCollaboratorByUserrId")]
        public async Task<ActionResult> GetCollaboratorByUserId()
        {
            try
            {
                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("userId", StringComparison.InvariantCultureIgnoreCase));
                int userId = Int32.Parse(userid.Value);
                var Id = fundoo.Collabrators.Where(x => x.userId == userId).FirstOrDefault();
                if (Id == null)
                {
                    return this.BadRequest(new { success = false, message = $"User doesn't exists" });
                }
                List<Collaborator> result = await this.collabBL.GetCollaboratorByUserId(userId);
                if (result != null)
                {
                    return this.Ok(new { success = true, message = $"Collaborator got successfully", data = result });
                }
                return this.BadRequest(new { success = false, message = $"Failed to get collaborator" });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //HTTP method to handle get collaborator request
        [Authorize]
        [HttpGet("GetCollaboratorByNoteId/{NoteId}")]
        public async Task<ActionResult> GetCollaboratorByNoteId(int NoteId)
        {
            try
            {
                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("userId", StringComparison.InvariantCultureIgnoreCase));
                int userId = Int32.Parse(userid.Value);
                var Id = fundoo.Collabrators.FirstOrDefault(x => x.NoteId == NoteId && x.userId == userId);
                if (Id == null)
                {
                    return this.BadRequest(new { success = false, message = $"Note doesn't exists" });
                }
                List<Collaborator> result = await this.collabBL.GetCollaboratorByUserId(userId);
                if (result != null)
                {
                    return this.Ok(new { success = true, message = $"Collaborator got successfully", data = result });
                }
                return this.BadRequest(new { success = false, message = $"Failed to get collaborator" });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("GetAllNotes_ByRadisCache")]
        public async Task<ActionResult> GetCollaboratorByRedisCache()
        {
            try
            {
                string serializeCollabList;
                var collabList = new List<Collaborator>();
                var redisCollabList = await distributedCache.GetAsync(keyName);
                if (redisCollabList != null)
                {
                    serializeCollabList = Encoding.UTF8.GetString(redisCollabList);
                    collabList = JsonConvert.DeserializeObject<List<Collaborator>>(serializeCollabList);
                }
                else
                {
                    collabList = await this.collabBL.GetCollaboratorByRedisCache();
                    serializeCollabList = JsonConvert.SerializeObject(collabList);
                    redisCollabList = Encoding.UTF8.GetBytes(serializeCollabList);
                    var options = new DistributedCacheEntryOptions()
                        .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                        .SetSlidingExpiration(TimeSpan.FromMinutes(2));
                    await distributedCache.SetAsync(keyName, redisCollabList, options);
                }
                return this.Ok(new { success = true, message = "Get Collabrator successful!!!", data = collabList });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
    
}
