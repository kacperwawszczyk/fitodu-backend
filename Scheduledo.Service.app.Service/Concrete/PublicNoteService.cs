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
    public class PublicNoteService : IPublicNoteService
    {
        private readonly IMapper _mapper;
        private readonly Context _context;
        

        public PublicNoteService(Context context,
           IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task <Result<ICollection<PublicNote>>> GetAllNotes(string coachId)
        {

            var result = new Result<ICollection<PublicNote>>();

            var notes = await _context.PublicNotes.Where(x => x.IdCoach == coachId)
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

        public async Task<Result<PublicNote>> GetClientsNote(string clientId)
        {

            var result = new Result<PublicNote>();
            var note = await _context.PublicNotes.Where(x =>  x.IdClient == clientId).FirstOrDefaultAsync();

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

        public async Task<Result> CreateNote(PublicNote note)
        {
            var result = new Result();
            _context.PublicNotes.Add(note);

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

        public async Task<Result> UpdateNote(PublicNote note)
        {
            var result = new Result();

            var noteInDatabase = await _context.PublicNotes.FindAsync(note.IdCoach, note.IdClient);


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

            var noteInDatabase = await _context.PublicNotes.FindAsync(coachId, clientId);

            if (noteInDatabase != null)
            {
                _context.PublicNotes.Remove(noteInDatabase);
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
