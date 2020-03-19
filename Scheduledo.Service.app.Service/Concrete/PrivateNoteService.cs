using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Scheduledo.Core.Enums;
using Scheduledo.Model;
using Scheduledo.Model.Entities;
using Scheduledo.Service.Abstract;
using Scheduledo.Service.Models;
using Scheduledo.Service.Models.PrivateNote;
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

        public async Task <Result<ICollection<PrivateNote>>> GetAllNotes(string Id)
        {

            var result = new Result<ICollection<PrivateNote>>();

            var notes = await _context.PrivateNotes.Where(x => x.IdCoach == Id)
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
                result.Error = ErrorType.NotFound;
            }
            return result;
        }

        public async Task<Result> CreateNote(string coachId, PrivateNoteInput noteInput)
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
                PrivateNote existingNote = await _context.PrivateNotes
                .Where(x => x.IdCoach == coachId && x.IdClient == noteInput.IdClient)
                .FirstOrDefaultAsync();

                if(existingNote != null)
                {
                    result.Error = ErrorType.BadRequest;
                    result.ErrorMessage = "this coach already has a private note for that client";
                    return result;
                }

                PrivateNote note = new PrivateNote();

                note.IdCoach = coachId;
                note.IdClient = noteInput.IdClient;
                note.Note = noteInput.Note;
            
                _context.PrivateNotes.Add(note);
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

        public async Task<Result> UpdateNote(PrivateNote note)
        {
            var result = new Result();

            using (var transaction = _context.Database.BeginTransaction())
            {
                PrivateNote existingNote = await _context.PrivateNotes
                 .Where(x => x.IdCoach == note.IdCoach && x.IdClient == note.IdClient)
                 .FirstOrDefaultAsync();

                if (existingNote == null)
                {
                    result.Error = ErrorType.BadRequest;
                    result.ErrorMessage = "this coach does not have a private note for that client";
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
                PrivateNote existingNote = await _context.PrivateNotes
                 .Where(x => x.IdCoach == coachId && x.IdClient == clientId)
                 .FirstOrDefaultAsync();

                if (existingNote == null)
                {
                    result.Error = ErrorType.BadRequest;
                    result.ErrorMessage = "this coach does not have a private note for that client";
                    return result;
                }

                _context.PrivateNotes.Remove(existingNote);
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
