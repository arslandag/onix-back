using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Onix.Core.Abstraction;
using Onix.Core.Extensions;
using Onix.SharedKernel;
using Onix.SharedKernel.ValueObjects;
using Onix.SharedKernel.ValueObjects.Ids;
using Onix.WebSites.Application.Database;
using Onix.WebSites.Domain.Blocks;
using Onix.WebSites.Domain.WebSites.ValueObjects;

namespace Onix.WebSites.Application.Commands.AddBlock;

public class AddBlockHandler
{
    private readonly IValidator<AddBlockCommand> _validator;
    private readonly IWebSiteRepository _webSiteRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AddBlockCommand> _logger;

    public AddBlockHandler(
        IValidator<AddBlockCommand> validator,
        IWebSiteRepository webSiteRepository,
        IUnitOfWork unitOfWork,
        ILogger<AddBlockCommand> logge)
    {
        _validator = validator;
        _webSiteRepository = webSiteRepository;
        _unitOfWork = unitOfWork;
        _logger = logge;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        AddBlockCommand command ,CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command,cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToList();

        var webSiteId = WebSiteId.Create(command.WebSiteId);
        
        //сделать через рид
        var webSiteResult = await _webSiteRepository
            .GetById(webSiteId, cancellationToken);

        if (webSiteResult.IsFailure)
            return webSiteResult.Error.ToErrorList();

        var blockId = BlockId.NewUserId();
        var title = Title.Create(command.Title).Value;
        var description = Description.Create(command.Description).Value;
        
        var block = Block.Create(blockId, title, description).Value;

        webSiteResult.Value.AddBlock(block);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return block.Id.Value;
    }
}