using System.ComponentModel.DataAnnotations;

namespace Debit.Models
{
    public class DebitCustomer
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Người dùng không để trống")]
        public Guid CustomerId { get; set; }

        [Required(ErrorMessage = "Tên đồ để góp để trống")]
        public string Items { get; set; }

        [Required(ErrorMessage = "Số tiền không được để trống")]
        public decimal Money { get; set; }


        [Required(ErrorMessage = "Số tiền không được để trống")]
        public decimal ProcessMoney { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime DateComplete { get; set; }

        [Required]
        public bool Status { get; set; } = false;

        public virtual ICollection<Accumulate>? Accumulates { get; set; }
        public virtual Customer? Customer { get; set; }
    }
}
