using static MaybeFinal.EnumsForApp.Enums;

namespace MaybeFinal.Models
{
    public class Loan
    {
        public string Id { get; set; }
        public LoanType LoanType { get; set; }
        public decimal Amount { get; set; }
        public Currency Currency { get; set; }
        public int LoanPeriod { get; set; }  // in months
        public LoanStatus Status { get; set; } = LoanStatus.InProgress;
        public string UserId { get; set; }
        public User User { get; set; }
        public string Comment { get; internal set; }
    }

}
