using SportsLeague.Domain.Entities;

namespace SportsLeague.Domain.Interfaces.Repositories
{
    public interface IMatchLineupRepository : IGenericRepository<MatchLineup>
    {
        Task<IEnumerable<MatchLineup>> GetByMatchIdAsync(int matchId);
        Task<IEnumerable<MatchLineup>> GetByPlayerIdAsync(int playerId);
        Task<MatchLineup?> GetByMatchAndPlayerAsync(int matchId, int playerId);

    }

}

