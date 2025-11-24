using TenjinX.Mappers.Interfaces.Mappers;
using TenjinX.Mappers.Extensions;
using TenjinX.Mappers.Tests.Implementations;
using TenjinX.Mappers.Tests.TestFixtures;
using TenjinX.Mappers.Tests.TestModels;
using FluentAssertions;
using TenjinX.Mappers.Models;
using TenjinX.Mappers.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;
using TenjinX.Mappers.Exceptions;

namespace TenjinX.Mappers.Tests.ExtensionsTests;

public class TenjinXMapperServiceExtensionsTests
{
    private const int NumberOfTestItems = 100;

    private bool _invokedFunction;

    private readonly ITenjinXMapperService _mapperService;
    private readonly IServiceProvider _serviceProvider;

    public TenjinXMapperServiceExtensionsTests()
    {
        var services = new ServiceCollection();

        services.AddTenjinXMappersAndService(typeof(TenjinXMapperServiceExtensionsTests).Assembly);

        _serviceProvider = services.BuildServiceProvider();
        _mapperService = _serviceProvider.GetRequiredService<ITenjinXMapperService>();
    }

    [Fact]
    public void MapNew_WhenCalledWithNoFactory_MapsCorrectly()
    {
        var result = _mapperService.MapNew<TestModelB>(ModelTestFixtures.StartA);

        result.Should().BeEquivalentTo(ModelTestFixtures.FromAToB);
        _invokedFunction.Should().BeFalse();
    }

    [Fact]
    public void MapNew_WhenCalledWithFuncFactory_MapsCorrectly()
    {
        var result = _mapperService.MapNew<TestModelB>(ModelTestFixtures.StartA, GetFromModelAToBFactory());

        result.Should().BeEquivalentTo(ModelTestFixtures.FromAToB);
        _invokedFunction.Should().BeTrue();
    }

    [Fact]
    public void MapNew_WhenCalledWithFuncFactoryAndReturnsNull_ThrowsAnException()
    {
        Action act = () => _mapperService.MapNew<TestModelB>(ModelTestFixtures.StartA, GetNullModelBFactory());

        act.Should().Throw<TenjinXMapperException>();
    }

    [Fact]
    public void MapNew_WhenCalledWithTaskFunc_MapsCorrectly()
    {
        var result = _mapperService.MapNew<TestModelB>(ModelTestFixtures.StartA, GetFromModelAToBAsyncFactory());

        result.Should().BeEquivalentTo(ModelTestFixtures.FromAToB);
        _invokedFunction.Should().BeTrue();
    }

    [Fact]
    public void MapNew_WhenCalledWithTaskFuncFactoryAndReturnsNull_ThrowsAnException()
    {
        Action act = () => _mapperService.MapNew<TestModelB>(ModelTestFixtures.StartA, GetNullModelBSyncFactory());

        act.Should().Throw<TenjinXMapperException>();
    }

    [Fact]
    public void MapNullable_WhenSourceIsNull_DoesNotInvokeMapper()
    {
        var testModelB = new TestModelB();
        var originalB = testModelB with { };

        _mapperService.MapNullable(null, testModelB);

        _invokedFunction.Should().BeFalse();
        testModelB.Should().BeEquivalentTo(originalB);
    }

    [Fact]
    public void MapNullable_WhenDestinationIsNull_DoesNotInvokeMapper()
    {
        _mapperService.MapNullable<TestModelB>(ModelTestFixtures.StartA, null);

        _invokedFunction.Should().BeFalse();
    }

    [Fact]
    public void MapNullable_WhenTheSourceAndDestinationAreNotNull_InvokesMapper()
    {
        var testModelB = new TestModelB();

        _mapperService.MapNullable(ModelTestFixtures.StartA, testModelB);

        _invokedFunction.Should().BeFalse();
        testModelB.Should().BeEquivalentTo(ModelTestFixtures.FromAToB);
    }

    [Fact]
    public void MapNullableNew_WhenSourceIsNull_ReturnsNull()
    {
        var result = _mapperService.MapNullableNew<TestModelB>(null);

        _invokedFunction.Should().BeFalse();
        result.Should().BeNull();
    }

    [Fact]
    public void MapNullableNew_WhenSourceIsNotNullAndNoFactoryIsUsed_InvokesMapper()
    {
        var result = _mapperService.MapNullableNew<TestModelB>(ModelTestFixtures.StartA);

        _invokedFunction.Should().BeFalse();
        result.Should().BeEquivalentTo(ModelTestFixtures.FromAToB);
    }

    [Fact]
    public void MapNullableNew_WhenSourceIsNotNullAndFuncFactoryIsUsed_InvokesMapper()
    {
        var result = _mapperService.MapNullableNew<TestModelB>(ModelTestFixtures.StartA, GetFromModelAToBFactory());

        _invokedFunction.Should().BeTrue();
        result.Should().BeEquivalentTo(ModelTestFixtures.FromAToB);
    }

    [Fact]
    public void MapNullableNew_WhenSourceIsNotNullAndFuncFactoryIsUsedAndReturnsNull_ReturnsNull()
    {
        var result = _mapperService.MapNullableNew<TestModelB>(ModelTestFixtures.StartA, GetNullModelBFactory());

        result.Should().BeNull();
    }

    [Fact]
    public void MapNullableNew_WhenSourceIsNotNullAndTaskFuncFactoryIsUsed_InvokesMapper()
    {
        var result = _mapperService.MapNullableNew<TestModelB>(ModelTestFixtures.StartA, GetFromModelAToBAsyncFactory());

        _invokedFunction.Should().BeTrue();
        result.Should().BeEquivalentTo(ModelTestFixtures.FromAToB);
    }

    [Fact]
    public void MapNullableNew_WhenSourceIsNotNullAndTaskFuncFactoryIsUsedAndReturnsNull_ReturnsNull()
    {
        var result = _mapperService.MapNullableNew<TestModelB>(ModelTestFixtures.StartA, GetNullModelBSyncFactory());

        result.Should().BeNull();
    }

    [Fact]
    public void MapManyNew_WhenCalledToExistingCollectionWithNoFactory_MapsCorrectly()
    {
        var sources = GetInitialModelACollection().ToList();
        var destinations = new List<TestModelB>();

        _mapperService.MapManyNew(sources, destinations);

        TestDestinationModelBCollection(destinations);
    }

    [Fact]
    public void MapManyNew_WhenCalledToExistingCollectionWithFuncFactory_MapsCorrectly()
    {
        var sources = GetInitialModelACollection().ToList();
        var destinations = new List<TestModelB>();

        _mapperService.MapManyNew(sources, destinations, GetFromModelAToBFactory(true));

        TestDestinationModelBCollection(destinations);
    }

    [Fact]
    public void MapManyNew_WhenCalledToExistingCollectionWithFuncFactoryAndReturnsNull_ThrowsException()
    {
        var sources = GetInitialModelACollection().ToList();
        var destinations = new List<TestModelB>();
        var act = () => _mapperService.MapManyNew(sources, destinations, GetNullModelBSyncFactory());

        act.Should().Throw<TenjinXMapperException>();
    }

    [Fact]
    public void MapManyNew_WhenCalledToExistingCollectionWithTaskFuncFactory_MapsCorrectly()
    {
        var sources = GetInitialModelACollection().ToList();
        var destinations = new List<TestModelB>();

        _mapperService.MapManyNew(sources, destinations, GetFromModelAToBAsyncFactory(true));

        TestDestinationModelBCollection(destinations);
    }

    [Fact]
    public void MapManyNew_WhenCalledToExistingCollectionWithTaskFuncFactoryAndFactoryReturnsNull_ThrowsException()
    {
        var sources = GetInitialModelACollection().ToList();
        var destinations = new List<TestModelB>();
        var act = () => _mapperService.MapManyNew(sources, destinations, GetNullModelBSyncFactory());

        act.Should().Throw<TenjinXMapperException>();
    }

    [Fact]
    public void MapManyNew_WhenCalledToNewCollectionWithNoFactory_MapsCorrectly()
    {
        var sources = GetInitialModelACollection().ToList();
        var destinations = _mapperService.MapManyNew<TestModelB>(sources);

        TestDestinationModelBCollection(destinations);
    }

    [Fact]
    public void MapManyNew_WhenCalledToNewCollectionWithFuncFactory_MapsCorrectly()
    {
        var sources = GetInitialModelACollection().ToList();
        var destinations = _mapperService.MapManyNew<TestModelB>(sources, GetFromModelAToBFactory(true));

        TestDestinationModelBCollection(destinations);
    }

    [Fact]
    public void MapManyNew_WhenCalledToNewCollectionWithFuncFactoryAndFactoryReturnsNull_ThrowsException()
    {
        var sources = GetInitialModelACollection().ToList();
        var act = () => _mapperService.MapManyNew<TestModelB>(sources, GetNullModelBSyncFactory());

        act.Should().Throw<TenjinXMapperException>();
    }

    [Fact]
    public void MapManyNew_WhenCalledToNewCollectionWithAsyncFuncFactory_MapsCorrectly()
    {
        var sources = GetInitialModelACollection().ToList();
        var destinations = _mapperService.MapManyNew<TestModelB>(sources, GetFromModelAToBAsyncFactory(true));

        TestDestinationModelBCollection(destinations);
    }

    [Fact]
    public void MapManyNew_WhenCalledToNewCollectionWithAsyncFuncFactoryAndFactoryReturnsNull_ThrowsException()
    {
        var sources = GetInitialModelACollection().ToList();
        var act = () => _mapperService.MapManyNew<TestModelB>(sources, GetNullModelBSyncFactory());

        act.Should().Throw<TenjinXMapperException>();
    }

    [Fact]
    public void MapManyNew_WhenSourceIsNullAndCalledToExistingCollectionWithNoFactory_MapsCorrectly()
    {
        var destinations = new List<TestModelB>();

        _mapperService.MapManyNew(null, destinations);

        destinations.Should().BeEmpty();
    }

    [Fact]
    public void MapManyNew_WhenSourceIsNullAndCalledToExistingCollectionWithFuncFactory_MapsCorrectly()
    {
        var destinations = new List<TestModelB>();

        _mapperService.MapManyNew(null, destinations, GetFromModelAToBFactory(true));

        destinations.Should().BeEmpty();
    }

    [Fact]
    public void MapManyNew_WhenSourceIsNullAndCalledToExistingCollectionWithTaskFuncFactory_MapsCorrectly()
    {
        var destinations = new List<TestModelB>();

        _mapperService.MapManyNew(null, destinations, GetFromModelAToBAsyncFactory(true));

        destinations.Should().BeEmpty();
    }

    [Fact]
    public void MapManyNew_WhenSourceIsNullAndCalledToNewCollectionWithNoFactory_MapsCorrectly()
    {
        var destinations = _mapperService.MapManyNew<TestModelB>(null);

        destinations.Should().BeEmpty();
    }

    [Fact]
    public void MapManyNew_WhenSourceIsNullAndCalledToNewCollectionWithFuncFactory_MapsCorrectly()
    {
        var destinations = _mapperService.MapManyNew<TestModelB>(null, GetFromModelAToBFactory(true));

        destinations.Should().BeEmpty();
    }

    [Fact]
    public void MapManyNew_WhenSourceIsNullAndWhenCalledToNewCollectionWithAsyncFuncFactory_MapsCorrectly()
    {
        var destinations = _mapperService.MapManyNew<TestModelB>(null, GetFromModelAToBAsyncFactory(true));

        destinations.Should().BeEmpty();
    }

    private static IEnumerable<TestModelA> GetInitialModelACollection()
    {
        for (var i = 0; i < NumberOfTestItems; i++)
        {
            yield return ModelTestFixtures.StartAFromIndex(i);
        }
    }

    private static void TestDestinationModelBCollection(IEnumerable<TestModelB> destinations)
    {
        var enumeratedDestinations = destinations.ToList();

        for (var i = 0; i < NumberOfTestItems; i++)
        {
            enumeratedDestinations[i].Should().BeEquivalentTo(ModelTestFixtures.FromAToBFromIndex(i));
        }
    }

    private Func<TenjinXMapperNewContext<object>, TestModelB> GetFromModelAToBFactory(bool useContextIndex = false)
    {
        return (context) =>
        {
            _invokedFunction = true;

            if (!useContextIndex)
            {
                context.Index.Should().Be(0);
                context.Source.Should().BeEquivalentTo(ModelTestFixtures.StartA);
            }
            else
            {
                context.Source.Should().BeEquivalentTo(ModelTestFixtures.StartAFromIndex(context.Index));
            }

            context.Context.Should().BeNull();

            return new TestModelB();
        };
    }

    private static Func<TenjinXMapperNewContext<object>, TestModelB> GetNullModelBFactory()
    {
        return (_) => null!;
    }

    private static Func<TenjinXMapperNewContext<object>, Task<TestModelB>> GetNullModelBSyncFactory()
    {
        return async (_) => null!;
    }

    private Func<TenjinXMapperNewContext<object>, Task<TestModelB>> GetFromModelAToBAsyncFactory(bool useContextIndex = false)
    {
        return async (context) =>
        {
            _invokedFunction = true;

            if (!useContextIndex)
            {
                context.Index.Should().Be(0);
                context.Source.Should().BeEquivalentTo(ModelTestFixtures.StartA);
            }
            else
            {
                context.Source.Should().BeEquivalentTo(ModelTestFixtures.StartAFromIndex(context.Index));
            }
            
            context.Context.Should().BeNull();

            return new TestModelB();
        };
    }
}
