using Onix.Core.Abstraction;

namespace Onix.WebSites.Application.Commands.Locations.Add;

public record AddLocationCommand(
    Guid WebSiteId,
    string Name,
    string Phone,
    string City,
    string Street,
    string Build,
    string Index) : ICommand;