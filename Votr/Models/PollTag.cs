using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Votr.Models
{
    public class PollTag
    {
        public virtual int PollTagId {get;set;}
        [Required]
        public virtual Poll Poll { get; set; }
        [Required]
        public virtual Tag Tag { get; set; }
    }
}