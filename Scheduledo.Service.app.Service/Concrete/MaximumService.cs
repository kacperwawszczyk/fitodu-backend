using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Scheduledo.Core.Enums;
using Scheduledo.Model;
using Scheduledo.Model.Entities;
using Scheduledo.Service.Abstract;
using Scheduledo.Service.Models;
using Scheduledo.Service.Models.Maximum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduledo.Service.Concrete
{
    public class MaximumService : IMaximumService
    {

        private readonly IMapper _mapper;
        private readonly Context _context;

        public MaximumService(Context context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<ICollection<Maximum>>> GetAllMaximums(string IdCoach, string IdClient)
        {
            var result = new Result<ICollection<Maximum>>();

            var max = await _context.Maximums.Where(x => x.IdClient == IdClient).ToListAsync();

            CoachClient coachClient = await _context.CoachClients
                .Where(x => x.IdCoach == IdCoach && x.IdClient == IdClient)
                .FirstOrDefaultAsync();

            if (coachClient == null)
            {
                result.Error = ErrorType.BadRequest;
                result.ErrorMessage = "This client doesn't belong to the current coach";
                return result;
            }

            if (max != null)
            {
                result.Data = max;
            }
            else
            {
                result.Error = ErrorType.NoContent;
            }
            return result;
        }

        public async Task<Result<Maximum>> GetClientMaximum(string IdCoach, string IdClient, int IdExercise)
        {
            var result = new Result<Maximum>();

            var max = await _context.Maximums
                .Where(x => x.IdClient == IdClient && x.IdExercise == IdExercise)
                .FirstOrDefaultAsync();

            CoachClient coachClient = await _context.CoachClients
                .Where(x => x.IdCoach == IdCoach && x.IdClient == IdClient)
                .FirstOrDefaultAsync();

            if (coachClient == null)
            {
                result.Error = ErrorType.BadRequest;
                result.ErrorMessage = "This client doesn't belong to the current coach";
                return result;
            }

            if (max != null)
            {
                result.Data = max;
            }
            else
            {
                result.Error = ErrorType.NoContent;
            }
            return result;
        }

        public async Task<Result> CreateMaximum(string IdCoach, CreateMaximumInput max)
        {
            var result = new Result();

            CoachClient coachClient = await _context.CoachClients
                .Where(x => x.IdCoach == IdCoach && x.IdClient == max.IdClient)
                .FirstOrDefaultAsync();

            Maximum existingMaximum = await _context.Maximums
                .Where(x => x.IdClient == max.IdClient && x.IdExercise == max.IdExercise)
                .FirstOrDefaultAsync();

            if (coachClient == null)
            {
                result.Error = ErrorType.BadRequest;
                result.ErrorMessage = "This client doesn't belong to the current coach";
                return result;
            }

            if (existingMaximum != null)
            {
                result.Error = ErrorType.BadRequest;
                result.ErrorMessage = "This client already has a maximum with given IdExercise or IdClient/IdExercise doesn't exist";
                return result;
            }

            Maximum _max = new Maximum();
            _max.IdClient = max.IdClient;
            _max.IdExercise = max.IdExercise;
            _max.Max = max.Max;
            _context.Maximums.Add(_max);

            if (await _context.SaveChangesAsync() > 0)
            {
                result = new Result(true);
            }
            else
            {
                result.Error = ErrorType.BadRequest;
            }
            return result;
        }

        public async Task<Result> UpdateMaximum(string IdCoach, Maximum max)
        {
            var result = new Result();

            if (max == null)
            {
                result.Error = ErrorType.BadRequest;
                return result;
            }

            CoachClient coachClient = await _context.CoachClients
                .Where(x => x.IdCoach == IdCoach && x.IdClient == max.IdClient)
                .FirstOrDefaultAsync();

            Maximum existingMaximum = await _context.Maximums
                .Where(x => x.IdClient == max.IdClient && x.IdExercise == max.IdExercise)
                .FirstOrDefaultAsync();

            if (coachClient == null)
            {
                result.Error = ErrorType.BadRequest;
                result.ErrorMessage = "This client doesn't belong to the current coach";
                return result;
            }

            if (existingMaximum == null)
            {
                result.Error = ErrorType.BadRequest;
                result.ErrorMessage = "Maximum with given IdClient and IdExercise doesn't exist";
                return result;
            }
            else
            {
                existingMaximum.IdClient = max.IdClient;
                existingMaximum.IdExercise = max.IdExercise;
                existingMaximum.Max = max.Max;
                try
                {
                    if (await _context.SaveChangesAsync() > 0)
                    {
                        result = new Result(true);
                    }
                    return result;
                }
                catch (DbUpdateConcurrencyException)
                {
                    result.Error = ErrorType.InternalServerError;
                    return result;
                }
            }
        }

        public async Task<Result> DeleteMaximum(string IdCoach, Maximum max)
        {
            var result = new Result();

            if (max == null)
            {
                result.Error = ErrorType.BadRequest;
                return result;
            }

            CoachClient coachClient = await _context.CoachClients
                .Where(x => x.IdCoach == IdCoach && x.IdClient == max.IdClient)
                .FirstOrDefaultAsync();

            Maximum existingMaximum = await _context.Maximums
                .Where(x => x.IdClient == max.IdClient && x.IdExercise == max.IdExercise)
                .FirstOrDefaultAsync();

            if (coachClient == null)
            {
                result.Error = ErrorType.BadRequest;
                result.ErrorMessage = "This client doesn't belong to the current coach";
                return result;
            }

            if (existingMaximum == null)
            {
                result.Error = ErrorType.BadRequest;
                result.ErrorMessage = "Maximum with given IdClient and IdExercise doesn't exist";
                return result;
            }
            else
            {
                _context.Maximums.Remove(existingMaximum);
                try
                {
                    if (await _context.SaveChangesAsync() > 0)
                    {
                        result = new Result(true);
                    }
                    return result;
                }
                catch (DbUpdateConcurrencyException)
                {
                    result.Error = ErrorType.InternalServerError;
                    return result;
                }
            }
        }

    }
}
