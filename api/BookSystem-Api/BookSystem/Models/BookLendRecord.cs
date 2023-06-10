using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookSystem.Models
{
    public class BookLendRecord
    {
        public string BookName { get; set; }
        public int BookId { get; set; }
        public string BookKeeperId { get; set; }
        public string BookKeeperCname { get; set; }
        public string BookKeeperEname { get; set; }
        public string LendDate { get; set; }
    }
}