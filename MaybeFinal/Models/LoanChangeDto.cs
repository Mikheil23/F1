using static MaybeFinal.EnumsForApp.Enums;

namespace MaybeFinal.Models
{
    public class LoanChangeDto
    {
        public decimal Amount { get; set; }
        public int LoanPeriod { get; set; } // In months
        public LoanStatus Status { get; set; } // Enum
    }

}
