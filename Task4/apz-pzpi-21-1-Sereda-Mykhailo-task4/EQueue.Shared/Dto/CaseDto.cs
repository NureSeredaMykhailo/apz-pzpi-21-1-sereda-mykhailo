namespace EQueue.Shared.Dto
{
    public class CaseDto
    {
        public long Id { get; set; }
        public int Age { get; set; }
        public string? Name { get; set; }
        public long? ClinicId { get; set; }
        public string? ClinicTitle { get; set; }
        public long StartedTreatmentUnixTime { get; set; }
        public long TraumasRegisteredUnixTime { get; set; }
        public List<TraumaDto> TraumaDtos { get; set; } = new();
        public bool Survived { get; set; }
        public bool GotIncurableDamage { get; set; }
    }
}
