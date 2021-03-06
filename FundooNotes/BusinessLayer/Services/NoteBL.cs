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
    public class NoteBL : INoteBL
    {
        INoteRL noteRL;
        public NoteBL(INoteRL userRL)
        {
            this.noteRL = userRL;
        }
        public async Task AddNote(NotePostModel notePostModel, int userId)
        {

            try
            {
                await this.noteRL.AddNote(notePostModel, userId);
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }



        public async Task<Note> GetNote(int noteId, int userId)
        {
            try
            {
                return await this.noteRL.GetNote(noteId, userId);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }



        public async Task DeleteNote(int noteId, int userId)
        {
            try
            {
                await this.noteRL.DeleteNote(noteId, userId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Note> UpdateNote(NotePostModel notePostModel, int noteId, int userId)
        {
            try
            {
                return await this.noteRL.UpdateNote(notePostModel, noteId, userId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Note> ArchieveNote(int noteId, int userId)
        {
            try
            {
                return await this.noteRL.ArchieveNote(noteId, userId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task<Note> PinNote(int noteId, int userId)
        {
            try
            {
                return this.noteRL.PinNote(noteId, userId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Task<Note> TrashNote(int noteId, int userId)
        {
            try
            {
                return noteRL.TrashNote(noteId, userId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Task<Note> ChangeColor(int noteId, int userId, string newColor)
        {
            try
            {
                return this.noteRL.ChangeColor(noteId, userId, newColor);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<List<Note>> GetAllNote(int userId)
        {
            try
            {
                return await this.noteRL.GetAllNote(userId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<Note>> GetAllNotes_ByRadisCache()
        {
            try
            {
                return await this.noteRL.GetAllNotes_ByRadisCache();
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}


