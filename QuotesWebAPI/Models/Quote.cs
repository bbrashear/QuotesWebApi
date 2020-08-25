using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QuotesWebAPI.Models
{
    public class Quote
    {
        public int ID { get; set; }
        [Required] //model validations used to ensure proper info is present
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

        public string UserID { get; set; }
    }
}