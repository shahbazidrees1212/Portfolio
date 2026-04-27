using System.Collections.Generic;

namespace Portfolio.Models
{
    public class ProjectsViewModel
    {
        public List<Project> Projects { get; set; }
    }

    public class Project
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Technologies { get; set; }
        public string GithubUrl { get; set; }
        public string VideoUrl { get; set; }   // ✅ Added VideoUrl
    }
}
