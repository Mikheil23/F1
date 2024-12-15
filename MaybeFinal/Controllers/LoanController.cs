using MaybeFinal.Models;
using MaybeFinal.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MaybeFinal.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoanController : ControllerBase
    {
        private readonly LoanService _loanService;
        private readonly ILogger<LoanController> _logger;

        public LoanController(LoanService loanService, ILogger<LoanController> logger)
        {
            _loanService = loanService;
            _logger = logger;
        }

        #region User Endpoints

        [Authorize(Roles = "User")]
        [HttpPost("request-loan")]
        public async Task<IActionResult> RequestLoan([FromBody] LoanRequestDto loanRequest)
        {
            if (loanRequest == null || string.IsNullOrEmpty(loanRequest.UserId))
            {
                _logger.LogWarning("Invalid loan request received: {Request}", loanRequest);
                return BadRequest("Invalid loan request.");
            }

            _logger.LogInformation("Received loan request for user with ID: {UserId}", loanRequest.UserId);

            var result = await _loanService.RequestLoanAsync(loanRequest);

            if (!result.IsSuccess)  // Check IsSuccess from OperationResult
            {
                _logger.LogWarning("Loan request rejected for user with ID: {UserId}, reason: {Reason}", loanRequest.UserId, result.Message);
                return Unauthorized(result.Message);  // Use the message from the OperationResult
            }

            _logger.LogInformation("Loan request in progress for user with ID: {UserId}.", loanRequest.UserId);
            return Ok(result.Message);  // Return the message from the OperationResult
        }


        #endregion

        #region Accountant Endpoints

        [Authorize(Roles = "Accountant")]
        [HttpGet("view-loans")]
        public async Task<IActionResult> ViewUserLoans(int userId)
        {
            if (userId <= 0)
            {
                _logger.LogWarning("Invalid user ID: {UserId} received in request.", userId);
                return BadRequest("Invalid user ID.");
            }

            var loans = await _loanService.ViewUserLoansAsync(userId);

            if (loans == null || !loans.Any())
            {
                _logger.LogWarning("No loans found for user with ID: {UserId}", userId);
                return NotFound("User or loans not found.");
            }

            _logger.LogInformation("Loans retrieved for user with ID: {UserId}", userId);
            return Ok(loans);
        }

        [Authorize(Roles = "Accountant")]
        [HttpPut("change-loan")]
        public async Task<IActionResult> ChangeLoan(int loanId, [FromBody] LoanChangeDto loanChange)
        {
            if (loanId <= 0 || loanChange == null)
            {
                _logger.LogWarning("Invalid loan change request received for loan ID: {LoanId}", loanId);
                return BadRequest("Invalid loan change request.");
            }

            var result = await _loanService.ChangeLoanAsync(loanId, loanChange);

            if (result == "Loan not found.")  // This is based on the assumption that ChangeLoanAsync returns string, adjust it to handle the OperationResult if needed
            {
                _logger.LogWarning("Loan with ID {LoanId} not found for change request.", loanId);
                return NotFound(result);
            }

            _logger.LogInformation("Loan with ID {LoanId} successfully updated.", loanId);
            return Ok(result);
        }

        [Authorize(Roles = "Accountant")]
        [HttpDelete("delete-loan")]
        public async Task<IActionResult> DeleteLoan(int loanId)
        {
            if (loanId <= 0)
            {
                _logger.LogWarning("Invalid loan ID: {LoanId} received for deletion.", loanId);
                return BadRequest("Invalid loan ID.");
            }

            var result = await _loanService.DeleteLoanAsync(loanId);

            if (result == "Loan not found.")  // Similar to the change-loan endpoint, this could be based on the string result from your service method
            {
                _logger.LogWarning("Loan with ID {LoanId} not found for deletion.", loanId);
                return NotFound(result);
            }

            _logger.LogInformation("Loan with ID {LoanId} successfully deleted.", loanId);
            return Ok(result);
        }

        [Authorize(Roles = "Accountant")]
        [HttpPut("block-user")]
        public async Task<IActionResult> BlockUser(int userId)
        {
            if (userId <= 0)
            {
                _logger.LogWarning("Invalid user ID: {UserId} received for blocking.", userId);
                return BadRequest("Invalid user ID.");
            }

            var result = await _loanService.BlockUserAsync(userId);

            if (result == "User not found.")  // Same as above, checking for string results from service
            {
                _logger.LogWarning("User with ID {UserId} not found for blocking.", userId);
                return NotFound(result);
            }

            _logger.LogInformation("User with ID {UserId} successfully blocked.", userId);
            return Ok(result);
        }

        #endregion
    }
}


