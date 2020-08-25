using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QuotesAPI.Models
{
    public class Quote
    {
        public int ID { get; set; }
        [Required]
        [StringLength(25)]
        public string Title { get; set; }
        [Required]
        [StringLength(20)]
        public string Author { get; set; }
        [Required]
        [StringLength(500)]
        public string Description { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
    }
}