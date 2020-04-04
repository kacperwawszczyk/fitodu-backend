using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Fitodu.Core.Enums;
using Fitodu.Model;
using Fitodu.Model.Entities;
using Fitodu.Service.Abstract;
using Fitodu.Service.Models;
using Fitodu.Service.Models.PrivateNote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;

namespace Fitodu.Service.Concrete
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

        public async Task<Result<ICollection<PrivateNoteOutput>>> GetAllNotes(string Id)
        {
            var result = new Result<ICollection<PrivateNoteOutput>>();

            IQueryable notes = null;
            notes = _context.PrivateNotes.Where(x => x.IdCoach == Id);
            result.Data = await notes.ProjectTo<PrivateNoteOutput>(_mapper.ConfigurationProvider).ToListAsync();

            if (result.Data == null)
            {
                result.Error = ErrorType.NotFound;
                result.ErrorMessage = "Couldn't find any private notes";
            }
            return result;
        }

        public async Task<Result<PrivateNoteOutput>> GetClientsNote(string coachId, string clientId)
        {
            var result = new Result<PrivateNoteOutput>();

            IQueryable note = null;
            note = _context.PrivateNotes.Where(x => x.IdCoach == coachId && x.IdClient == clientId);
            result.Data = await note.ProjectTo<PrivateNoteOutput>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();

            if (result.Data == null)
            {
                result.Error = ErrorType.NotFound;
                result.ErrorMessage = "Couldn't find any notes of client with given id.";
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
                result.ErrorMessage = "This coach isn't related with that client.";
                return result;
            }

            using (var transaction = _context.Database.BeginTransaction())
            {
                PrivateNote existingNote = await _context.PrivateNotes
                .Where(x => x.IdCoach == coachId && x.IdClient == noteInput.IdClient)
                .FirstOrDefaultAsync();

                if (existingNote != null)
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
                    result.ErrorMessage = "Couldn't save changes to the database.";
                    return result;
                }
                transaction.Commit();
            }
            return result;
        }

        public async Task<Result> UpdateNote(string coachId, PrivateNoteInput noteInput)
        {
            var result = new Result();

            using (var transaction = _context.Database.BeginTransaction())
            {
                PrivateNote existingNote = await _context.PrivateNotes
                 .Where(x => x.IdCoach == coachId && x.IdClient == noteInput.IdClient)
                 .FirstOrDefaultAsync();

                if (existingNote == null)
                {
                    result.Error = ErrorType.BadRequest;
                    result.ErrorMessage = "this coach does not have a private note for that client";
                    return result;
                }

                existingNote.Note = noteInput.Note;
                if (await _context.SaveChangesAsync() == 0)
                {
                    transaction.Rollback();
                    result.Error = ErrorType.InternalServerError;
                    result.ErrorMessage = "Couldn't save changes to the database.";
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
                    result.ErrorMessage = "Couldn't save changes to the database.";
                    return result;
                }
                transaction.Commit();
            }
            return result;
        }
    }
}
