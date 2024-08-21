namespace EQueue.Db.Models
{
    public class Case : IEntity
    {
        public long Id { get; set; }
        public int Age { get; set; }
        public string? Name { get; set; }
        public long? ClinicId { get; set; }
        public Clinic? Clinic { get; set; }
        public long StartedTreatmentUnixTime { get; set; }
        public long TraumasRegisteredUnixTime { get; set; }
        public List<Trauma> Traumas { get; set; }
        public bool Survived { get; set; }
        public bool GotIncurableDamage { get; set; }
        public List<CasePriority> CasePriorities { get; set; }
    }
}
