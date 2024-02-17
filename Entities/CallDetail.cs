using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CsvHelper.Configuration.Attributes;

namespace CDR.Entities
{
    [Table("CallDetails")]
    public class CallDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Ignore]
        public int ID { get; set; }

        [Name("caller_id")]
        public long? CallerId { get; set; }

        [Name("recipient")]
        public long Recipient { get; set; }

        [Name("call_date")]
        [Format("dd/MM/yyyy")]
        public DateOnly CallDate {  get; set; }

        [Name("end_time")]
        [Format("HH:mm:ss")]
        public TimeOnly EndTime { get; set; }

        [Name("duration")]
        public int Duration { get; set; }

        [Name("cost")]
        public decimal Cost { get; set; }

        [Name("reference")]
        public string Reference { get; set; }

        [Name("currency")]
        public string Currency {  get; set; }
    }
}
