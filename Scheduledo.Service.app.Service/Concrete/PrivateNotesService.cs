using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Scheduledo.Model;
using Scheduledo.Model.Entities;
using Scheduledo.Service.Abstract;
using Scheduledo.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduledo.Service.Concrete
{
    public class PrivateNotesService : IPrivateNotesService
    {
        private readonly IMapper _mapper;
        private readonly Context _context;

        public PrivateNotesService(Context context,
           IMapper mapper)
        {
            _context = context ;
            _mapper = mapper;
        }
        
        public async Task <List<PrivateNote> > GetAllNotes(string Id)
        {

            //var result = new Result<PrivateNote>();

            var note = await _context.PrivateNotes.Where(x => x.IdCoach == Id)
                                                  .ToListAsync();

            //var data = _mapper.Map<>
            return note;
        }
    }
}
