using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Fitodu.Core.Enums;
using Fitodu.Model;
using Fitodu.Model.Entities;
using Fitodu.Service.Abstract;
using Fitodu.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;

namespace Fitodu.Service.Concrete
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

        public async Task<Result<ICollection<MaximumOutput>>> GetAllMaximums(string IdCoach, string IdClient)
        {
            var result = new Result<ICollection<MaximumOutput>>();

            if (IdCoach == null)
            {
                result.Error = ErrorType.BadRequest;
                return result;
            }

            CoachClient coachClient = await _context.CoachClients
                .Where(x => x.IdCoach == IdCoach && x.IdClient == IdClient)
                .FirstOrDefaultAsync();

            if (coachClient == null)
            {
                result.Error = ErrorType.BadRequest;
                result.ErrorMessage = "This client doesn't belong to the current coach";
                return result;
            }

            IQueryable max = null;
            max = _context.Maximums.Where(x => x.IdClient == IdClient);

            result.Data = await max
                    .ProjectTo<MaximumOutput>(_mapper.ConfigurationProvider)
                    .ToListAsync();

            if (result.Data == null)
            {
                result.Error = ErrorType.NotFound;
            }
            return result;
        }

        public async Task<Result<MaximumOutput>> GetClientMaximum(string IdCoach, string IdClient, int IdExercise)
        {
            var result = new Result<MaximumOutput>();

            if (IdCoach == null)
            {
                result.Error = ErrorType.BadRequest;
                return result;
            }

            CoachClient coachClient = await _context.CoachClients
                .Where(x => x.IdCoach == IdCoach && x.IdClient == IdClient)
                .FirstOrDefaultAsync();

            if (coachClient == null)
            {
                result.Error = ErrorType.BadRequest;
                result.ErrorMessage = "This client doesn't belong to the current coach";
                return result;
            }

            Maximum m = await _context.Maximums
                .Where(x => x.IdExercise == IdExercise)
                .FirstOrDefaultAsync();

            if (m == null)
            {
                result.Error = ErrorType.NotFound;
                return result;
            }

            IQueryable max = null;
            max = _context.Maximums.Where(x => x.IdClient == IdClient && x.IdExercise == IdExercise);
            
            result.Data = await max
                    .ProjectTo<MaximumOutput>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();

            if (result.Data == null)
            {
                result.Error = ErrorType.NotFound;
            }
            return result;
        }

        public async Task<Result> CreateMaximum(string IdCoach, MaximumInput max)
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

            using (var transaction = _context.Database.BeginTransaction())
            {
                _context.Maximums.Add(_max);

                if (await _context.SaveChangesAsync() == 0)
                {
                    transaction.Rollback();
                    result.Error = ErrorType.InternalServerError;
                    return result;
                }

                transaction.Commit();
            }

            return result;
        }

        public async Task<Result> UpdateMaximum(string IdCoach, MaximumInput max)
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

                using (var transaction = _context.Database.BeginTransaction())
                {
                    _context.Maximums.Update(existingMaximum);

                    if (await _context.SaveChangesAsync() == 0)
                    {
                        transaction.Rollback();
                        result.Error = ErrorType.InternalServerError;
                        return result;
                    }

                    transaction.Commit();
                }

                return result;
            }
        }

        public async Task<Result> DeleteMaximum(string IdCoach, string IdClient, int IdExercise)
        {
            var result = new Result();

            Maximum max = await _context.Maximums
                .Where(x => x.IdClient == IdClient && x.IdExercise == IdExercise)
                .FirstOrDefaultAsync();

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
                using (var transaction = _context.Database.BeginTransaction())
                {
                    _context.Maximums.Remove(existingMaximum);

                    if (await _context.SaveChangesAsync() == 0)
                    {
                        transaction.Rollback();
                        result.Error = ErrorType.InternalServerError;
                        return result;
                    }

                    transaction.Commit();
                }

                return result;
            }
        }

    }
}
