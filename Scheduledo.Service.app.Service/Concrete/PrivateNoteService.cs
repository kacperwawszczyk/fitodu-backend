using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Scheduledo.Core.Enums;
using Scheduledo.Model;
using Scheduledo.Model.Entities;
using Scheduledo.Service.Abstract;
using Scheduledo.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduledo.Service.Concrete
{
    public class PrivateNoteService : IPrivateNoteService
    {
        private readonly IMapper _mapper;
        private readonly Context _context;
        

        public PrivateNoteService(Context context,
           IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task <Result<List<PrivateNote>>> GetAllNotes(string Id)
        {

            var result = new Result<List<PrivateNote>>();

            var notes = await _context.PrivateNotes.Where(x => x.IdCoach == Id)
                                                  .ToListAsync();

            if(notes != null)
            {
                result.Data = notes;

            }
            else
            {
                result.Error = ErrorType.NoContent; //może inny?
            }
            return result;
        }

        public async Task<Result<PrivateNote>> GetClientsNote(string coachId, string clientId)
        {

            var result = new Result<PrivateNote>();
            var note = await _context.PrivateNotes.FindAsync(coachId, clientId);

            if (note != null)
            {
                result.Data = note;
            }
            else
            {
                result.Error = ErrorType.NoContent; //może inny?
            }
            return result;
        }

        public async Task<Result> CreateNote(PrivateNote note)
        {
            var result = new Result();
            _context.PrivateNotes.Add(note);

            if (await _context.SaveChangesAsync() > 0)
            {
                result = new Result(true);
            }
            else
            {
                result.Error = ErrorType.BadRequest; //może być co innego, może dodać nowy?
            }
            return result;
        }

        public async Task<Result> UpdateNote(PrivateNote note)
        {
            var result = new Result();

            var noteInDatabase = await _context.PrivateNotes.FindAsync(note.IdCoach, note.IdClient);
            //var noteInDatabase = _context.PrivateNotes.
            //    Where(x => x.IdCoach == note.IdCoach && x.IdClient == note.IdClient).FirstOrDefaultAsync<PrivateNote>();

           // _context.Entry(note).State = EntityState.Modified;

            if(noteInDatabase != null)
            {
                noteInDatabase.Note = note.Note;
                try
                {
                    if (await _context.SaveChangesAsync() > 0)
                    {
                        result = new Result(true);
                    }
                    return result;
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (noteInDatabase != null)
                    {
                        result.Error = ErrorType.NotFound; //może być co innego, może dodać nowy?
                        return result;
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            else
            {
                result.Error = ErrorType.NotFound;
                return result;
            }


        }

        public async Task<Result> DeleteNote(string coachId, string clientId)
        {
            var result = new Result();

            var noteInDatabase = await _context.PrivateNotes.FindAsync(coachId, clientId);

            if (noteInDatabase != null)
            {
                _context.PrivateNotes.Remove(noteInDatabase);
                try
                {
                    if (await _context.SaveChangesAsync() > 0)
                    {
                        result = new Result(true);
                    }
                    return result;
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (noteInDatabase != null)
                    {
                        result.Error = ErrorType.NotFound;//może być co innego, może dodać nowy?
                        return result;
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            else
            {
                result.Error = ErrorType.NotFound;
                return result;
            }
        }
    }
}
