using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Scheduledo.Core.Enums;
using Scheduledo.Model;
using Scheduledo.Model.Entities;
using Scheduledo.Service.Abstract;
using Scheduledo.Service.Models;
using Scheduledo.Service.Models.Training;
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



        public async Task<Result<ICollection<Training>>> GetCoachsTrainings(string idCoach)
        {
            var result = new Result<ICollection<Training>>();

            var coachsTrainings = await _context.Trainings.Where(x => x.IdCoach == idCoach).ToListAsync();

            if(coachsTrainings != null)
            {
                result.Data = coachsTrainings;
            }
            else
            {
                result.Error = ErrorType.NotFound; //może inny?
            }
            return result;

        }

        public async Task<Result<ICollection<Training>>> GetClientsTrainings(string idClient)
        {
            var result = new Result<ICollection<Training>>();

            var clientsTrainings = await _context.Trainings.Where(x => x.IdClient == idClient).ToListAsync();

            if(clientsTrainings != null)
            {
                result.Data = clientsTrainings;
            }
            else
            {
                result.Error = ErrorType.NotFound; //może inny?
            }
            return result;
        }

        public async Task<Result> AddTraining(TrainingInput trainingInput)
        {
            var result = new Result();

            Training _training = new Training();
            _training.IdClient = trainingInput.IdClient;
            _training.IdCoach = trainingInput.IdCoach;
            if(_training.Note != null) _training.Note = trainingInput.Note;
            _training.Date = trainingInput.Date;
            _training.Description = trainingInput.Description;

            _context.Trainings.Add(_training);
            if (await _context.SaveChangesAsync() > 0)
            {
                result = new Result(true);
            }
            else
            {
                result.Error = ErrorType.BadRequest; //może być co innego, może dodać nowy?
            }
            return result;
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

        public async Task<Result> EditTraining(Training training)
        {
            var result = new Result();

            var exisitngTraining = await _context.Trainings.Where(
                x => x.Id == training.Id).FirstOrDefaultAsync();

            if(exisitngTraining == null)
            {
                result.Error = ErrorType.NotFound; //może inny?
                result.ErrorMessage = "Training with this Id does not exist in the database";
                return result;
            }

            exisitngTraining.IdClient = training.IdClient;
            exisitngTraining.Note = training.Note;
            exisitngTraining.Description = training.Description;
            exisitngTraining.Date = training.Date;

            if (await _context.SaveChangesAsync() > 0)
            {
                result = new Result(true);
            }
            else
            {
                result.Error = ErrorType.BadRequest; //może być co innego, może dodać nowy?
            }
            return result;
        }

        public async Task<Result> DeleteTraining(Training training)
        {
            var result = new Result();
            var exisitngTraining = await _context.Trainings.Where(
                x => x.Id == training.Id).FirstOrDefaultAsync();

            if (exisitngTraining == null)
            {
                result.Error = ErrorType.NotFound; //może inny?
                result.ErrorMessage = "Training with this Id does not exist in the database";
                return result;
            }

            _context.Trainings.Remove(exisitngTraining);

            if (await _context.SaveChangesAsync() > 0)
            {
                result = new Result(true);
            }
            else
            {
                result.Error = ErrorType.BadRequest; //może być co innego, może dodać nowy?
            }
            return result;

        }
    }
}
