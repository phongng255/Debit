using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Debit.DTOs
{
    public class DebitDTO
    {
        [SwaggerSchema(ReadOnly = true)]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Id Customer Không để trống !")]
        public Guid CustomerId { get; set; }

        [Required(ErrorMessage = "Tên Sản phẩm không để trống")]
        public string Items { get; set; }   

        [Required(ErrorMessage = "Giá Khống để trống")]
        public decimal Money { get; set; }

        [SwaggerSchema(ReadOnly = true)]
        public decimal ProcessMoney { get; set; }

        [SwaggerSchema(ReadOnly = true)]
        public DateTime? CreatedAt { get; set; } = DateTime.Now;

        [SwaggerSchema(ReadOnly = true)]
        public DateTime? DateComplete { get; set; }

        [SwaggerSchema(ReadOnly = true)]
        public bool? Status { get; set; } = false;
    }
}
