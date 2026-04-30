using System.Collections.Generic;

namespace Portfolio.Models
{
    public class SkillsViewModel
    {
        public List<Skill> TechnicalSkills { get; set; }
    }
    public class Skill
    {
        public string Name { get; set; }
        public int Percentage { get; set; }
    }
}
