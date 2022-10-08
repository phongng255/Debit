using System.ComponentModel.DataAnnotations;

namespace Debit.Models
{
    public class DebitCustomer
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid CustomerId { get; set; }

        [Required]
        public string Items { get; set; }

        [Required]
        public decimal Money { get; set; }
        public decimal ProcessMoney { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime DateComplete { get; set; }

        [Required]
        public bool Status { get; set; } = false;

        public virtual ICollection<Accumulate>? Accumulates { get; set; }
        public virtual Customer? Customer { get; set; }
    }
}
