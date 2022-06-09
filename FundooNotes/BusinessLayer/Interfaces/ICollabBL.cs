using CommonLayer;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface ICollabBL
    {

        Task<Collaborator> AddCollaborator(int userId, int NoteId, CollabValidation collab);
        Task<bool> RemoveCollaborator(int userId, int NoteId, int collaboratorId);
        Task<List<Collaborator>> GetCollaboratorByUserId(int userId);
        Task<List<Collaborator>> GetCollaboratorByNoteId(int userId, int NoteId);
        Task<List<Collaborator>> GetCollaboratorByRedisCache();

    }
}
