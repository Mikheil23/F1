using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using static MaybeFinal.EnumsForApp.Enums;

namespace MaybeFinal.Models
{
    public class User : IdentityUser<string>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public decimal Salary { get; set; }
        public bool IsBlocked { get; set; } = false;
        public string PasswordHash { get; set; }
        public List<Loan> Loans { get; set; }
        public string Role { get; set; } // Custom Role property

        // Foreign key for Accountant
        public string AccountantId { get; set; }
        public Accountant Accountant { get; set; }  // Navigation property
    }











}
