using System.ComponentModel.DataAnnotations;

namespace Debit.Models
{
    public class Customer
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(255)]
        public string? Name { get; set; }

        [Required]
        [MaxLength(11)]
        public string? PhoneNumber { get; set; }

        public virtual ICollection<DebitCustomer>? Debits { get; set; }

    }
}
