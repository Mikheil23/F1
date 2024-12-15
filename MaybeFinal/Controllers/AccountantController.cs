using MaybeFinal.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MaybeFinal.Models;

namespace MaybeFinal.Controllers
{
    [Authorize(Roles = "Accountant")]
    [ApiController]
    [Route("api/[controller]")]
    public class AccountantController : ControllerBase
    {
        private readonly LoanService _loanService;
        private readonly ILogger<AccountantController> _logger;

        public AccountantController(LoanService loanService, ILogger<AccountantController> logger)
        {
            _loanService = loanService;
            _logger = logger;
        }

        [HttpPut("update-loan-status")]
        public async Task<IActionResult> UpdateLoanStatus(int loanId, [FromBody] LoanStatusUpdateDto loanStatusUpdate)
        {
            try
            {
                // Correct method call to update loan status
                var result = await _loanService.UpdateLoanStatusAsync(loanId, loanStatusUpdate);

                if (result == "Loan not found.")
                {
                    _logger.LogWarning($"Loan with ID {loanId} not found.");
                    return NotFound(result);
                }

                // Log successful loan status update
                _logger.LogInformation($"Loan with ID {loanId} status updated to {loanStatusUpdate.Status}.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating the loan status for loan ID {loanId}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("block-user")]
        public async Task<IActionResult> BlockUser(int userId)
        {
            try
            {
                var result = await _loanService.BlockUserAsync(userId);

                if (result == "User not found.")
                {
                    _logger.LogWarning($"User with ID {userId} not found.");
                    return NotFound(result);
                }

                // Log user blocking
                _logger.LogInformation($"User with ID {userId} has been blocked.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while blocking user with ID {userId}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}



