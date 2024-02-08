namespace net7.Dtos.Fight
{
    public class AttackResultDto
    {
        public string Attacker { get; set; } = string.Empty;
        public string Opponent { get; set; } = string.Empty;
        public int AttackerHr { get; set; }
        public int OpponentHr { get; set; }
        public int Damage { get; set; }
    }
}