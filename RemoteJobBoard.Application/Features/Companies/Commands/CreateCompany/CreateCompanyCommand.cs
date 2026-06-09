
using MediatR;
using RemoteJobBoard.Application.DTOs.Company;

namespace RemoteJobBoard.Application.Features.Companies.Commands.CreateCompany;

public record CreateCompanyCommand(
    Guid OwnerId,
    string Name,
    string? Website,
    string? Location,
    string? Industry,
    string? Description
) : IRequest<CompanyDto>;