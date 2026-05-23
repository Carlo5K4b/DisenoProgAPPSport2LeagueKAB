namespace SportsLeague.API.DTOs.Request
{
    public class MatchLineupRequestDTO
    {
        public int PlayerId { get; set; }
        public bool IsStarter { get; set; } // false= suplente,  true= titular
        public string Position { get; set; } = string.Empty;

    }
}
