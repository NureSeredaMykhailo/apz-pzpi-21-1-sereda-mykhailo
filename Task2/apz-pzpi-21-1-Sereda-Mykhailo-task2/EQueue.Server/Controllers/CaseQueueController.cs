using AutoMapper;
using EQueue.Db;
using EQueue.Db.Models;
using EQueue.Predictor.Models;
using EQueue.Predictor.Services;
using EQueue.Shared.Dto;
using EQueue.Shared.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace EQueue.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CaseQueueController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly PredictorService _predictorService;

        public CaseQueueController(ApplicationDbContext context, IMapper mapper, PredictorService predictorService)
        {
            _context = context;
            _mapper = mapper;
            _predictorService = predictorService;
        }

        [HttpGet("filter")]
        public async Task<IActionResult> GetFilteredCaseQueues(long dateUnix, CancellationToken cancellationToken)
        {
            try
            {
                var casePriorities = await _context.CasePriorities
                    .Include(cp => cp.Case)
                    .ThenInclude(c => c.Traumas)
                    .ThenInclude(c => c.TraumaPlace)
                    .Include(cp => cp.Case)
                    .ThenInclude(c => c.Traumas)
                    .ThenInclude(c => c.TraumaType)
                    .Where(cp => cp.PriorityPeriodStartUnix <= dateUnix && cp.PriorityPeriodEndUnix >= dateUnix)
                    .OrderByDescending(cp => cp.CombinedPriority)
                    .ToListAsync(cancellationToken);

                var caseQueueDtos = _mapper.Map<List<CaseQueueDto>>(casePriorities);
                return Ok(caseQueueDtos);
            }
            catch
            {
                return BadRequest(new { Message = "An error occurred while fetching case queues." });
            }
        }

        [HttpPost("new")]
        public async Task<IActionResult> EditCase(CaseDto caseDto, CancellationToken cancellationToken)
        {
            try
            {
                var caseEntity = _mapper.Map<Case>(caseDto);
                caseEntity.Clinic = null;
                _context.Cases.Update(caseEntity);
                await _context.SaveChangesAsync(cancellationToken);
                await AddQueuePriorities(caseEntity);

                return Ok(_mapper.Map<CaseDto>(caseEntity));
            }
            catch
            {
                throw new UnknownErrorException();
            }
        }

        private async Task AddQueuePriorities(Case caseEntity)
        {
            var timeMarks = GetNextHalfHourMarks(7);
            var predicts = timeMarks.Select(m => GetPrediction(caseEntity, m)).ToList();

            var queueRecords = new List<CasePriority>();
            for (int i = 0; i < 5; i++)
            {
                queueRecords.Add(CreatePriority(predicts, i, caseEntity));
            }

            _context.CasePriorities.AddRange(queueRecords);
            await _context.SaveChangesAsync();
        }

        private PredictOutput GetPrediction(Case caseEntity, long startedTreatmentTime)
        {
            var damageInput = _mapper.Map<DamagePredictInput>(caseEntity);
            damageInput.TreatmentStartDelayMinutes = CalculateTreatmentStartDelayMinutes(caseEntity.TraumasRegisteredUnixTime, startedTreatmentTime);

            var deathInput = _mapper.Map<DeathPredictInput>(caseEntity);
            deathInput.TreatmentStartDelayMinutes = CalculateTreatmentStartDelayMinutes(caseEntity.TraumasRegisteredUnixTime, startedTreatmentTime);

            return _predictorService.Predict(damageInput, deathInput);
        }

        private int CalculateTreatmentStartDelayMinutes(long registeredUnixTime, long startedTreatmentUnixTime)
        {
            DateTime registeredTime = DateTimeOffset.FromUnixTimeSeconds(registeredUnixTime).UtcDateTime;
            DateTime startedTime = DateTimeOffset.FromUnixTimeSeconds(startedTreatmentUnixTime).UtcDateTime;

            return (int)(startedTime - registeredTime).TotalMinutes;
        }

        private List<long> GetNextHalfHourMarks(int amount)
        {
            var result = new List<long>();
            var currentDateTime = DateTime.UtcNow;
            var minutes = currentDateTime.Minute;

            var nextHalfHour = currentDateTime.AddMinutes(minutes < 30 ? -minutes : -(minutes - 30)).AddSeconds(-currentDateTime.Second).AddMilliseconds(-currentDateTime.Millisecond);

            for (int i = 0; i < amount; i++)
            {
                result.Add(((DateTimeOffset)nextHalfHour).ToUnixTimeSeconds());
                nextHalfHour = nextHalfHour.AddMinutes(30);
            }

            return result;
        }

        private CasePriority CreatePriority(List<PredictOutput> predictOutputs, int index, Case caseEntity)
        {            
            var result = new CasePriority()
            {
                CaseId = caseEntity.Id,
                PriorityPeriodStartUnix = AddMinutesToUnixTime(caseEntity.TraumasRegisteredUnixTime, (int)predictOutputs[index].TreatmentStartDelayMinutes),
                PriorityPeriodEndUnix = AddMinutesToUnixTime(caseEntity.TraumasRegisteredUnixTime, (int)predictOutputs[index + 1].TreatmentStartDelayMinutes),
            };

            for (int i = index; i < index + 2; i++)
            {
                result.DamagePriority += predictOutputs[i].DamageProbability;
                result.DeathPriority += 1 - predictOutputs[i].SurvivedProbability;
            }
            result.DamagePriority = result.DamagePriority / 2 * 5;
            result.DeathPriority = result.DeathPriority / 2 * 5;
            result.CombinedPriority = (result.DeathPriority + result.DamagePriority * 0.3f) / 2.3f;

            return result;
        }

        private long AddMinutesToUnixTime(long unixTime, int minutesToAdd)
        {
            DateTime dateTime = DateTimeOffset.FromUnixTimeSeconds(unixTime).UtcDateTime;
            DateTime newDateTime = dateTime.AddMinutes(minutesToAdd);
            return ((DateTimeOffset)newDateTime).ToUnixTimeSeconds();
        }

        private float GetDeathPriority(PredictOutput predict)
        {
            return predict.Survived ? predict.SurvivedProbability : 1 - predict.SurvivedProbability;
        }

        private float GetDamagePriority(PredictOutput predict)
        {
            return predict.GotIncurableDamage ? predict.DamageProbability : 1 - predict.DamageProbability;
        }
    }
}
