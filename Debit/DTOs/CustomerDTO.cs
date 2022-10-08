using System.ComponentModel.DataAnnotations;
using Debit.Models;
namespace Debit.DTOs
{
    public class CustomerDTO
    {
        public  Guid Id { get; set; }

        [Required(ErrorMessage = "Tên không được để trống")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Số điện thoại không để trống")]
        [MaxLength(11)]
        public string? PhoneNumber { get; set; }

        public List<DebitDTO>? Debits { get; set; }

    }
}
