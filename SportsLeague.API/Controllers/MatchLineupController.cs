using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SportsLeague.API.DTOs.Request;
using SportsLeague.API.DTOs.Response;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Services;

namespace SportsLeague.API.Controllers
{
    [ApiController]
[Route("api/match/{matchId}/lineup")]

    public class MatchLineupController : ControllerBase
    {
        private readonly IMatchLineupService _matchLineupService;
        private readonly IMapper _mapper;
        public MatchLineupController(
            IMatchLineupService matchLineupService,
            IMapper mapper)

        {
            _matchLineupService = matchLineupService;
            _mapper = mapper;

        }

        // POST api/MatchLineup/{id}
        [HttpPost("{id}/player")]
        public async Task<ActionResult<MatchLineupResponseDTO>> RegisterMatchLineup(
     int id, MatchLineupRequestDTO dto)
        {
            try
            {
                // Use the correct method signature: RegisterPlayerAsync(int matchId, int playerId)
                var created = await _matchLineupService.RegisterPlayerAsync(id, dto.PlayerId);
                var responseDTO = _mapper.Map<MatchLineupResponseDTO>(created);
                return CreatedAtAction(nameof(GetPlayer), new { id }, responseDTO);

            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MatchLineupResponseDTO>>> GetMatchLineup(int matchId)
        {
            var lineup = await _matchLineupService.GetLineupByMatchAsync(matchId);
            return Ok(_mapper.Map<IEnumerable<MatchLineupResponseDTO>>(lineup));
        }

        [HttpGet("team/{teamId}")]
        public async Task<ActionResult<IEnumerable<MatchLineupResponseDTO>>> GetMatchLineupByTeam(int matchId, int teamId)
        {
            try
            {
                var lineup = await _matchLineupService.GetLineupByMatchAsync(matchId);
                return Ok(_mapper.Map<IEnumerable<MatchLineupResponseDTO>>(lineup));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // GET api/MatchLineup/{id}
        [HttpGet("{id}/player")]
        public async Task<ActionResult<IEnumerable<MatchLineupResponseDTO>>> GetPlayer(int id)
        {
            var players = await _matchLineupService.GetPlayerByMatchAsync(id);
            return Ok(_mapper.Map<IEnumerable<MatchLineupResponseDTO>>(players));
        }

        // DELETE api/MatchLineup/{tid}
        [HttpDelete("{id}/player/{tid}")]
        public async Task<ActionResult> RemoveFromPlayer(int id, int tid)
        {
            try
            {
                await _matchLineupService.RemoveFromPlayerAsync(id, tid);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
