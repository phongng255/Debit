using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Debit.DTOs
{
    public class CustomerDeBitDTO
    {
        [SwaggerSchema(ReadOnly = true)]
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
