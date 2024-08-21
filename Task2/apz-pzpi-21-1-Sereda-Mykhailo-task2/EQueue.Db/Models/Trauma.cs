namespace EQueue.Db.Models
{
    public class Trauma : IEntity
    {
        public long Id { get; set; }
        public long? TraumaTypeId { get; set; }
        public TraumaType TraumaType { get; set; }
        public long? TraumaPlaceId { get; set; }
        public TraumaPlace TraumaPlace { get; set; }
        public long? CaseId { get; set; }
        public Case Case { get; set; }
    }
}
