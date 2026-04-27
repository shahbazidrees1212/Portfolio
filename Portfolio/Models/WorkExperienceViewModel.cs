using System.Collections.Generic;

namespace Portfolio.Models
{
    public class WorkExperienceViewModel
    {
        public List<WorkExperience> WorkExperiences { get; set; }
    }

    public class WorkExperience
    {
        public string Position { get; set; }
        public string Company { get; set; }
        public string Duration { get; set; }
        public string Description { get; set; }
    }
}
