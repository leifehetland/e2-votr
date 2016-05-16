using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Votr.Models
{
    public class Vote
    {
        public int VoteId { get; set; }
        public Poll Poll { get; set; }
        public ApplicationUser Voter { get; set; }
        public Option Choice { get; set; }
    }
}