using FluentValidation;
using MaybeFinal.Models;

namespace MaybeFinal.Validations
{
    public class LoanRequestDtoValidator : AbstractValidator<LoanRequestDto>
    {
        public LoanRequestDtoValidator()
        {
            RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Amount must be greater than 0.");
            RuleFor(x => x.LoanPeriod).GreaterThan(0).WithMessage("Loan period must be greater than 0.");
            RuleFor(x => x.UserId).NotEmpty().WithMessage("User ID is required.");
        }
    }


}
