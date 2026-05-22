using SportsLeague.Domain.Entities;

namespace SportsLeague.Domain.Interfaces.Services
{
    public interface IMatchLineupService
    {
     
        Task<IEnumerable<MatchLineup>> GetByLineupMatchAsync(int matchId);
        Task<IEnumerable<MatchLineup>> GetByLineupTeamAsync(int matchId, int teamId);
        Task<MatchLineup> RegisterPlayerToLineupAsync(MatchLineup matchLineup);
        Task RemovePlayerFromLineupAsync(int id);
        Task<IEnumerable<MatchLineup>> GetByMatchAndTeamAsync(int matchId, int teamId);

     
    }
}
