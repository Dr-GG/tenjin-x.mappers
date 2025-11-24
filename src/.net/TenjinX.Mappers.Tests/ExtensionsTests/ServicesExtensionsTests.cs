using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using TenjinX.Mappers.Enums;
using TenjinX.Mappers.Exceptions;
using TenjinX.Mappers.Extensions;
using TenjinX.Mappers.Interfaces.Mappers;
using TenjinX.Mappers.Interfaces.Services;
using TenjinX.Mappers.Services.Mappers;
using TenjinX.Mappers.Tests.Implementations;
using TenjinX.Mappers.Tests.TestModels;

namespace TenjinX.Mappers.Tests.ExtensionsTests;

public class ServicesExtensionsTests
{
    private readonly IServiceCollection _services = new ServiceCollection();

    [Fact]
    public void AddTenjinXMappers_WhenCalledWithAnUnknownScope_ThrowsAnException()
    {
        var action = () => _services.AddTenjinXMappers
        (
            typeof(ServicesExtensionsTests).Assembly, 
            TenjinXMapperScope.Unknown
        );

        action.Should().Throw<TenjinXMapperException>();
    }

    [Theory]
    [InlineData(TenjinXMapperScope.Scoped)]
    [InlineData(TenjinXMapperScope.Singleton)]
    [InlineData(TenjinXMapperScope.Transient)]
    public void AddTenjinXMappers_WhenGivenAnAssembly_RegistersAllMappers(TenjinXMapperScope scope)
    {
        var assembly = typeof(ServicesExtensionsTests).Assembly;

        _services.AddTenjinXMappers(assembly, scope);

        AssertRegisteredMappers(scope);
    }

    [Fact]
    public void AddTenjinXMapperService_WhenCalledWithAnUnknownScope_ThrowsAnException()
    {
        var action = () => _services.AddTenjinXMapperService(TenjinXMapperScope.Unknown);

        action.Should().Throw<TenjinXMapperException>();
    }

    [Theory]
    [InlineData(TenjinXMapperScope.Scoped)]
    [InlineData(TenjinXMapperScope.Singleton)]
    [InlineData(TenjinXMapperScope.Transient)]
    public void AddTenjinXMapperService_WhenCalled_RegistersOnlyMapperService(TenjinXMapperScope scope)
    {
        _services.AddTenjinXMapperService(scope);

        AssertNoRegisteredMappers();
        AssertServiceRegistered<ITenjinXMapperService, TenjinXMapperService>(_services, scope);
    }

    [Fact]
    public void AddTenjinXMappersAndService_WhenCalledWithAnUnknownScope_ThrowsAnException()
    {
        var action = () => _services.AddTenjinXMappersAndService
        (
            typeof(ServicesExtensionsTests).Assembly, 
            TenjinXMapperScope.Unknown
        );

        action.Should().Throw<TenjinXMapperException>();
    }

    [Theory]
    [InlineData(TenjinXMapperScope.Scoped)]
    [InlineData(TenjinXMapperScope.Singleton)]
    [InlineData(TenjinXMapperScope.Transient)]
    public void AddTenjinXMappersAndService_WhenGivenAnAssembly_RegistersMappersAndMapperService(TenjinXMapperScope scope)
    {
        var assembly = typeof(ServicesExtensionsTests).Assembly;

        _services.AddTenjinXMappersAndService(assembly, scope);

        AssertRegisteredMappers(scope);
        AssertServiceRegistered<ITenjinXMapperService, TenjinXMapperService>(_services, scope);
    }

    private void AssertRegisteredMappers(TenjinXMapperScope scope)
    {
        AssertServiceRegistered<ITenjinXMapper<TestModelA, TestModelB>, AToBMapper>(_services, scope);
        AssertServiceRegistered<ITenjinXMapper<TestModelB, TestModelA>, BToAMapper>(_services, scope);
        AssertServiceRegistered<ITenjinXMapper<TestModelB, TestModelC>, BToCAndCToBMapper>(_services, scope);
        AssertServiceRegistered<ITenjinXMapper<TestModelC, TestModelB>, BToCAndCToBMapper>(_services, scope);
        AssertServiceRegistered<ITenjinXMapper<TestModelC, TestModelD>, CToDMapper>(_services, scope);

        AssertServiceNotRegistered<AbstractMapper>(_services);
    }

    private void AssertNoRegisteredMappers()
    {
        AssertServiceNotRegistered<ITenjinXMapper<TestModelA, TestModelB>>(_services);
        AssertServiceNotRegistered<ITenjinXMapper<TestModelB, TestModelA>>(_services);
        AssertServiceNotRegistered<ITenjinXMapper<TestModelB, TestModelC>>(_services);
        AssertServiceNotRegistered<ITenjinXMapper<TestModelC, TestModelB>>(_services);
        AssertServiceNotRegistered<ITenjinXMapper<TestModelC, TestModelD>>(_services);
        AssertServiceNotRegistered<AbstractMapper>(_services);
    }

    private static void AssertServiceNotRegistered<TService>(IServiceCollection services)
    {
        var descriptor = services.FirstOrDefault(d => d.ServiceType == typeof(TService));

        Assert.Null(descriptor);
    }

    private static void AssertServiceRegistered<TService, TImplementation>
    (
        IServiceCollection services, 
        TenjinXMapperScope scope
    )
        where TImplementation : TService
    {
        var lifetime = GetLifetime(scope);
        var descriptor = services.FirstOrDefault
        (d =>
            d.ServiceType == typeof(TService) &&
            d.ImplementationType == typeof(TImplementation) &&
            d.Lifetime == lifetime
        );

        Assert.NotNull(descriptor);
    }

    private static ServiceLifetime GetLifetime(TenjinXMapperScope scope)
    {
        return scope switch
        {
            TenjinXMapperScope.Scoped => ServiceLifetime.Scoped,
            TenjinXMapperScope.Singleton => ServiceLifetime.Singleton,
            TenjinXMapperScope.Transient => ServiceLifetime.Transient,
            _ => ServiceLifetime.Scoped
        };
    }
}
