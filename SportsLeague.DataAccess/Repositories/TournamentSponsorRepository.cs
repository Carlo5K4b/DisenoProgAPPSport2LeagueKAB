using SportsLeague.DataAccess.Context;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace SportsLeague.DataAccess.Repositories
{
    public class TournamentSponsorRepository : GenericRepository<TournamentSponsor>, ITournamentSponsorRepository
    {
        public TournamentSponsorRepository(LeagueDbContext context) : base(context)
        {
        }


        public async Task<IEnumerable<TournamentSponsor>> GetByTournamentIdAsync(int tournamentId)
        {
            return await _dbSet
                .Include(ts => ts.Sponsor)
                .Include(ts => ts.Tournament)
                .Where(ts => ts.TournamentId == tournamentId)
                .ToListAsync();
        }

        public async Task<IEnumerable<TournamentSponsor>> GetBySponsorIdAsync(int sponsorId)
        {
            return await _dbSet
                .Include(ts => ts.Sponsor)
                .Include(ts => ts.Tournament)
                .Where(ts => ts.SponsorId == sponsorId)
                .ToListAsync();
        }

        public async Task<TournamentSponsor?> GetByTournamentAndSponsorAsync(int tournamentId, int sponsorId)
        {
            return await _dbSet
                .FirstOrDefaultAsync(ts => ts.TournamentId == tournamentId
                                        && ts.SponsorId == sponsorId);
        }
    }
}
