using Microsoft.EntityFrameworkCore;
using SportsLeague.DataAccess.Context;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;
using System.Text.RegularExpressions;

namespace SportsLeague.DataAccess.Repositories
{
    public class MatchLineupRepository : GenericRepository<MatchLineup>, IMatchLineupRepository
    {
        public MatchLineupRepository(LeagueDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<MatchLineup>> GetByMatchIdAsync(int matchId)
        {
            return await _dbSet
                .Include(ts => ts.Player)
                .Include(ts => ts.Match)
                .Where(ts => ts.MatchId == matchId)
                .ToListAsync();
        }

        public async Task<IEnumerable<MatchLineup>> GetByPlayerIdAsync(int playerId)
        {
            return await _dbSet
                .Include(ts => ts.Match)
                .Include(ts => ts.Player)
                .Where(ts => ts.PlayerId == playerId)
                .ToListAsync();
        }

        public async Task<MatchLineup?> GetByMatchAndPlayerAsync(int matchId, int playerId)
        {
            return await _dbSet
                .FirstOrDefaultAsync(ts => ts.MatchId == matchId
                                        && ts.PlayerId == playerId);
        }
    }
}
