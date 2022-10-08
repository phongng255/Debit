using System.ComponentModel.DataAnnotations;

namespace Debit.Models
{
    public class AccumulateDTO
    {
        public Guid Id { get; set; }
        [Required]
        public Guid DebitId { get; set; }
        [Required]
        public decimal? Money { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        
    }
}
