using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Scheduledo.Core.Enums;
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
    public class TrainingService : ITrainingService
    {
        private readonly IMapper _mapper;
        private readonly Context _context;

        public TrainingService(Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<string>> GetTrainingsCoach(int idTraining)
        {
            var result = new Result<string>();

            Training training = await _context.Trainings.Where(x => x.Id == idTraining).FirstOrDefaultAsync();

            if(training != null)
            {
                result.Data = training.IdCoach;
            }
            else
            {
                result.Error = ErrorType.NotFound; //może inny?
            }
            return result;
        }
    }
}
