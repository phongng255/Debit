using System.ComponentModel.DataAnnotations;

namespace Debit.Models
{
    public class Accumulate
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Hóa đơn góp không được để trống")]
        public Guid DebitId { get; set; }
        [Required(ErrorMessage = "Số tiền không được để trống")]
        public decimal Money { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }=DateTime.Now;
        public virtual DebitCustomer? Debit { get; set; }
    }
}
