namespace MaybeFinal.Models
{
    public class Accountant
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<User> Users { get; set; }
    }

}
