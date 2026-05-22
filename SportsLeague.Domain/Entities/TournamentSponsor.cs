
using SportsLeague.Domain.Enums;

namespace SportsLeague.Domain.Entities
{
    public class TournamentSponsor : AuditBase
    {
        public int TournamentId { get; set; }
        public int SponsorId { get; set; }
        public decimal ContractAmount { get; set; }
        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;


        // Navigation Properties (Estas tablas Tournament,Sponsor se conectan con la tabla intermedia TournamentSponsor)
        public Tournament Tournament { get; set; } = null!;
        public Sponsor Sponsor { get; set; } = null!;

      
    }
}
