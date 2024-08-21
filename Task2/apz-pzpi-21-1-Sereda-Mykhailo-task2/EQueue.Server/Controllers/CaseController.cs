using EQueue.Db.Models;
using EQueue.Server.Controllers.Base;
using EQueue.Shared.Dto;
using EQueue.Shared.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using EQueue.Db;

namespace EQueue.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CaseController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CaseController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCase(long id, CancellationToken cancellationToken)
        {
            try
            {
                var caseEntity = await _context.Cases
                    .Include(c => c.Clinic)
                    .Include(c => c.Traumas).ThenInclude(t => t.TraumaType)
                    .Include(c => c.Traumas).ThenInclude(t => t.TraumaPlace)
                    .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

                if (caseEntity == null)
                {
                    return NotFound(new { Message = "Case not found" });
                }

                var caseDto = _mapper.Map<CaseDto>(caseEntity);
                return Ok(caseDto);
            }
            catch
            {
                throw new UnknownErrorException();
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllCases(CancellationToken cancellationToken)
        {
            try
            {
                var cases = await _context.Cases
                    .Include(c => c.Clinic)
                    .Include(c => c.Traumas).ThenInclude(t => t.TraumaType)
                    .Include(c => c.Traumas).ThenInclude(t => t.TraumaPlace)
                    .ToListAsync(cancellationToken);

                var caseDtos = _mapper.Map<List<CaseDto>>(cases);
                return Ok(caseDtos);
            }
            catch
            {
                throw new UnknownErrorException();
            }
        }

        [HttpPost("edit")]
        public async Task<IActionResult> EditCase(CaseDto caseDto, CancellationToken cancellationToken)
        {
            try
            {
                var caseEntity = _mapper.Map<Case>(caseDto);
                caseEntity.Clinic = null;
                _context.Cases.Update(caseEntity);
                await _context.SaveChangesAsync(cancellationToken);

                return Ok(_mapper.Map<CaseDto>(caseEntity));
            }
            catch
            {
                throw new UnknownErrorException();
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteCase(long id, CancellationToken cancellationToken)
        {
            try
            {
                var caseEntity = await _context.Cases
                    .Include(c => c.Traumas)
                    .Include(c => c.CasePriorities)
                    .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

                if (caseEntity == null)
                {
                    return NotFound(new { Message = "Case not found" });
                }

                if (caseEntity.Traumas.Any())
                {
                    _context.Traumas.RemoveRange(caseEntity.Traumas);
                }

                _context.Cases.Remove(caseEntity);
                await _context.SaveChangesAsync(cancellationToken);

                return Ok(true);
            }
            catch(Exception ex)
            {
                throw new UnknownErrorException();
            }
        }

    }
}
