// GetCompanyByIdHandler.cs
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RemoteJobBoard.Application.DTOs.Company;
using RemoteJobBoard.Core.Exceptions;
using RemoteJobBoard.Infrastructure.Data;

namespace RemoteJobBoard.Application.Features.Companies.Queries.GetCompanyById;

public class GetCompanyByIdHandler : IRequestHandler<GetCompanyByIdQuery, CompanyDto>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public GetCompanyByIdHandler(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CompanyDto> Handle(
        GetCompanyByIdQuery request,
        CancellationToken cancellationToken)
    {
        var company = await _context.Companies
            .Include(c => c.Reviews)
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (company is null)
            throw new NotFoundException("Company", request.Id);

        return _mapper.Map<CompanyDto>(company);
    }
}