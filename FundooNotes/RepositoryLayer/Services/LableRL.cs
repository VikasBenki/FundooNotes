using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RepositoryLayer.Entity;
using RepositoryLayer.FundooNotesContext;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Services
{
    public class LableRL : ILableRL
    {
        FundooContext fundoo;
        public IConfiguration configuration;
        public LableRL(FundooContext fundoo, IConfiguration configuration)
        {
            this.fundoo = fundoo;
            this.configuration = configuration;

        }

        public async Task AddLable(int userId, int Noteid, string LableName)
        {
            try
            {
                var user = fundoo.Users.FirstOrDefault(u => u.userID == userId);
                var note = fundoo.Notes.FirstOrDefault(b => b.NoteId == Noteid);
                Lable lable = new Lable
                {
                    User = user,
                    Note = note
                };
                lable.LableName = LableName;
                fundoo.Lables.Add(lable);
                await fundoo.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<List<Lable>> GetLable(int userId)
        {
            try
            {
                List<Lable> reuslt = await fundoo.Lables.Where(u => u.UserId == userId).Include(u => u.User).Include(u => u.Note).ToListAsync();
                return reuslt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<Lable>> GetLableByNoteId(int NoteId)
        {
            try
            {
                List<Lable> reuslt = await fundoo.Lables.Where(u => u.NoteId == NoteId).Include(u => u.User).Include(u => u.Note).ToListAsync();
                return reuslt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Lable> UpdateLable(int userId, int LabelId, string LableName)
        {
            try
            {

                Lable reuslt = fundoo.Lables.FirstOrDefault(u => u.LableId == LabelId && u.UserId == userId);

                if (reuslt != null)
                {
                    reuslt.LableName = LableName;
                    await fundoo.SaveChangesAsync();
                    var result = fundoo.Lables.Where(u => u.LableId == LabelId).FirstOrDefaultAsync();
                    return reuslt;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteLable(int LabelId, int userId)
        {
            try
            {
                var result = fundoo.Lables.FirstOrDefault(u => u.LableId == LabelId && u.UserId == userId);
                fundoo.Lables.Remove(result);
                await fundoo.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<Lable>> GetlabelByRedisCache()
        {
            try
            {
                List<Lable> reuslt = await fundoo.Lables.ToListAsync();
                return reuslt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}

