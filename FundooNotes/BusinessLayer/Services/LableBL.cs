using BusinessLayer.Interfaces;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class LableBL: ILableBL
    {
        ILableRL LableRL;
        public LableBL(ILableRL LableRL)
        {
            this.LableRL = LableRL;
        }

        public async Task AddLable(int userId, int Noteid, string LableName)
        {
            try
            {
                await this.LableRL.AddLable(userId, Noteid, LableName);
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
                return await this.LableRL.GetLable(userId);
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
                return await this.LableRL.GetLableByNoteId(NoteId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Lable> UpdateLable(int userId, int LableId, string LableName)
        {
            try
            {
                return await this.LableRL.UpdateLable(userId, LableId, LableName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteLable(int LableId, int userId)
        {
            try
            {
                await this.LableRL.DeleteLable(LableId, userId);
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
                return await this.LableRL.GetlabelByRedisCache();
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}


