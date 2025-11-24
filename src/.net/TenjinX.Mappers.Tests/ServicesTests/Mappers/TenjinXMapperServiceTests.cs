using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using TenjinX.Mappers.Enums;
using TenjinX.Mappers.Exceptions;
using TenjinX.Mappers.Extensions;
using TenjinX.Mappers.Interfaces.Services;
using TenjinX.Mappers.Tests.TestFixtures;
using TenjinX.Mappers.Tests.TestModels;

namespace TenjinX.Mappers.Tests.ServicesTests.Mappers;

public class TenjinXMapperServiceTests
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ITenjinXMapperService _mapperService;

    public TenjinXMapperServiceTests()
    {
        var services = new ServiceCollection();

        services.AddTenjinXMappersAndService(typeof(TenjinXMapperServiceTests).Assembly);

        _serviceProvider = services.BuildServiceProvider();
        _mapperService = _serviceProvider.GetRequiredService<ITenjinXMapperService>();
    }

    [Fact]
    public void Map_WhenGivenValidSourceAndDestination_MapsSuccessfully()
    {
        var destination = new TestModelB();

        _mapperService.Map(ModelTestFixtures.StartA, destination);

        destination.Should().BeEquivalentTo(ModelTestFixtures.FromAToB);
    }

    [Fact]
    public void Map_WhenGivenValidSourceAndDestinationMultipleTimes_MapsSuccessfully()
    {
        var destination = new TestModelB();

        _mapperService.Map(ModelTestFixtures.StartA, destination);
        _mapperService.Map(ModelTestFixtures.StartA, destination);

        destination.Should().BeEquivalentTo(ModelTestFixtures.FromAToB);
    }

    [Theory]
    [InlineData(null, null)]
    [InlineData(null, "NonNullDestination")]
    [InlineData("NonNullSource", null)]
    public void Map_WhenGivenNullParameters_ThrowsAnException(object? source, object? destination)
    {
        var act = () => _mapperService.Map(source!, destination!);

        act.Should().Throw<TenjinXMapperException>();
    }

    [Fact]
    public void Map_WhenGivenUnsupportedMapping_ThrowsAnException()
    {
        var source = new TestModelA();
        var destination = new TestModelC();
        var act = () => _mapperService.Map(source, destination);

        act.Should().Throw<TenjinXMapperException>();
    }

    [Fact]
    public void Map_WhenUsingAServiceProviderWhereTheMapperHasADifferentScopeThanTheService_StillWorksAsExpected()
    {
        var services = new ServiceCollection();

        services.AddTenjinXMappers(typeof(TenjinXMapperServiceTests).Assembly, TenjinXMapperScope.Transient);
        services.AddTenjinXMapperService(TenjinXMapperScope.Singleton);

        var serviceProvider = services.BuildServiceProvider();
        var mapperService = serviceProvider.GetRequiredService<ITenjinXMapperService>();
        var destination = new TestModelB();
        using var customScope = serviceProvider.CreateScope();

        mapperService.Map(customScope.ServiceProvider, ModelTestFixtures.StartA, destination);

        destination.Should().BeEquivalentTo(ModelTestFixtures.FromAToB);
    }

    [Fact]
    public void Map_WhenMultipleThreadsAccessTheService_IsThreadSafeAndWorksCorrectly()
    {
        var tasks = new List<Func<Task>>();

        for (var i = 0; i < 10; i++)
        {
            Task task()
            {
                Task.Delay(100);

                for (var j = 0; j < 1000; j++)
                {
                    if (j % 5 == 0)
                    {
                        var destination = new TestModelB();

                        _mapperService.Map(ModelTestFixtures.StartA, destination);

                        destination.Should().BeEquivalentTo(ModelTestFixtures.FromAToB);
                    }
                    else if (j % 5 == 1)
                    {
                        var destination = new TestModelA();

                        _mapperService.Map(ModelTestFixtures.StartB, destination);
                        destination.Should().BeEquivalentTo(ModelTestFixtures.FromBToA);
                    }
                    else if (j % 5 == 2)
                    {
                        var destination = new TestModelC();

                        _mapperService.Map(ModelTestFixtures.StartB, destination);
                        destination.Should().BeEquivalentTo(ModelTestFixtures.FromBToC);
                    }
                    else if (j % 5 == 3)
                    {
                        var destination = new TestModelB();

                        _mapperService.Map(ModelTestFixtures.StartC, destination);
                        destination.Should().BeEquivalentTo(ModelTestFixtures.FromCToB);
                    }
                    else
                    {
                        var destination = new TestModelD();

                        _mapperService.Map(ModelTestFixtures.StartC, destination);

                        destination.Should().BeEquivalentTo(ModelTestFixtures.FromCToD);
                    }
                }

                return Task.CompletedTask;
            }

            tasks.Add(task);
        }

        Parallel.ForEach(tasks, task => task().Wait());
    }
}
