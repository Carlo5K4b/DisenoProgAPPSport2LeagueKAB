using Microsoft.Extensions.Logging;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Enums;
using SportsLeague.Domain.Helpers;
using SportsLeague.Domain.Interfaces.Repositories;
using SportsLeague.Domain.Interfaces.Services;

namespace SportsLeague.Domain.Services
{
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

        public async Task<MatchLineup> RegisterPlayerAsync(int matchId, int playerId)
        {
            // Validar que el Match existe
            var matchExists = await _matchLineupRepository.GetByIdAsync(matchId);
            if (matchExists == null)
                throw new KeyNotFoundException($"Solo se pueden registrar alineaciones en partidos Scheduled {matchId}");


            // Validar que el Jugador(Player) existe
            var playerExists = await _matchLineupRepository.ExistsAsync(playerId);
            if (!playerExists)
                throw new KeyNotFoundException(
                    $"No se encontró el jugador con ID {playerId}");

            // Validar que no esté duplicado
            var existing = await _matchLineupRepository
                .GetByMatchAndPlayerAsync(matchId, playerId);
            if (existing != null)
                throw new InvalidOperationException(
                    $"El jugador ya está vinculado a este partido");

            var matchLineup = new MatchLineup
            {
                MatchId = matchId,
                PlayerId = playerId,

            };

            _logger.LogInformation("Registering playerI {PlayerIdId} to match {MatchId}",
                matchId, playerId);
            return await _matchLineupRepository.CreateAsync(matchLineup);
        }

        // Implementación del método requerido por la interfaz IMatchLineupService
        public async Task<IEnumerable<MatchLineup>> GetLineupByMatchAsync(int matchId)
        {
            _logger.LogInformation("Obteniendo alineación para el partido {MatchId}", matchId);
            await _validationHelper.ValidateMatchForEventAsync(matchId);

            var matchLineups = await _matchLineupRepository.GetByMatchIdAsync(matchId);
            return matchLineups ?? Enumerable.Empty<MatchLineup>();
        }

        //Listar todos los partidos en los que ha participado un jugador específico
        public async Task<IEnumerable<MatchLineup>> GetPlayerByMatchAsync(int matchId)
        {
            _logger.LogInformation("Obteniendo jugadores para el partido {MatchId}", matchId);
            await _validationHelper.ValidateMatchForEventAsync(matchId);

            var matchLineups = await _matchLineupRepository.GetByMatchIdAsync(matchId);
            return matchLineups ?? Enumerable.Empty<MatchLineup>();
        }

        public async Task RemoveFromPlayerAsync(int matchId, int playerId)
        {
            var existing = await _matchLineupRepository
                .GetByMatchAndPlayerAsync(matchId, playerId);
            if (existing == null)
                throw new KeyNotFoundException(
                    $"No se encontró la vinculación entre el jugador {playerId} y el partido {matchId}");

            _logger.LogInformation("Removing player {SponsorId} from match {MatchId}",
                matchId, playerId);
            await _matchLineupRepository.DeleteAsync(existing.Id);
        }
    }
}
