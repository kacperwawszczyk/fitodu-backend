using Scheduledo.Model.Entities;
using Scheduledo.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Scheduledo.Service.Abstract
{
    public interface IPublicNoteService
    {
        Task<List<PublicNote>> GetAllNotes(string Id);
        Task<PublicNote> GetClientsNote(string coachId, string clientId);
        Task<Result> CreateNote(PublicNote note);
        Task<Result> UpdateNote(PublicNote note);
        Task<Result> DeleteNote(string coachId, string clientId);
    }
}
