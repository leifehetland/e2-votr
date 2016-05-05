using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Votr.Models
{
    public class Poll
    {
        public int PollId { get; set; }

        [MaxLength(500)]
        [Required]
        public string Title { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }
        
        // Need Options Relation
        public virtual ICollection<Option> Options { get; set; }
        // Tag Relation
        // User Relation
        public virtual ApplicationUser CreatedBy { get; set; }
        // Vote Relation 
    }
}