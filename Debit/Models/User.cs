﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Debit.Models
{
    [Index(nameof(User.PhoneNumber), IsUnique = true)]
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(255)]
        
        public string? PhoneNumber { get; set; }
        [Required]
        [MaxLength(255)]
        public string? PassworhHash { get; set; }

    }
}
