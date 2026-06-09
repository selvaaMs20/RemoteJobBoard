using MediatR;
using RemoteJobBoard.Application.DTOs.Company;

namespace RemoteJobBoard.Application.Features.Companies.Queries.GetCompanyById;

public record GetCompanyByIdQuery(Guid Id) : IRequest<CompanyDto>;