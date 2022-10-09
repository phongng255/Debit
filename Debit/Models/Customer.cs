using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Debit.Models
{
    [Index(nameof(Customer.PhoneNumber), IsUnique = true)]
    public class Customer
    {
        [Key]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Tên không được để trống")]
        [MaxLength(255)]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Số điện thoại không để trống")]
        [MaxLength(11)]
        public string? PhoneNumber { get; set; }

        public virtual ICollection<DebitCustomer>? Debits { get; set; }

    }
}
