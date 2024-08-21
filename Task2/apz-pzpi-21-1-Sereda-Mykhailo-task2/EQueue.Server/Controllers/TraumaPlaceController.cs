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
    public class TraumaPlaceController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public TraumaPlaceController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            try
            {
                var results = await _context.TraumaPlaces.ToListAsync(cancellationToken);
                return Ok(_mapper.Map<List<TraumaPlaceDto>>(results));
            }
            catch
            {
                throw new UnknownErrorException();
            }
        }

        [HttpPost("edit")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Edit(TraumaPlaceDto traumaPlace, CancellationToken cancellationToken)
        {
            try
            {
                var mapped = _mapper.Map<TraumaPlace>(traumaPlace);
                var results = _context.TraumaPlaces.Update(mapped);
                await _context.SaveChangesAsync(cancellationToken);
                return Ok(_mapper.Map<TraumaPlaceDto>(mapped));
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
                var traumaPlace = await _context.TraumaPlaces.FindAsync(new object[] { id }, cancellationToken);
                if (traumaPlace == null)
                {
                    return NotFound(new { Message = "TraumaPlace not found" });
                }

                _context.TraumaPlaces.Remove(traumaPlace);
                await _context.SaveChangesAsync(cancellationToken);

                return Ok(true);
            }
            catch
            {
                throw new UnknownErrorException();
            }
        }
    }
}
