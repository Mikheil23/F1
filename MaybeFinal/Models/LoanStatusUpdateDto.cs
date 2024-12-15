namespace MaybeFinal.Models
{
    public class LoanStatusUpdateDto
    {
        public string Status { get; set; }
        public string? Comment { get; set; } // Optional comment (if you want additional context)
    }
}
