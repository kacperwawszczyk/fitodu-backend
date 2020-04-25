using AutoMapper;
using AutoMapper.QueryableExtensions;
using Fitodu.Core.Enums;
using Fitodu.Model;
using Fitodu.Model.Entities;
using Fitodu.Service.Abstract;
using Fitodu.Service.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitodu.Service.Concrete
{
    public class SummaryService : ISummaryService
    {
        private readonly IMapper _mapper;
        private readonly Context _context;

        public SummaryService(Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<ICollection<SummaryOutput>>> GetAllSummaries(string IdCoach, string IdClient)
        {
            var result = new Result<ICollection<SummaryOutput>>();

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

            IQueryable sum = null;
            sum = _context.Summaries.Where(x => x.IdClient == IdClient);

            result.Data = await sum
                .ProjectTo<SummaryOutput>(_mapper.ConfigurationProvider)
                .ToListAsync();

            if (result.Data == null)
            {
                result.Error = ErrorType.NotFound;
            }

            return result;
        }

        public async Task<Result<SummaryOutput>> GetClientSummary(string IdCoach, string IdClient, int Id)
        {
            var result = new Result<SummaryOutput>();

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

            Summary s = await _context.Summaries
                .Where(x => x.IdClient == IdClient && x.Id == Id)
                .FirstOrDefaultAsync();

            if (s == null)
            {
                result.Error = ErrorType.NotFound;
                return result;
            }

            IQueryable sum = null;
            sum = _context.Summaries.Where(x => x.IdClient == IdClient && x.Id == Id);

            result.Data = await sum
                .ProjectTo<SummaryOutput>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            if (result.Data == null)
            {
                result.Error = ErrorType.NotFound;
            }

            return result;
        }

        public async Task<Result> CreateSummary(string IdCoach, SummaryInput sum)
        {
            var result = new Result();

            if (sum == null)
            {
                result.Error = ErrorType.BadRequest;
                return result;
            }

            CoachClient coachClient = await _context.CoachClients
                .Where(x => x.IdCoach == IdCoach && x.IdClient == sum.IdClient)
                .FirstOrDefaultAsync();

            if (coachClient == null)
            {
                result.Error = ErrorType.BadRequest;
                result.ErrorMessage = "This client doesn't belong to the current coach";
                return result;
            }

            Client existingClient = await _context.Clients
                .Where(x => x.Id == sum.IdClient)
                .FirstOrDefaultAsync();

            Summary _sum = new Summary();
            _sum.IdClient = sum.IdClient;
            _sum.Weight = sum.Weight;
            _sum.FatPercentage = sum.FatPercentage;
            _sum.Description = sum.Description;
            _sum.Date = sum.Date;

            existingClient.Weight = sum.Weight;
            existingClient.FatPercentage = sum.FatPercentage;

            using (var transaction = _context.Database.BeginTransaction())
            {
                _context.Summaries.Add(_sum);
                _context.Clients.Update(existingClient);

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

        public async Task<Result> UpdateSummary(string IdCoach, UpdateSummaryInput sum)
        {
            var result = new Result();

            if (sum == null)
            {
                result.Error = ErrorType.BadRequest;
                return result;
            }

            CoachClient coachClient = await _context.CoachClients
                .Where(x => x.IdCoach == IdCoach && x.IdClient == sum.IdClient)
                .FirstOrDefaultAsync();

            Summary existingSummary = await _context.Summaries
                .Where(x => x.Id == sum.Id && x.IdClient == sum.IdClient)
                .FirstOrDefaultAsync();

            Client existingClient = await _context.Clients
                .Where(x => x.Id == sum.IdClient)
                .FirstOrDefaultAsync();

            if (coachClient == null)
            {
                result.Error = ErrorType.BadRequest;
                result.ErrorMessage = "This client doesn't belong to the current coach";
                return result;
            }

            if (existingSummary == null)
            {
                result.Error = ErrorType.BadRequest;
                result.ErrorMessage = "This client doesn't have a summary with given Id or IdClient/Id doesn't exist";
                return result;
            }
            else
            {
                existingSummary.Id = sum.Id;
                existingSummary.IdClient = sum.IdClient;
                existingSummary.Weight = sum.Weight;
                existingSummary.FatPercentage = sum.FatPercentage;
                existingSummary.Description = sum.Description;
                existingSummary.Date = sum.Date;

                existingClient.Weight = sum.Weight;
                existingClient.FatPercentage = sum.FatPercentage;

                using (var transaction = _context.Database.BeginTransaction())
                {
                    _context.Summaries.Update(existingSummary);
                    _context.Clients.Update(existingClient);

                    if (await _context.SaveChangesAsync() == 0)
                    {
                        transaction.Rollback();
                        result.Error = ErrorType.InternalServerError;
                        return result;
                    }

                    transaction.Commit();
                }
            }

            return result;
        }

        public async Task<Result> DeleteSummary(string IdCoach, string IdClient, int Id)
        {
            var result = new Result();

            Summary sum = await _context.Summaries
                .Where(x => x.IdClient == IdClient && x.Id == Id)
                .FirstOrDefaultAsync();

            if (sum == null)
            {
                result.Error = ErrorType.BadRequest;
                return result;
            }

            CoachClient coachClient = await _context.CoachClients
                .Where(x => x.IdCoach == IdCoach && x.IdClient == sum.IdClient)
                .FirstOrDefaultAsync();

            Summary existingSummary = await _context.Summaries
                .Where(x => x.IdClient == sum.IdClient && x.Id == sum.Id)
                .FirstOrDefaultAsync();

            if (coachClient == null)
            {
                result.Error = ErrorType.BadRequest;
                result.ErrorMessage = "This client doesn't belong to the current coach";
                return result;
            }

            if (existingSummary == null)
            {
                result.Error = ErrorType.BadRequest;
                result.ErrorMessage = "Summary with given Id and IdClient doesn't exist";
                return result;
            }
            else
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    _context.Summaries.Remove(existingSummary);

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
