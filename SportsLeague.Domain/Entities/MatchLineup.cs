using SportsLeague.Domain.Enums;

namespace SportsLeague.Domain.Entities
{
    public class MatchLineup : AuditBase
    {
        public int MatchId { get; set; }
        public int PlayerId { get; set; }
        public bool IsStarter { get; set; } = true;// false= suplente,  true= titular
        public string Position { get; set; } = string.Empty;
        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;


        // Navigation Properties (Estas tablas Match,Player se conectan con la tabla intermedia MatchLineup)

        public Match Match { get; set; } = null!;
        public Player Player { get; set; } = null!;

    }
}
