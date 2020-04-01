using Fitodu.Model.Entities;
using Fitodu.Service.Models;
using Fitodu.Service.Models.PublicNote;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fitodu.Service.Abstract
{
    public interface IPublicNoteService
    {
        Task<Result<ICollection<PublicNoteOutput>>> GetAllNotes(string coachId);
        Task<Result<PublicNoteOutput>> GetClientsNote(string clientId, string requesterId);
        Task<Result> CreateNote(string coachId, PublicNoteInput note);
        Task<Result> UpdateNote(string coachId, PublicNoteInput note);
        Task<Result> DeleteNote(string coachId, string clientId);
    }
}
