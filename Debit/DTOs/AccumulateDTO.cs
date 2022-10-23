using Debit.DTOs;
using System.ComponentModel.DataAnnotations;

namespace Debit.Models
{
    public class AccumulateDetailDTO
    {
        public Guid Id { get; set; }
        public Guid DebitId { get; set; }
        public decimal? Money { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Items { get; set; }
        public string CustomerName { get; set; }
    }
    public class AccumulateDTO
    {
        public Guid Id { get; set; }
        public Guid DebitId { get; set; }
        public string Money { get; set; }
        public string Item { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
