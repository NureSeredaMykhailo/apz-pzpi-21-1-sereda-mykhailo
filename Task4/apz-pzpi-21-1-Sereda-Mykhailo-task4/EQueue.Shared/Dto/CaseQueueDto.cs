namespace EQueue.Shared.Dto
{
    public class CaseQueueDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long TraumasRegisteredUnixTime { get; set; }
        public long CaseId { get; set; }
        public long CasePriorityId { get; set; }
        public float DamagePriority { get; set; }
        public float DeathPriority { get; set; }
        public float CombinedPriority { get; set; }
        public List<TraumaDto> TraumaDtos { get; set; } = new();
        public long PriorityPeriodStartUnix { get; set; }
        public long PriorityPeriodEndUnix { get; set; }
    }
}
