using System.ComponentModel.DataAnnotations;

namespace GroupProjectBackEndV2.Data.Models
{
    public class StudentProgram
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<User>? Students { get; set; }
    }
}
