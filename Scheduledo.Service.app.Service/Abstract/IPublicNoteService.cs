using Scheduledo.Model.Entities;
using Scheduledo.Service.Models;
using Scheduledo.Service.Models.PublicNote;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Scheduledo.Service.Abstract
{
    public interface IPublicNoteService
    {
        Task<Result<ICollection<PublicNote>>> GetAllNotes(string coachId);
        Task<Result<PublicNote>> GetClientsNote(string clientId, string requesterId);
        Task<Result> CreateNote(string coachId, PublicNoteInput note);
        Task<Result> UpdateNote(string coachId, PublicNoteInput note);
        Task<Result> DeleteNote(string coachId, string clientId);
    }
}
