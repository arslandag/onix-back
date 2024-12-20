using FluentValidation;
using Onix.Core.Validation;
using Onix.SharedKernel;
using Onix.SharedKernel.ValueObjects;
using Onix.WebSites.Domain.WebSites.ValueObjects;

namespace Onix.WebSites.Application.Commands.WebSites.Create;

public class CreateWebSiteValidator : AbstractValidator<CreateWebSiteCommand>
{
    public CreateWebSiteValidator()
    {
        RuleFor(c => c.Url)
            .NotEmpty()
            .WithError(Errors.Domain.Required(ConstType.Url));
        
        RuleFor(c => c.Url)
            .MaximumLength(Constants.URL_MAX_LENGTH)
            .WithError(Errors.Domain.MaxLength(ConstType.Url));
        
        RuleFor(c => c.Url)
            .MinimumLength(Constants.URL_MIN_LENGTH)
            .WithError(Errors.Domain.MinLength(ConstType.Url));

        RuleFor(c => c.Url)
            .Matches(Constants.URL_REGEX)
            .WithError(Errors.Domain.Invalid(ConstType.Url));

        RuleFor(c => c.Name)
            .NotEmpty()
            .WithError(Errors.Domain.Required(ConstType.Name));

        RuleFor(c => c.Name)
            .MaximumLength(Constants.NAME_MAX_LENGTH)
            .WithError(Errors.Domain.MaxLength(ConstType.Name));
        
        RuleFor(c => c.Name)
            .MinimumLength(Constants.NAME_MIN_LENGTH)
            .WithError(Errors.Domain.MaxLength(ConstType.Name));
    }
}