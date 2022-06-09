using CommonLayer;
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
    public class CollabRL : ICollabRL
    {
        FundooContext fundoo;
        public IConfiguration configuration;
        public CollabRL(FundooContext fundoo, IConfiguration configuration)
        {
            this.fundoo = fundoo;
            this.configuration = configuration;

        }
        public async Task<Collaborator> AddCollaborator(int userId, int NoteId, CollabValidation collab)
        {
            try
            {
                var user = fundoo.Users.FirstOrDefault(u => u.userID == userId);
                var note = fundoo.Notes.FirstOrDefault(b => b.NoteId == NoteId);
                Collaborator collaborator = new Collaborator
                {
                    User = user,
                    Note = note
                };
                collaborator.CollabEmail = collab.email;
                fundoo.Collabrators.Add(collaborator);
                await fundoo.SaveChangesAsync();
                return collaborator;

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
                var result = fundoo.Collabrators.FirstOrDefault(u => u.userId == userId && u.NoteId == NoteId && u.collaboratorId == collaboratorId);
                if (result != null)
                {
                    fundoo.Collabrators.Remove(result);
                    await fundoo.SaveChangesAsync();
                    return true;
                }
                return false;
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
                List<Collaborator> result = await fundoo.Collabrators.Where(u => u.userId == userId).Include(u => u.User).Include(U => U.Note).ToListAsync();
                return result;
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
                List<Collaborator> result = await fundoo.Collabrators.Where(u => u.userId == userId && u.NoteId == NoteId).Include(u => u.User).Include(U => U.Note).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<List<Collaborator>> GetCollaboratorByRedisCache()
        {
            try
            {
                List<Collaborator> result = await fundoo.Collabrators.ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}

