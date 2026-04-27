using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Portfolio.Models;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace Portfolio.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly EmailSettings _emailSettings;

        public HomeController(ApplicationDbContext context,
                              IOptions<EmailSettings> emailSettings)
        {
            _context = context;
            _emailSettings = emailSettings.Value;
        }

        // ================= HOME =================

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewBag.Message = "Learn more about me.";
            return View();
        }

        public IActionResult Skills()
        {
            var skills = new SkillsViewModel
            {
                TechnicalSkills = new List<Skill>
                {
                    new Skill { Name = "ASP.NET MVC", Percentage = 90 },
                    new Skill { Name = "C#", Percentage = 85 },
                    new Skill { Name = "JavaScript", Percentage = 80 },
                    new Skill { Name = "SQL Server", Percentage = 85 },
                    new Skill { Name = "HTML/CSS", Percentage = 90 },
                    new Skill { Name = "Bootstrap", Percentage = 85 }
                }
            };

            return View(skills);
        }
        // GET: /Home/Projects
        public IActionResult Projects()
        {
            var projects = new ProjectsViewModel
            {
                Projects = new List<Project>
                {
                   new Project
    {
        Title = "E-Commerce Platform",
        Description = "Full-stack e-commerce application with payment integration",
        Technologies = "ASP.NET MVC, SQL Server, Bootstrap",
        GithubUrl = "https://github.com/shahbazidrees1212/PortfolioWeb",
        VideoUrl = "/videos/demo1.mp4"
    },
    new Project
    {
        Title = "Task Management System",
        Description = "Web-based task tracking and project management tool",
        Technologies = "ASP.NET Core, Entity Framework, jQuery",
        GithubUrl = "https://github.com/shahbazidrees1212/ShortVideoApp",
        VideoUrl = "/videos/demo2.mp4"
    },
    new Project
    {
        Title = "Weather Dashboard",
        Description = "Real-time weather application with API integration",
        Technologies = "ASP.NET Web API, JavaScript, Chart.js",
        GithubUrl = "https://github.com/shahbazidrees1212/Student-Management-System",
        VideoUrl =  "/videos/demo4.mp4"
    },
    new Project
    {
        Title = "Mobile Backend API",
        Description = "Backend service for mobile apps with push notifications",
        Technologies = "ASP.NET Core, Azure, Firebase",
        GithubUrl = "https://github.com/shahbazidrees1212/EmployeeDataEmailNotifier",
        VideoUrl = null
    }
                }
            };

            return View(projects);
        }

        // GET: /Home/Experience
        public IActionResult Experience()
        {
            var experience = new WorkExperienceViewModel
            {
                WorkExperiences = new List<WorkExperience>
                {
                    new WorkExperience
                    {
                        Position = "Senior Software Developer",
                        Company = "Tech Solutions Inc.",
                        Duration = "Jan 2022 - Present",
                        Description = "Leading development of enterprise web applications using ASP.NET MVC and Azure services."
                    },
                    new WorkExperience
                    {
                        Position = "Software Developer",
                        Company = "Digital Innovations Ltd.",
                        Duration = "Jun 2020 - Dec 2021",
                        Description = "Developed and maintained multiple client-facing web applications using modern .NET technologies."
                    },
                    new WorkExperience
                    {
                        Position = "Junior Developer",
                        Company = "StartUp Tech",
                        Duration = "Jan 2019 - May 2020",
                        Description = "Assisted in building responsive web applications and RESTful APIs."
                    }
                }
            };

            return View(experience);
        }

        // ================= CONTACT =================

        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contact(ContactViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                // 🔹 1. Save in Database
                _context.Contacts.Add(model);
                await _context.SaveChangesAsync();

                // 🔹 2. Send Email
                var email = new MimeMessage();

                email.From.Add(new MailboxAddress(
                    _emailSettings.SenderName,
                    _emailSettings.SenderEmail));

                email.To.Add(new MailboxAddress(
                    "Shahbaz",
                    _emailSettings.SenderEmail));

                email.Subject = model.Subject;

                email.Body = new TextPart("plain")
                {
                    Text = $"New Contact Message:\n\n" +
                           $"Name: {model.Name}\n" +
                           $"Email: {model.Email}\n\n" +
                           $"Message:\n{model.Message}"
                };

                using (var smtp = new SmtpClient())
                {
                    await smtp.ConnectAsync(
                        _emailSettings.SmtpServer,
                        _emailSettings.Port,
                        SecureSocketOptions.StartTls);

                    await smtp.AuthenticateAsync(
                        _emailSettings.Username,
                        _emailSettings.Password);

                    await smtp.SendAsync(email);
                    await smtp.DisconnectAsync(true);
                }

                ViewBag.SuccessMessage = "Your message has been sent successfully!";
                ModelState.Clear();

                return View(new ContactViewModel());
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Something went wrong while sending message.";
                return View(model);
            }
        }
    }
}
