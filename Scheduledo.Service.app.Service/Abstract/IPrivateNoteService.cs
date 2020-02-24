using Scheduledo.Model.Entities;
using Scheduledo.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Scheduledo.Service.Abstract
{
    public interface IPrivateNoteService
    {
        Task<Result<ICollection<PrivateNote>>> GetAllNotes(string Id);
        Task<Result<PrivateNote>> GetClientsNote(string coachId, string clientId);
        Task<Result> CreateNote(PrivateNote note);
        Task<Result> UpdateNote(PrivateNote note);
        Task<Result> DeleteNote(string coachId, string clientId);
    }
}
