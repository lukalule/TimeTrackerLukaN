﻿using System.ComponentModel.DataAnnotations;

namespace TimeTrackerLukaN.Domain
{
    public class User
    {
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        public decimal HourRate { get; set; }
    }
}
