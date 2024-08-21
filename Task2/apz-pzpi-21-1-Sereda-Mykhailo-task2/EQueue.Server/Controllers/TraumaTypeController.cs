using EQueue.Db;
using EQueue.Db.Models;
using EQueue.Server.Controllers.Base;
using EQueue.Shared.Dto;
using EQueue.Shared.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using EQueue.Shared;
using Microsoft.AspNetCore.Authorization;

namespace EQueue.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TraumaTypeController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public TraumaTypeController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            try
            {
                var results = await _context.TraumaTypes.ToListAsync(cancellationToken);
                return Ok(_mapper.Map<List<TraumaTypeDto>>(results));
            }
            catch
            {
                throw new UnknownErrorException();
            }
        }

        [HttpPost("edit")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Edit(TraumaTypeDto traumaType, CancellationToken cancellationToken)
        {
            try
            {
                var mapped = _mapper.Map<TraumaType>(traumaType);
                var results = _context.TraumaTypes.Update(mapped);
                await _context.SaveChangesAsync(cancellationToken);
                return Ok(_mapper.Map<TraumaTypeDto>(mapped));
            }
            catch
            {
                throw new UnknownErrorException();
            }
        }

        [HttpDelete("delete/{id}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Delete(long id, CancellationToken cancellationToken)
        {
            try
            {
                var traumaType = await _context.TraumaTypes.FindAsync(new object[] { id }, cancellationToken);
                if (traumaType == null)
                {
                    return NotFound(new { Message = "TraumaType not found" });
                }

                _context.TraumaTypes.Remove(traumaType);
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
