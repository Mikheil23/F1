using static MaybeFinal.EnumsForApp.Enums;

namespace MaybeFinal.Models
{
    public class LoanRequestDto
    {
        public string UserId { get; set; }
        public LoanType LoanType { get; set; }
        public decimal Amount { get; set; }
        public Currency Currency { get; set; }
        public int LoanPeriod { get; set; } // in months
    }

}
