using Scheduledo.Model.Entities;
using Scheduledo.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Scheduledo.Service.Abstract
{
    public interface IPrivateNotesService
    {
        Task<List<PrivateNote>> GetAllNotes(string Id);
    }
}
