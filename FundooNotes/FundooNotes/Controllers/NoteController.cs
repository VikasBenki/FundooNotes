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
    [ApiController]
    [Route("[controller]")]
   
    public class NoteController : ControllerBase
    {
        INoteBL noteBL;
        private readonly IMemoryCache memoryCache;
        private readonly IDistributedCache distributedCache;
        private string keyName = "VikasBenki";

        public NoteController(INoteBL noteBL, IMemoryCache memoryCache, IDistributedCache distributedCache)
        {
            this.noteBL = noteBL;
            this.memoryCache = memoryCache;
            this.distributedCache = distributedCache;

        }

        [Authorize]
        [HttpPost("AddNote")]
        public async Task<ActionResult> AddNote(NotePostModel notePostModel)
        {
            try
            {
                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("userId", StringComparison.InvariantCultureIgnoreCase));
                int userId = Int32.Parse(userid.Value);

                await this.noteBL.AddNote(notePostModel, userId);
                return this.Ok(new { success = true, message = "Note Added Successfull " });
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        [Authorize]
        [HttpGet(("GetNote/{NoteId}"))]
        public async Task<ActionResult<Note>> GetNote(int NoteId, int UserId)
        {
            try
            {
                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("UserId", StringComparison.InvariantCultureIgnoreCase));
                UserId = Int32.Parse(userid.Value);

                var result = await this.noteBL.GetNote(NoteId, UserId);
                return this.Ok(new
                {
                    success = true,
                    message = $"Note got successfuly ",
                    data = result
                });
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        [Authorize]
        [HttpDelete("Delete/{NoteId}")]
        public async Task<ActionResult> DeleteNote(int NoteId)
        {
            try
            {
                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("UserId", StringComparison.InvariantCultureIgnoreCase));
                int UserId = Int32.Parse(userid.Value);

                await this.noteBL.DeleteNote(NoteId, UserId);
                return this.Ok(new { success = true, message = "Note deleted successfully!!!" });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [Authorize]
        [HttpPut("Update/{NoteId}")]
        public async Task<IActionResult> UpdateNote(NotePostModel notePostModel, int NoteId)
        {
            try
            {
                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("UserId", StringComparison.InvariantCultureIgnoreCase));
                int UserId = Int32.Parse(userid.Value);
                var result = await this.noteBL.UpdateNote(notePostModel, NoteId, UserId);
                return this.Ok(new { success = true, message = $"Note updated successfully!!!", data = result });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [Authorize]
        [HttpPut("ArchiveNote/{NoteId}")]
        public async Task<IActionResult> ArchieveNote(NotePostModel notePostModel, int NoteId)
        {
            try
            {
                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("UserId", StringComparison.InvariantCultureIgnoreCase));
                int UserId = Int32.Parse(userid.Value);
                var result = await this.noteBL.ArchieveNote(NoteId, UserId);
                return this.Ok(new { success = true, message = $"Note Archieved successfully!!!", data = result });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [Authorize]
        [HttpPut("IsPinned/{NoteId}")]
        public async Task<ActionResult> IsPinned(int NoteId)
        {
            try
            {
                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("UserId", StringComparison.InvariantCultureIgnoreCase));
                int UserId = Int32.Parse(userid.Value);
                var res = await this.noteBL.PinNote(NoteId, UserId);
                if (res != null)
                    return this.Ok(new { success = true, message = "Note pinned successfully!!!" });
                else
                    return this.BadRequest(new { success = false, message = "Failed to pin note or Id does not exists" });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [Authorize]
        [HttpPut("IsTrash/{NoteId}")]
        public async Task<ActionResult> IsTrash(int NoteId)
        {
            try
            {
                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("UserId", StringComparison.InvariantCultureIgnoreCase));
                int UserId = Int32.Parse(userid.Value);
                var res = await this.noteBL.TrashNote(NoteId, UserId);
                if (res != null)
                    return this.Ok(new { success = true, message = "Note trashed successfully!!!" });
                else
                    return this.BadRequest(new { success = false, message = "Failed to trash note or Id does not exists" });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [Authorize]
        [HttpPut("ChangeColorNote/{NoteId}")]
        public async Task<ActionResult> ChangeColorNote(int NoteId, string color)
        {
            try
            {
                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("UserId", StringComparison.InvariantCultureIgnoreCase));
                int UserId = Int32.Parse(userid.Value);
                var res = await this.noteBL.ChangeColor(NoteId, UserId, color);
                if (res != null)
                    return this.Ok(new { success = true, message = "Note color changed successfully!!!" });
                else
                    return this.BadRequest(new { success = false, message = "Failed to change color note or Id does not exists" });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Authorize]
        [HttpGet("GetAllNotes")]
        public async Task<ActionResult> GetAllNotes()
        {
            try
            {
                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("UserId", StringComparison.InvariantCultureIgnoreCase));
                int UserId = Int32.Parse(userid.Value);
                List<Note> result = new List<Note>();
                result = await this.noteBL.GetAllNote(UserId);
                return this.Ok(new { success = true, message = $"Below are all notes", data = result });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet("GetAllNotes_ByRadisCache")]
        public async Task<ActionResult> GetAllNotes_ByRadisCache()
        {
            try
            {
                string serializeNoteList;
                var noteList = new List<Note>();
                var redisNoteList = await distributedCache.GetAsync(keyName);
                if (redisNoteList != null)
                {
                    serializeNoteList = Encoding.UTF8.GetString(redisNoteList);
                    noteList = JsonConvert.DeserializeObject<List<Note>>(serializeNoteList);
                }
                else
                {
                    noteList = await this.noteBL.GetAllNotes_ByRadisCache();
                    serializeNoteList = JsonConvert.SerializeObject(noteList);
                    redisNoteList = Encoding.UTF8.GetBytes(serializeNoteList);
                    var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2));
                    await distributedCache.SetAsync(keyName, redisNoteList, options);

                }
                return this.Ok(new { success = true, message = "Get note successful!!!", data = noteList });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}

