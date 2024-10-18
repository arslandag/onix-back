using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Onix.Core.Abstraction;
using Onix.Core.Extensions;
using Onix.SharedKernel;
using Onix.SharedKernel.ValueObjects;
using Onix.WebSites.Application.Database;
using Onix.WebSites.Application.Queries.GetWebSiteByUrl;
using Onix.WebSites.Domain.WebSites.ValueObjects;

namespace Onix.WebSites.Application.Commands.WebSites.Update;

public class UpdateWebSiteHandle
{
    private readonly IValidator<UpdateWebSiteCommand> _validator;
    private readonly IWebSiteRepository _webSiteRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly GetWebSiteByUrlHandle _getWebSiteByUrlHandle;
    private readonly ILogger<UpdateWebSiteHandle> _logger;

    public UpdateWebSiteHandle(
        IValidator<UpdateWebSiteCommand> validator,
        IWebSiteRepository webSiteRepository,
        IUnitOfWork unitOfWork,
        GetWebSiteByUrlHandle getWebSiteByUrlHandle,
        ILogger<UpdateWebSiteHandle> logger)
    {
        _validator = validator;
        _webSiteRepository = webSiteRepository;
        _unitOfWork = unitOfWork;
        _getWebSiteByUrlHandle = getWebSiteByUrlHandle;
        _logger = logger;
    }
    
    public async Task<Result<Guid, ErrorList>> Handle(
        UpdateWebSiteCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToList();

        var url = Url.Create(command.Url).Value;

        var query = new GetWebSiteByUrlQuery(url.Value);
        var existingWebsite = await _getWebSiteByUrlHandle.Handle(query, cancellationToken);

        if (existingWebsite.IsSuccess)
            return Errors.Domain.AlreadyExist(nameof(url)).ToErrorList();

        var webSiteResult = await _webSiteRepository.GetById(command.WebSiteId, cancellationToken);
        if (webSiteResult.IsFailure)
            return webSiteResult.Error.ToErrorList();

        var name = Name.Create(command.Name).Value;
        webSiteResult.Value.Update(url, name);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Updated website with ID {WebsiteId}", command.WebSiteId);

        return webSiteResult.Value.Id.Value;
    }
}