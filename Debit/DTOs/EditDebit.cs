using System.ComponentModel.DataAnnotations;

namespace Debit.DTOs
{
    public class EditDebit
    {
        public Guid CustomerId { get; set; }

        public string? Items { get; set; }

        public decimal Money { get; set; }
    }
}
