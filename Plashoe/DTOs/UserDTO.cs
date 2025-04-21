﻿using System.ComponentModel.DataAnnotations;

namespace Plashoe.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
        public bool IsBlocked { get; set; }
        public string? Role { get; set; }
    }
}
