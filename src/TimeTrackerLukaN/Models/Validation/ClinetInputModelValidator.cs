using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;


namespace TimeTrackerLukaN.Models.Validation
{
    public class ClinetInputModelValidator : AbstractValidator<ClientInputModel>
    {
        public ClinetInputModelValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
            .Length(3, 100);

            //RuleFor(x => x.)
        }
    }
}
