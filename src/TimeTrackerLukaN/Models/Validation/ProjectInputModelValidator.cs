using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;


namespace TimeTrackerLukaN.Models.Validation
{
    public class ProjectInputModelValidator : AbstractValidator<ProjectInputModel>
    {
        public ProjectInputModelValidator()
        {
            RuleFor(x => x.Name).NotEmpty().Length(1, 100);
            RuleFor(x => x.ClientId).GreaterThan(0).NotEmpty();

        }
    }
}
