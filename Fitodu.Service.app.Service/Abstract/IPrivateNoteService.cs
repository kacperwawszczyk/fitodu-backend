using Fitodu.Model.Entities;
using Fitodu.Service.Models;
using Fitodu.Service.Models.PrivateNote;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fitodu.Service.Abstract
{
    public interface IPrivateNoteService
    {
        Task<Result<ICollection<PrivateNote>>> GetAllNotes(string Id);
        Task<Result<PrivateNote>> GetClientsNote(string coachId, string clientId);
        Task<Result> CreateNote(string coachId, PrivateNoteInput noteInput);
        Task<Result> UpdateNote(string coachId, PrivateNoteInput note);
        Task<Result> DeleteNote(string coachId, string clientId);
    }
}
