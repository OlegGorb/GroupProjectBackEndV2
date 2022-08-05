namespace GroupProjectBackEndV2.Data.Models
{
    public class TimeSpend
    {
        public int Id { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public User User { get; set; }
    }
}
