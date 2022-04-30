using BusinessLayer.Interfaces;
using CommonLayer;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class CollabBL : ICollabBL
    {
        ICollabRL collabRL;
        public CollabBL(ICollabRL collabRL)
        {
            this.collabRL = collabRL;
        }
        public async Task<Collaborator> AddCollaborator(int userId, int Noteid, CollabValidation collab)
        {
            try
            {
                return await this.collabRL.AddCollaborator(userId, Noteid, collab);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> RemoveCollaborator(int userId, int NoteId, int collaboratorId)
        {
            try
            {
                return await this.collabRL.RemoveCollaborator(userId, NoteId, collaboratorId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<Collaborator>> GetCollaboratorByUserId(int userId)
        {
            try
            {
                return await this.collabRL.GetCollaboratorByUserId(userId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<Collaborator>> GetCollaboratorByNoteId(int userId, int NoteId)
        {
            try
            {
                return await this.collabRL.GetCollaboratorByNoteId(userId, NoteId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
