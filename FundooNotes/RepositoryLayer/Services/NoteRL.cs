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
    public class NoteRL : INoteRL
    {


        FundooContext fundoo;
        public IConfiguration configuration;
        public NoteRL(FundooContext fundoo, IConfiguration configuration)
        {
            this.fundoo = fundoo;
            this.configuration = configuration;

        }

        public async Task AddNote(NotePostModel notePostModel, int userId)
        {
            try
            {
                var user = fundoo.Users.Where(u => u.userID == userId).FirstOrDefault();
                Note note = new Note
                {

                    User = user
                };
                note.Title = notePostModel.Title;
                note.Description = notePostModel.Description;
                note.BGColor = notePostModel.BGColor;
                note.RegisterDate = DateTime.Now;
                fundoo.Add(note);
                await fundoo.SaveChangesAsync();
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
                return await fundoo.Notes.Where(u => u.NoteId == noteId && u.UserId == userId)
                .Include(u => u.User).FirstOrDefaultAsync();
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
                var res = fundoo.Notes.FirstOrDefault(u => u.NoteId == noteId && u.UserId == userId);
                fundoo.Notes.Remove(res);
                await fundoo.SaveChangesAsync();
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
                var res = fundoo.Notes.FirstOrDefault(u => u.NoteId == noteId && u.UserId == userId);
                if (res != null)
                {
                    res.Title = notePostModel.Title;
                    res.Description = notePostModel.Description;
                    res.RegisterDate = DateTime.Now;
                    res.ModifiedDAte = DateTime.Now;
                    res.BGColor = notePostModel.BGColor;
                    res.IsArchive = notePostModel.IsArchive;
                    res.IsReminder = notePostModel.IsReminder;
                    res.IsPin = notePostModel.IsPin;
                    res.IsTrash = notePostModel.IsTrash;
                    await fundoo.SaveChangesAsync();

                    return await fundoo.Notes.Where(a => a.NoteId == noteId).FirstOrDefaultAsync();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public async Task<Note> ArchieveNote(int noteId, int userId)
        {
            try
            {
                var res = fundoo.Notes.FirstOrDefault(u => u.NoteId == noteId && u.UserId == userId);
                if (res != null)
                {
                    if (res.IsArchive == false)
                    {
                        res.IsArchive = true;
                    }
                    else
                    {
                        res.IsArchive = false;
                    }
                    await fundoo.SaveChangesAsync();
                    return await fundoo.Notes.Where(a => a.NoteId == noteId)
                        .Include(u => u.User).FirstOrDefaultAsync(); ;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public async Task<Note> PinNote(int noteId, int userId)
        {
            try
            {
                var res = fundoo.Notes.FirstOrDefault(u => u.NoteId == noteId && u.UserId == userId);
                if (res != null)
                {
                    if (res.IsPin == false)
                    {
                        res.IsPin = true;
                    }
                    if (res.IsPin == true)
                    {
                        res.IsPin = false;
                    }
                    await fundoo.SaveChangesAsync();
                    return await fundoo.Notes.Where(a => a.NoteId == noteId).FirstOrDefaultAsync();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<Note> TrashNote(int noteId, int userId)
        {
            try
            {
                var res = fundoo.Notes.FirstOrDefault(u => u.NoteId == noteId && u.UserId == userId);
                if (res != null)
                {
                    if (res.IsTrash == false)
                    {
                        res.IsTrash = true;
                    }
                    if (res.IsTrash == true)
                    {
                        res.IsTrash = false;
                    }
                    await fundoo.SaveChangesAsync();
                    return await fundoo.Notes.Where(a => a.NoteId == noteId).FirstOrDefaultAsync();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<Note> ChangeColor(int noteId, int userId, string newColor)
        {
            try
            {
                var res = fundoo.Notes.FirstOrDefault(u => u.NoteId == noteId && u.UserId == userId);
                if (res != null)
                {
                    res.BGColor = newColor;
                    await fundoo.SaveChangesAsync();
                    return await fundoo.Notes.Where(a => a.NoteId == noteId).FirstOrDefaultAsync();
                }
                else
                {
                    return null;
                }
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
                return await fundoo.Notes.Where(u => u.UserId == userId).Include(u => u.User).ToListAsync();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}

