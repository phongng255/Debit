using System.ComponentModel.DataAnnotations;

namespace Debit.Models
{
    public class Debit
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid CustomerId { get; set; }

        [Required]
        public decimal Money { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required]
        public bool Status { get; set; } = false;

        public ICollection<Accumulate> Accumulates { get; set; }
        public Customer Customer { get; set; }
    }
}
