// Ignore Spelling: Datacom

using DatacomTest.Server.Models;
using DatacomTest.Server.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DatacomTest.Server.Repositories;

public class RepositoryApplications : RepositoryBase<Application>, IRepositoryApplications
{
    //private readonly ApplicationDbContext _context;
    private readonly DbSet<Application> applications;

    public RepositoryApplications(ApplicationDbContext context, ILogger<RepositoryApplications> logger) : base(context, logger)
    {
        applications = _context.Set<Application>();
    }
}