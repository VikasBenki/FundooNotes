using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interfaces
{
    public interface ILableRL
    {
        Task AddLable(int userId, int Noteid, string LableName);
        Task<List<Lable>> GetLable(int userId);
        Task<List<Lable>> GetLableByNoteId(int NoteId);
        Task<Lable> UpdateLable(int userId, int LableId, string LableName);
        Task DeleteLable(int LableId, int userId);
        Task<List<Lable>> GetlabelByRedisCache();
    }
}
