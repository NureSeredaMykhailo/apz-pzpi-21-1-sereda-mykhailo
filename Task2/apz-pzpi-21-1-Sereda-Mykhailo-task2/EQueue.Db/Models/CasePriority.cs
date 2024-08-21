namespace EQueue.Db.Models
{
    public class CasePriority : IEntity
    {
        public long Id { get; set; }
        public long CaseId { get; set; }
        public Case? Case { get; set; }
        public float DamagePriority { get; set; }
        public float DeathPriority { get; set; }
        public float CombinedPriority { get; set; }
        public long PriorityPeriodStartUnix { get; set; }
        public long PriorityPeriodEndUnix { get; set; }
    }
}
