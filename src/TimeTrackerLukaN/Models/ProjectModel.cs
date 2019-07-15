using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeTrackerLukaN.Domain;

namespace TimeTrackerLukaN.Models
{
    public class ProjectModel
    {

        private ProjectModel()
        {

        }

        public long Id { get; set; }
        public  string Name { get; set; }
        public  long ClientId { get; set; }
        public string ClientName { get; set; }

        public static ProjectModel FromProject(Project project)
        {
            return new ProjectModel
            {
                Id = project.Id,
                ClientId =  project.Client.Id,
                ClientName = project.Client.Name,
                Name = project.Name
            };
        }

    }
}
