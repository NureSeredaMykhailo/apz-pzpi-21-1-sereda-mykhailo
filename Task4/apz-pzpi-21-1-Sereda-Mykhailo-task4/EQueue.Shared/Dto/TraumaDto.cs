namespace EQueue.Shared.Dto
{
    public class TraumaDto
    {
        public long Id { get; set; }    
        public long? TraumaTypeId { get; set; }
        public string? TraumaTypeTitle { get; set; }
        public long? TraumaPlaceId { get; set; }
        public string? TraumaPlaceTitle { get; set; }
        public long? CaseId { get; set; }
    }
}
