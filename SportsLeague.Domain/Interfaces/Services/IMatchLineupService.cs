using SportsLeague.Domain.Entities;

namespace SportsLeague.Domain.Interfaces.Services
{
    public interface IMatchLineupService
    {

        Task<IEnumerable<MatchLineup>> GetLineupByMatchAsync(int matchId);
        Task<MatchLineup> RegisterPlayerAsync(int matchId, int playerId);
        Task<IEnumerable<MatchLineup>> GetPlayerByMatchAsync(int matchId);
        Task RemoveFromPlayerAsync(int matchId, int playerId);
    }
}
