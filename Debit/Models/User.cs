using System.ComponentModel.DataAnnotations;

namespace Debit.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(255)]
        public string? Name { get; set; }
        [Required]
        [MaxLength(255)]
        public string? PassworhHash { get; set; }

    }
}
