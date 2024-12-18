using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Onix.Core.Abstraction;
using Onix.Core.Extensions;
using Onix.SharedKernel;
using Onix.SharedKernel.ValueObjects;
using Onix.SharedKernel.ValueObjects.Ids;
using Onix.WebSites.Application.Database;

namespace Onix.WebSites.Application.Commands.WebSites.UpdateContact;

public class UpdateContactHandler
{
    private readonly ILogger<UpdateContactHandler> _logger;
    private readonly IWebSiteRepository _webSiteRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<UpdateContactCommand> _validator;

    public UpdateContactHandler(
        ILogger<UpdateContactHandler> logger,
        IWebSiteRepository webSiteRepository,
        IUnitOfWork unitOfWork,
        IValidator<UpdateContactCommand> validator)
    {
        _logger = logger;
        _webSiteRepository = webSiteRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<UnitResult<ErrorList>> Handle(
        UpdateContactCommand command, CancellationToken cancellationToken = default)
    {
        var validator = await _validator.ValidateAsync(command, cancellationToken);
        if (!validator.IsValid)
            return validator.ToList();
        
        var webSiteId = WebSiteId.Create(command.WebSiteId);
        
        var webSiteResult = await _webSiteRepository
            .GetById(webSiteId, cancellationToken);
        if (webSiteResult.IsFailure)
            return webSiteResult.Error.ToErrorList();

        var email = Email.Create(command.Email).Value;
        var phone = Phone.Create(command.Phone).Value;
        
        var result = webSiteResult.Value.UpdateContact(phone, email);
        if (result.IsFailure)
            return result.Error.ToErrorList();

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return UnitResult.Success<ErrorList>();
    }
}