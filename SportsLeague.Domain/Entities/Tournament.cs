using System;
using System.Collections.Generic;
using System.Text;

using SportsLeague.Domain.Enums;

namespace SportsLeague.Domain.Entities
{
    public class Tournament : AuditBase
    {
        public string Name { get; set; } = string.Empty;
        public string Season { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public TournamentStatus Status { get; set; } = TournamentStatus.Pending;

        // Navigation Properties(Torneo de muchos equipos:TournamentTeam)
        //Relación de muchos a muchos entre Tournament y Team a través de la tabla intermedia TournamentTeam
        public ICollection<TournamentTeam> TournamentTeams { get; set; } = new List<TournamentTeam>();

        // Agregar dentro de la clase Tournament, después de TournamentTeams:
        public ICollection<Match> Matches { get; set; } = new List<Match>();
    }
}

