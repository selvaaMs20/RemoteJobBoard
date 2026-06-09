using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RemoteJobBoard.Application.DTOs.Company;
using RemoteJobBoard.Core.Entities;
using RemoteJobBoard.Core.Exceptions;
using RemoteJobBoard.Infrastructure.Data;

namespace RemoteJobBoard.Application.Features.Companies.Commands.CreateCompany;

public class CreateCompanyHandler : IRequestHandler<CreateCompanyCommand, CompanyDto>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public CreateCompanyHandler(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CompanyDto> Handle(
        CreateCompanyCommand request,
        CancellationToken cancellationToken)
    {
        // Check owner exists
        var owner = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == request.OwnerId, cancellationToken);

        if (owner is null)
            throw new NotFoundException("User", request.OwnerId);

        // Check company name not already taken
        var exists = await _context.Companies
            .AnyAsync(c => c.Name == request.Name, cancellationToken);

        if (exists)
            throw new ConflictException(
                $"A company with the name '{request.Name}' already exists.");

        var company = new Company
        {
            OwnerId = request.OwnerId,
            Name = request.Name,
            Website = request.Website,
            Location = request.Location,
            Industry = request.Industry,
            Description = request.Description
        };

        _context.Companies.Add(company);
        await _context.SaveChangesAsync(cancellationToken);

        // Reload with reviews for mapping
        var created = await _context.Companies
            .Include(c => c.Reviews)
            .FirstAsync(c => c.Id == company.Id, cancellationToken);

        return _mapper.Map<CompanyDto>(created);
    }
}