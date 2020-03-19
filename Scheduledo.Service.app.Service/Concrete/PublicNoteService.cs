using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Scheduledo.Core.Enums;
using Scheduledo.Model;
using Scheduledo.Model.Entities;
using Scheduledo.Service.Abstract;
using Scheduledo.Service.Models;
using Scheduledo.Service.Models.PublicNote;
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
                result.Error = ErrorType.NotFound;
            }
            return result;
        }

        public async Task<Result<PublicNote>> GetClientsNote(string clientId, string requesterId)
        {

            var result = new Result<PublicNote>();
            var note = new PublicNote();
            if( clientId == requesterId)
            {
                note = await _context.PublicNotes.Where(x => x.IdClient == clientId).FirstOrDefaultAsync();

            }
            else
            {
                note = await _context.PublicNotes.Where(x => x.IdClient == clientId && x.IdCoach == requesterId).FirstOrDefaultAsync();
            }

            if (note != null)
            {
                result.Data = note;
            }
            else
            {
                result.Error = ErrorType.NotFound;
            }
            return result;

        }

        public async Task<Result> CreateNote(string coachId, PublicNoteInput noteInput)
        {
            var result = new Result();
            var clientsCoach = await _context.CoachClients.FirstOrDefaultAsync(x => x.IdCoach == coachId && x.IdClient == noteInput.IdClient);
            if (clientsCoach == null)
            {
                result.Error = ErrorType.BadRequest;
                return result;
            }

            using (var transaction = _context.Database.BeginTransaction())
            {
                PublicNote existingNote = await _context.PublicNotes
                .Where(x => x.IdCoach == coachId && x.IdClient == noteInput.IdClient)
                .FirstOrDefaultAsync();

                if (existingNote != null)
                {
                    result.Error = ErrorType.BadRequest;
                    result.ErrorMessage = "this coach already has a public note for that client";
                    return result;
                }

                PublicNote note = new PublicNote();
                note.IdCoach = coachId;
                note.IdClient = noteInput.IdClient;
                note.Note = noteInput.Note;

                _context.PublicNotes.Add(note);
                if (await _context.SaveChangesAsync() == 0)
                {
                    transaction.Rollback();
                    result.Error = ErrorType.InternalServerError;
                    return result;
                }
                transaction.Commit();
            }
            return result;
        }

        public async Task<Result> UpdateNote(PublicNote note)
        {
            var result = new Result();
            using (var transaction = _context.Database.BeginTransaction())
            {
                PublicNote existingNote = await _context.PublicNotes
                .Where(x => x.IdCoach == note.IdCoach && x.IdClient == note.IdClient)
                .FirstOrDefaultAsync();

                if (existingNote == null)
                {
                    result.Error = ErrorType.BadRequest;
                    result.ErrorMessage = "this coach does not have a public note for that client";
                    return result;
                }

                existingNote.Note = note.Note;

                if (await _context.SaveChangesAsync() == 0)
                {
                    transaction.Rollback();
                    result.Error = ErrorType.InternalServerError;
                    return result;
                }
                transaction.Commit();
            }
            return result;
        }

        public async Task<Result> DeleteNote(string coachId, string clientId)
        {
            var result = new Result();
            using (var transaction = _context.Database.BeginTransaction())
            {
                PublicNote existingNote = await _context.PublicNotes
                .Where(x => x.IdCoach == coachId && x.IdClient == clientId)
                .FirstOrDefaultAsync();

                if (existingNote == null)
                {
                    result.Error = ErrorType.BadRequest;
                    result.ErrorMessage = "this coach does not have a public note for that client";
                    return result;
                }

                _context.PublicNotes.Remove(existingNote);
                if (await _context.SaveChangesAsync() == 0)
                {
                    transaction.Rollback();
                    result.Error = ErrorType.InternalServerError;
                    return result;
                }
                transaction.Commit();
            }
            return result;
        }
    }
}
