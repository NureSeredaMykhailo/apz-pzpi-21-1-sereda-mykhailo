using AutoMapper;
using EQueue.Db;
using EQueue.Predictor.Models;
using EQueue.Server.Controllers.Base;
using EQueue.Shared.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EQueue.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PredictsController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public PredictsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("damage-predict-input/export")]
        public async Task<IActionResult> ExportDamagePredictInput(CancellationToken cancellationToken)
        {
            try
            {
                var cases = await _context.Cases
                    .Include(c => c.Traumas)
                    .ToListAsync(cancellationToken);

        
                var damageDeathPredictModels = _mapper.Map<List<DamagePredictInput>>(cases);
      
                var csvBytes = CsvConverterService.ConvertDamagePredictToCsv(damageDeathPredictModels);
     
                return File(csvBytes, "text/csv", "damage-predict-input.csv");
            }
            catch(Exception ex) 
            {
                throw new UnknownErrorException();
            }
        }

        [HttpGet("death-predict-input/export")]
        public async Task<IActionResult> ExportDeathPredictInput(CancellationToken cancellationToken)
        {
            try
            {
                var cases = await _context.Cases
                    .Include(c => c.Traumas)
                    .ToListAsync(cancellationToken);


                var damageDeathPredictModels = _mapper.Map<List<DeathPredictInput>>(cases);

                var csvBytes = CsvConverterService.ConvertDeathPredictToCsv(damageDeathPredictModels);

                return File(csvBytes, "text/csv", "death-predict-input.csv");
            }
            catch (Exception ex)
            {
                throw new UnknownErrorException();
            }
        }
    }
}
