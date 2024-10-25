using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Onix.Core.Abstraction;
using Onix.Core.Extensions;
using Onix.SharedKernel;
using Onix.SharedKernel.ValueObjects.Ids;
using Onix.WebSites.Application.Database;
using Onix.WebSites.Domain.Blocks.ValueObjects;

namespace Onix.WebSites.Application.Commands.Blocks.Update;

public class UpdateBlockHandler
{
    private readonly ILogger<UpdateBlockHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<UpdateBlockCommand> _validator;
    private readonly IWebSiteRepository _webSiteRepository;

    public UpdateBlockHandler(
        IValidator<UpdateBlockCommand> validator,
        IWebSiteRepository webSiteRepository,
        IUnitOfWork unitOfWork,
        ILogger<UpdateBlockHandler> logger)
    {
        _validator = validator;
        _webSiteRepository = webSiteRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        UpdateBlockCommand command, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToList();

        var webSiteId = WebSiteId.Create(command.WebSiteId);

        var webSiteResult = await _webSiteRepository
            .GetByIdWithBlocks(webSiteId, cancellationToken);
        if (webSiteResult.IsFailure)
            return webSiteResult.Error.ToErrorList();

        var blockId = BlockId.Create(command.BlockId);
        var code = Code.Create(command.Code).Value;

        var blockResult = webSiteResult.Value.Blocks
            .FirstOrDefault(b => b.Id == blockId);
        if (blockResult is null)
            return Errors.General.NotFound(blockId.Value).ToErrorList();

        var result = blockResult.Update(code);
        if (result.IsFailure)
            return result.Error.ToErrorList();

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return blockResult.Id.Value;
    }
}