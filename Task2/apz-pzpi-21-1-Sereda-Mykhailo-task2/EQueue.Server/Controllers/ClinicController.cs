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
    public class ClinicController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ClinicController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            try
            {
                var results = await _context.Clinics.ToListAsync(cancellationToken);
                return Ok(_mapper.Map<List<ClinicDto>>(results));
            }
            catch
            {
                throw new UnknownErrorException();
            }
        }

        [HttpPost("edit")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Edit(ClinicDto clinic, CancellationToken cancellationToken)
        {
            try
            {
                var mapped = _mapper.Map<Clinic>(clinic);
                var results = _context.Clinics.Update(mapped);
                await _context.SaveChangesAsync(cancellationToken);
                return Ok(_mapper.Map<ClinicDto>(mapped));
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
                var clinic = await _context.Clinics.FindAsync(new object[] { id }, cancellationToken);
                if (clinic == null)
                {
                    return NotFound(new { Message = "Clinic not found" });
                }

                _context.Clinics.Remove(clinic);
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
