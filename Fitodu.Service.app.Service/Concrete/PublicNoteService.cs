using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Fitodu.Core.Enums;
using Fitodu.Model;
using Fitodu.Model.Entities;
using Fitodu.Service.Abstract;
using Fitodu.Service.Models;
using Fitodu.Service.Models.PublicNote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;

namespace Fitodu.Service.Concrete
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

        public async Task<Result<ICollection<PublicNoteOutput>>> GetAllNotes(string coachId)
        {

            var result = new Result<ICollection<PublicNoteOutput>>();

            IQueryable notes = _context.PublicNotes.Where(x => x.IdCoach == coachId);

            result.Data = await notes.ProjectTo<PublicNoteOutput>(_mapper.ConfigurationProvider)
                .ToListAsync();

            if (result.Data == null)
            {
                result.Error = ErrorType.NotFound;
                result.ErrorMessage = "Couldn't find any public notes of this coach";
            }
            return result;
        }

        public async Task<Result<PublicNoteOutput>> GetClientsNote(string clientId, string requesterId)
        {

            var result = new Result<PublicNoteOutput>();
            IQueryable note = null;

            if (clientId == requesterId)
            {
                note = _context.PublicNotes.Where(x => x.IdClient == clientId);

            }
            else
            {
                note = _context.PublicNotes.Where(x => x.IdClient == clientId && x.IdCoach == requesterId);
            }

            result.Data = await note.ProjectTo<PublicNoteOutput>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();

            if (result.Data == null)
            {
                result.Error = ErrorType.NotFound;
                result.ErrorMessage = "Couldn't finde public note of this client";
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
                result.ErrorMessage = "This coach isn't related to that client.";
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
                    result.ErrorMessage = "Couldn't save changes to the database.";
                    return result;
                }
                transaction.Commit();
            }
            return result;
        }

        public async Task<Result> UpdateNote(string coachId, PublicNoteInput note)
        {
            var result = new Result();
            using (var transaction = _context.Database.BeginTransaction())
            {
                PublicNote existingNote = await _context.PublicNotes
                .Where(x => x.IdCoach == coachId && x.IdClient == note.IdClient)
                .FirstOrDefaultAsync();
                PublicNote _tmpExistingNote = existingNote;
                if (existingNote == null)
                {
                    result.Error = ErrorType.BadRequest;
                    result.ErrorMessage = "this coach does not have a public note for that client";
                    return result;
                }

                existingNote.Note = note.Note;
                _context.PublicNotes.Update(existingNote);
                if (await _context.SaveChangesAsync() == 0)
                {
                    if (existingNote.Equals(_tmpExistingNote))
                    {
                        transaction.Commit();
                        return result;
                    }
                    else
                    {
                        transaction.Rollback();
                        result.Error = ErrorType.InternalServerError;
                        result.ErrorMessage = "Couldn't save changes to the database.";
                        return result;
                    }
                }
                else
                {
                    transaction.Commit();
                }
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
                    result.ErrorMessage = "Couldn't save changes to the database.";
                    return result;
                }
                transaction.Commit();
            }
            return result;
        }
    }
}
