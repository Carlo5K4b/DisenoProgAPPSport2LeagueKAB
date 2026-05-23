using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SportsLeague.API.DTOs.Request;
using SportsLeague.API.DTOs.Response;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Services;

namespace SportsLeague.API.Controllers;
[ApiController]
[Route("api/match/{matchId}")]
public class MatchLineupController : ControllerBase
{
    private readonly IMatchLineupService _matchLineupService;
    private readonly IMapper _mapper;

    public MatchLineupController(IMatchLineupService matchLineupService, IMapper mapper)
    {
        _matchLineupService = matchLineupService;
        _mapper = mapper;
    }

    [HttpPost("matchLineup")]
    
    public async Task<ActionResult<MatchLineupResponseDTO>> AddPlayer(int matchId, MatchLineupRequestDTO dto)
    {
        try
        {
            var lineup = _mapper.Map<MatchLineup>(dto);
            lineup.MatchId = matchId;

            var created = await _matchLineupService.AddPlayerToLineupAsync(lineup);

            var fullLineup = await _matchLineupService.GetByLineupMatchAsync(matchId);
            var createdPlayer = fullLineup.First(x => x.Id == created.Id);

            return CreatedAtAction(nameof(GetMatchLineup), new { matchId }, _mapper.Map<MatchLineupResponseDTO>(createdPlayer));
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpGet("matchLineup")]
    //[HttpGet]
    public async Task<ActionResult<IEnumerable<MatchLineupResponseDTO>>> GetMatchLineup(int matchId)
    {
        var lineup = await _matchLineupService.GetByLineupMatchAsync(matchId);
        return Ok(_mapper.Map<IEnumerable<MatchLineupResponseDTO>>(lineup));
    }

 
  
    [HttpGet("team/{teamId}")]
    public async Task<ActionResult<IEnumerable<MatchLineupResponseDTO>>> GetMatchLineupByTeam(int matchId, int teamId)
    {
        try
        {
            var matchLineup = await _matchLineupService.GetByMatchAndTeamAsync(matchId, teamId);
            return Ok(_mapper.Map<IEnumerable<MatchLineupResponseDTO>>(matchLineup));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> RemovePlayer(int id)
    {
        try
        {
            await _matchLineupService.RemovePlayerFromLineupAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}