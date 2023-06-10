using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookSystem.Models
{
    public class BookQueryArg
    {
        public int BookId { get; set; }
        public string BookName { get; set; }
        public string BookClassId { get; set; }
        public string BookKeeperId { get; set; }
        public string BookStatusId { get; set; }

    }
}