using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeTrackerLukaN.Domain;

namespace TimeTrackerLukaN.Models
{
    public class ProjectInputModel
    {
        public string Name { get; set; }
        public long? ClientId { get; set; }

        public void MapTo(Project model)
        {
            model.Name = Name;
        }

    }
}
