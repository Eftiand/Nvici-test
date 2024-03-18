using API.Data;
using IntegrationTests.Init;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace IntegrationTests;

public class BaseIntegrationTest : IClassFixture<IntegrationWebAppFactory>, IDisposable
{
    protected readonly IServiceScope Scope;
    protected readonly AppDbContext _dbContext;

    protected BaseIntegrationTest(IntegrationWebAppFactory factory)
    {
        Scope = factory.Services.CreateScope();
        _dbContext = Scope.ServiceProvider.GetRequiredService<AppDbContext>();
    }

    public void Dispose()
    {
        Scope?.Dispose();
        _dbContext?.Dispose();
    }
}