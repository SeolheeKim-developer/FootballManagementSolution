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
    public class TeamsController : ControllerBase
    {
        private readonly FootballManagementContext _context;

        public TeamsController(FootballManagementContext context)
        {
            _context = context;
        }

        // //GET: api/Teams//Get all teams includingthe Name of the league the team is on
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeamDTO>>> GetTeams()
        {
            return await _context.Teams
                .Select(l => new TeamDTO
                {
                    ID = l.ID,
                    Name = l.Name,
                    Budget = l.Budget,
                    LeagueCode = l.LeagueCode,
                    League = new LeagueDTO
                    {
                        Code = l.League.Code,
                        Name = l.League.Name
                    }

                    //CreatedBy = l.CreatedBy,
                    //CreatedOn = l.CreatedOn,
                    //UpdatedBy = l.UpdatedBy,
                    //UpdatedOn = l.UpdatedOn
                })
                .ToListAsync();
        }

        // GET: api/Teams including the number of plyers and list of player
        [HttpGet("inc")]
        public async Task<ActionResult<IEnumerable<TeamDTO>>> GetTeamsInc()
        {
            return await _context.Teams
                .Include(t => t.PlayerTeams).ThenInclude(pt => pt.Player)
                .Select(l => new TeamDTO
                {
                    ID = l.ID,
                    Name = l.Name,
                    Budget = l.Budget,
                    LeagueCode = l.LeagueCode,
                    League = new LeagueDTO
                    {
                        Code = l.League.Code,
                        Name = l.League.Name
                    },
                    Players = l.PlayerTeams.Select(p => new PlayerDTO
                    {
                        ID = p.PlayerID,
                        FirstName = p.Player.FirstName,
                        LastName = p.Player.LastName,
                        Jersey = p.Player.Jersey,
                        EMail = p.Player.EMail,
                        DOB = p.Player.DOB,
                        FeePaid = p.Player.FeePaid
                    }).ToList(),
                    NumberOfPlayers = l.PlayerTeams.Count,
                    CreatedBy = l.CreatedBy,
                    CreatedOn = l.CreatedOn,
                    UpdatedBy = l.UpdatedBy,
                    UpdatedOn = l.UpdatedOn
                })
                .ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<TeamDTO>> GetTeam(int id)
        {
            var team = await _context.Teams
                .Include(t => t.League)
                .Include(t => t.PlayerTeams)
                .Select(l => new TeamDTO
                {
                    ID = l.ID,
                    Name = l.Name,
                    Budget = l.Budget,
                    LeagueCode = l.LeagueCode,
                    //League = new LeagueDTO
                    //{
                    //    Code = l.League.Code,
                    //    Name = l.League.Name
                    //},
                    CreatedBy = l.CreatedBy,
                    CreatedOn = l.CreatedOn,
                    UpdatedBy = l.UpdatedBy,
                    UpdatedOn = l.UpdatedOn
                })
                .FirstOrDefaultAsync(l => l.ID == id);

            if (team == null)
            {
                return NotFound(new { message = "Error: Team record not found." });
            }

            return team;
        }
        // GET: api/Teams/5
        [HttpGet("ByLeague/{leagueCode}")]
        public async Task<ActionResult<IEnumerable<TeamDTO>>> GetTeamByLeague(string leagueCode)
        {
            var teamDTOs = await _context.Teams
                .Include(t => t.League)
                .Include(t => t.PlayerTeams).ThenInclude(pt => pt.Player)
                .Where(t => t.LeagueCode == leagueCode)
                .Select(l => new TeamDTO
                {
                    ID = l.ID,
                    Name = l.Name,
                    Budget = l.Budget,
                    LeagueCode = l.LeagueCode,
                    League = new LeagueDTO
                    {
                        Code = l.League.Code,
                        Name = l.League.Name
                    },
                    Players = l.PlayerTeams.Select(p => new PlayerDTO
                    {
                        ID = p.PlayerID,
                        FirstName = p.Player.FirstName,
                        LastName = p.Player.LastName,
                        Jersey = p.Player.Jersey,
                        EMail = p.Player.EMail,
                        DOB = p.Player.DOB,
                        FeePaid = p.Player.FeePaid
                    }).ToList(),
                    NumberOfPlayers = l.PlayerTeams.Count,
                    CreatedBy = l.CreatedBy,
                    CreatedOn = l.CreatedOn,
                    UpdatedBy = l.UpdatedBy,
                    UpdatedOn = l.UpdatedOn
                })
                .ToListAsync();

            if (teamDTOs.Count > 0)
            {
                return teamDTOs;
            }
            else
            {
                return NotFound(new { message = "Error: No Team records for the league code." });
            }
            
        }

        // PUT: api/Teams/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTeam(int id, TeamDTO teamDTO)
        {
            if (id != teamDTO.ID)
            {
                return BadRequest(new { message = "Error: ID does not match Team" });
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //Get the record you want to update
            var teamToUpdate = await _context.Teams.FindAsync(id);

            //Check that you got it
            if (teamToUpdate == null)
            {
                return NotFound(new { message = "Error: Team record not found." });
            }

            //Update the properties of the entity object from the DTO object

            teamToUpdate.ID = teamDTO.ID;
            teamToUpdate.Name = teamDTO.Name;
            teamToUpdate.Budget = teamDTO.Budget;
            teamToUpdate.LeagueCode = teamDTO.LeagueCode;
            //teamToUpdate.League = new League
            //{
            //    Code = teamDTO.LeagueCode,
            //    Name = teamDTO.League.Name
            //};
            teamToUpdate.CreatedBy = teamDTO.CreatedBy;
            teamToUpdate.CreatedOn = teamDTO.CreatedOn;
            teamToUpdate.UpdatedBy = teamDTO.UpdatedBy;
            teamToUpdate.UpdatedOn = teamDTO.UpdatedOn;


            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateException ex)
            {

                // Log the error message or stack trace for debugging purposes
                Console.WriteLine(ex);

                // Return a specific error code along with a descriptive message
                return StatusCode(500, new { message = "Unable to save changes to the database. Try again, and if the problem persists see your system administrator." });
                //return BadRequest(new { message = "Unable to save changes to the database. Try again, and if the problem persists see your system administrator." });
                
            }
        }

        // POST: api/Teams
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TeamDTO>> PostTeam(TeamDTO teamDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Team team = new Team
            {
                ID = teamDTO.ID,
                Name = teamDTO.Name,
                Budget = teamDTO.Budget,
                LeagueCode = teamDTO.LeagueCode
                //League = new League
                //{
                //    Code = teamDTO.LeagueCode,
                //    Name = teamDTO.League.Name
                //}

            };

            try
            {
                _context.Teams.Add(team);
                await _context.SaveChangesAsync();

                //Assign Database Generated values back into the DTO
                teamDTO.ID = team.ID;

                return CreatedAtAction(nameof(GetTeam), new { id = team.ID }, teamDTO);
            }
            catch (DbUpdateException)
            {
                return BadRequest(new { message = "Unable to save changes to the database. Try again, and if the problem persists see your system administrator." });
            }
            
        }

        // DELETE: api/Teams/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeam(int id)
        {
            var team = await _context.Teams.FindAsync(id);
            if (team == null)
            {
                return NotFound(new { message = "Delete Error: Team has already been removed." });
            }
            try
            {
                _context.Teams.Remove(team);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateException)
            {
                return BadRequest(new { message = "Delete Error: Unable to delete Team." });
            }
        }

        private bool TeamExists(int id)
        {
            return _context.Teams.Any(e => e.ID == id);
        }
    }
}
