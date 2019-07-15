using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTrackerLukaN.Models
{
    public class TimeEntryInputModel
    {
        public long ProjectId { get; set; }
        public string ProjectName { get; set; }
        public long UserId { get; set; }
        public DateTime EntryDate { get; set; }
        public int Hours { get; set; }
        public string Description { get; set; }
    }
}
