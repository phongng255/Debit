using AutoMapper;
using Debit.Models;
using System.ComponentModel.DataAnnotations;

namespace Debit.DTOs
{
    public class UserDTO
    {
        public Guid Id { get; set; }
        public string? PhoneNumber { get; set; }

    }
}
