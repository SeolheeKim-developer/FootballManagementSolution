using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FootballManagement.Data;
using FootballManagement.Models;

namespace FootballManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaguesController : ControllerBase
    {
        private readonly FootballManagementContext _context;

        public LeaguesController(FootballManagementContext context)
        {
            _context = context;
        }

        // GET: api/Leagues
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LeagueDTO>>> GetLeagues()
        {
            return await _context.Leagues
                .Select(l => new LeagueDTO
                {
                    Code = l.Code,
                    Name = l.Name
                })
                .ToListAsync();
        }

        // GET: api/Leagues/5
        [HttpGet("{code}")]
        public async Task<ActionResult<LeagueDTO>> GetLeague(string code)
        {
            var leagueDTO = await _context.Leagues
                .Select(l => new LeagueDTO
                {
                    Code = l.Code,
                    Name = l.Name
                })
                .FirstOrDefaultAsync(l => l.Code ==code);

            if (leagueDTO == null)
            {
                return NotFound(new { message = "Error: League record not found." });
            }

            return leagueDTO;
        }
        // GET: api/Leagues/inc - Include the Teams Collection
        [HttpGet("inc")]
        public async Task<ActionResult<IEnumerable<LeagueDTO>>> GetLeaguesInc()
        {
            return await _context.Leagues
                .Include(d => d.Teams)
                .Select(d => new LeagueDTO
                {
                    Code = d.Code,
                    Name = d.Name,
                    Teams = d.Teams.Select(dTeam => new TeamDTO
                    {
                        Name = dTeam.Name
                        
                    }).ToList(),
                    NumberOfTeams = d.Teams.Count(),
                    CreatedBy = d.CreatedBy,
                    CreatedOn = d.CreatedOn,
                    UpdatedBy = d.UpdatedBy,
                    UpdatedOn = d.UpdatedOn
                })
                .ToListAsync();
        }

        // PUT: api/Leagues/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLeague(string id, LeagueDTO leagueDTO)
        {
            if (id != leagueDTO.Code)
            {
                return BadRequest(new { message = "Error: CODE does not match League"});
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Get the record you want to update
            var leagueToUpdate = await _context.Leagues.FindAsync(id);

            //Check that you got it
            if (leagueToUpdate == null)
            {
                return NotFound(new { message = "Error: League record not found." });
            }

            //Update the properties of the entity object from the DTO object
            leagueToUpdate.Code = leagueDTO.Code;
            leagueToUpdate.Name = leagueDTO.Name;
            //leagueToUpdate.CreatedBy = leagueDTO.CreatedBy;
            //leagueToUpdate.CreatedOn = leagueDTO.CreatedOn;
            //leagueToUpdate.UpdatedBy = leagueDTO.UpdatedBy;
            //leagueToUpdate.UpdatedOn = leagueDTO.UpdatedOn;


            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            
            catch (DbUpdateException dex)
            {
                if (dex.GetBaseException().Message.Contains("UNIQUE"))
                {
                    return BadRequest(new { message = "Unable to save: Duplicate CODE." });
                }
                else
                {
                    return BadRequest(new { message = "Unable to save changes to the database. Try again, and if the problem persists see your system administrator." });
                }
            }
        }

        // POST: api/Leagues
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LeagueDTO>> PostLeague(LeagueDTO leagueDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            League league = new League
            {
                Code = leagueDTO.Code,
                Name = leagueDTO.Name
            };
            try
            {
                _context.Leagues.Add(league);
                await _context.SaveChangesAsync();

                //Assign Database Generated values back into the DTO
                leagueDTO.Code = league.Code;

                return CreatedAtAction(nameof(GetLeague), new { id = league.Code }, leagueDTO);
            }
            catch (DbUpdateException dex)
            {
                if (dex.GetBaseException().Message.Contains("UNIQUE"))
                {
                    return BadRequest(new { message = "Unable to save: Duplicate Code" });
                }
                else
                {
                    return BadRequest(new { message = "Unable to save changes to the database. Try again, and if the problem persists see your system administrator." });
                }
            }
        }

        // DELETE: api/Leagues/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLeague(string id)
        {
            var league = await _context.Leagues.FindAsync(id);
            if (league == null)
            {
                return NotFound(new { message = "Delete Error: League has already been removed." });
            }
            try
            {
                _context.Leagues.Remove(league);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateException dex)
            {
                if (dex.GetBaseException().Message.Contains("FOREIGN KEY constraint failed"))
                {
                    return BadRequest(new { message = "Delete Error: Remember, you cannot delete a League that has teams assigned." });
                }
                else
                {
                    return BadRequest(new { message = "Delete Error: Unable to delete Doctor. Try again, and if the problem persists see your system administrator." });
                }
            }


        }

        private bool LeagueExists(string id)
        {
            return _context.Leagues.Any(e => e.Code == id);
        }
    }
}
