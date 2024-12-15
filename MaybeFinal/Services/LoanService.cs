using MaybeFinal.Contexts;
using MaybeFinal.Models;
using Microsoft.EntityFrameworkCore;
using static MaybeFinal.EnumsForApp.Enums;

namespace MaybeFinal.Services
{
    public class LoanService
    {
        private readonly MaybeFinalDbContext _context;

        public LoanService(MaybeFinalDbContext context)
        {
            _context = context;
        }

        // User Requests Loan
        public async Task<OperationResult> RequestLoanAsync(LoanRequestDto loanRequest)
        {
            var user = await _context.Users.FindAsync(loanRequest.UserId);
            if (user == null || user.IsBlocked)
            {
                return new OperationResult
                {
                    IsSuccess = false,
                    Message = "User is blocked or does not exist."
                };
            }

            var loan = new Loan
            {
                LoanType = loanRequest.LoanType,
                Amount = loanRequest.Amount,
                Currency = loanRequest.Currency,
                LoanPeriod = loanRequest.LoanPeriod,
                Status = LoanStatus.InProgress,
                UserId = loanRequest.UserId.ToString()
            };

            _context.Loans.Add(loan);
            await _context.SaveChangesAsync();

            return new OperationResult
            {
                IsSuccess = true,
                Message = "Loan request is in progress."
            };
        }

        // View Loans for a specific user
        public async Task<List<Loan>> ViewUserLoansAsync(int userId)
        {
            var user = await _context.Users.Include(u => u.Loans)
        .FirstOrDefaultAsync(u => u.Id == userId.ToString());  // Converting userId to string, assuming Id is of type string

            // Return the loans of the user, or an empty list if no user is found
            return user?.Loans ?? new List<Loan>();
        }

        // Update Loan details (Update status & optional comment)
        // Update Loan details (Update status & optional comment)
        public async Task<string> UpdateLoanStatusAsync(int loanId, LoanStatusUpdateDto loanStatusUpdate)
        {
            var loan = await _context.Loans.FindAsync(loanId);
            if (loan == null)
                return "Loan not found.";

            try
            {
                // Ensure the status is valid and map it
                if (Enum.TryParse<LoanStatus>(loanStatusUpdate.Status, true, out var parsedStatus))
                {
                    loan.Status = parsedStatus;
                }
                else
                {
                    return "Invalid loan status.";
                }

                // Optionally update comment, if provided
                if (!string.IsNullOrEmpty(loanStatusUpdate.Comment))
                {
                    loan.Comment = loanStatusUpdate.Comment;
                }

                await _context.SaveChangesAsync();
                return "Loan status updated successfully.";
            }
            catch (DbUpdateException ex)
            {
                // Log the error (you can also log the exception here)
                return $"Error updating loan status: {ex.Message}";
            }
        }


        // Delete Loan
        public async Task<string> DeleteLoanAsync(int loanId)
        {
            var loan = await _context.Loans.FindAsync(loanId);
            if (loan == null)
                return "Loan not found.";

            _context.Loans.Remove(loan);
            await _context.SaveChangesAsync();
            return "Loan deleted.";
        }

        // Block User
        public async Task<string> BlockUserAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return "User not found.";

            user.IsBlocked = true;
            await _context.SaveChangesAsync();
            return "User blocked.";
        }

        internal async Task<string> ChangeLoanAsync(int loanId, LoanChangeDto loanChange)
        {
            throw new NotImplementedException();
        }
    }
}


