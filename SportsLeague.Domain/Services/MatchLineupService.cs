using Microsoft.Extensions.Logging;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Enums;
using SportsLeague.Domain.Helpers;
using SportsLeague.Domain.Interfaces.Repositories;
using SportsLeague.Domain.Interfaces.Services;

namespace SportsLeague.Domain.Services;

public class MatchLineupService : IMatchLineupService
{
    private readonly IMatchLineupRepository _matchLineupRepository;
    private readonly MatchValidationHelper _validationHelper;
    private readonly ILogger<MatchLineupService> _logger;

    public MatchLineupService(
        IMatchLineupRepository matchLineupRepository,
        MatchValidationHelper validationHelper,
        ILogger<MatchLineupService> logger)
    {
        _matchLineupRepository = matchLineupRepository;
        _validationHelper = validationHelper;
        _logger = logger;
    }

    public async Task<MatchLineup> AddPlayerToLineupAsync(MatchLineup lineup)
    {

        var match = await _validationHelper.ValidateMatchForEventAsync(lineup.MatchId);

        if (match.Status != MatchStatus.Scheduled)
            throw new InvalidOperationException("Solo se pueden registrar alineaciones en partidos Scheduled");


        var player = await _validationHelper.ValidatePlayerInMatchAsync(lineup.PlayerId, match);

        if (await _matchLineupRepository.ExistsByMatchAndPlayerAsync(lineup.MatchId, lineup.PlayerId))
            throw new InvalidOperationException("El jugador ya está registrado en la alineación de este partido");

        if (lineup.IsStarter)
        {
            var currentLineup = await _matchLineupRepository.GetByMatchAndTeamAsync(lineup.MatchId, player.TeamId);
            if (currentLineup.Count(x => x.IsStarter) >= 11)
                throw new InvalidOperationException("El equipo ya tiene 11 titulares registrados en este partido");
        }

        _logger.LogInformation("Agregando jugador {PlayerId} al partido {MatchId} como {Position}",
            lineup.PlayerId, lineup.MatchId, lineup.Position);

        return await _matchLineupRepository.CreateAsync(lineup);
    }

    public async Task<IEnumerable<MatchLineup>> GetByLineupMatchAsync(int matchId)
    {
        _logger.LogInformation("Esta es la alineación completa para el partido {MatchId}", matchId);

        await _validationHelper.ValidateMatchForEventAsync(matchId);

        return await _matchLineupRepository.GetByMatchAsync(matchId);
    }

    public async Task<IEnumerable<MatchLineup>> GetByLineupTeamAsync(int matchId, int teamId)
    {
        _logger.LogInformation("Esta es la alineación del equipo {TeamId} para el partido {MatchId}", teamId, matchId);


        await _validationHelper.ValidateMatchForEventAsync(matchId);

        return await _matchLineupRepository.GetByMatchAndTeamAsync(matchId, teamId);
    }

    public async Task RemovePlayerFromLineupAsync(int id)
    {
        _logger.LogInformation("Eliminando registro de alineación con ID: {Id}", id);


        var exists = await _matchLineupRepository.ExistsAsync(id);
        if (!exists)
        {
            _logger.LogWarning("Registro de alineación {Id} no encontrado para eliminación", id);
            throw new KeyNotFoundException($"No se encontró el registro de alineación con ID {id}");
        }

        await _matchLineupRepository.DeleteAsync(id);
    }
    public async Task<IEnumerable<MatchLineup>> GetByMatchAndTeamAsync(int matchId, int teamId)
    {
        return await _matchLineupRepository.GetByMatchAndTeamAsync(matchId, teamId);
    }
}