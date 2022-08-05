using System.ComponentModel.DataAnnotations;

namespace GroupProjectBackEndV2.Data.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public StudentProgram? Program { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Admin { get; set; } = false;
        public ICollection<TimeSpend>? TimeSpends { get; set; }


        //public int? CurrentSessionId { get; set; }
        //public TimeSpend? CurrentSession { get; set; }
    }
}
