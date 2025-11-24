using TenjinX.Mappers.Interfaces.Mappers;
using TenjinX.Mappers.Extensions;
using TenjinX.Mappers.Tests.Implementations;
using TenjinX.Mappers.Tests.TestFixtures;
using TenjinX.Mappers.Tests.TestModels;
using FluentAssertions;
using TenjinX.Mappers.Models;

namespace TenjinX.Mappers.Tests.ExtensionsTests;

public class TenjinXMapperExtensionsTests
{
    private const int NumberOfTestItems = 100;

    private bool _invokedFunction;

    private readonly ITenjinXMapper<TestModelA, TestModelB> _aToBMapper;

    public TenjinXMapperExtensionsTests()
    {
        _aToBMapper = new AToBMapper();
    }

    [Fact]
    public void MapNew_WhenCalledWithNoFactory_MapsCorrectly()
    {
        var result = _aToBMapper.MapNew(ModelTestFixtures.StartA);

        result.Should().BeEquivalentTo(ModelTestFixtures.FromAToB);
        _invokedFunction.Should().BeFalse();
    }

    [Fact]
    public void MapNew_WhenCalledWithFuncFactory_MapsCorrectly()
    {
        var result = _aToBMapper.MapNew(ModelTestFixtures.StartA, GetFromModelAToBFactory());

        result.Should().BeEquivalentTo(ModelTestFixtures.FromAToB);
        _invokedFunction.Should().BeTrue();
    }

    [Fact]
    public void MapNew_WhenCalledWithTaskFunc_MapsCorrectly()
    {
        var result = _aToBMapper.MapNew(ModelTestFixtures.StartA, GetFromModelAToBAsyncFactory());

        result.Should().BeEquivalentTo(ModelTestFixtures.FromAToB);
        _invokedFunction.Should().BeTrue();
    }

    [Fact]
    public void MapNullable_WhenSourceIsNull_DoesNotInvokeMapper()
    {
        var testModelB = new TestModelB();
        var originalB = testModelB with { };

        _aToBMapper.MapNullable(null, testModelB);

        _invokedFunction.Should().BeFalse();
        testModelB.Should().BeEquivalentTo(originalB);
    }

    [Fact]
    public void MapNullable_WhenDestinationIsNull_DoesNotInvokeMapper()
    {
        _aToBMapper.MapNullable(ModelTestFixtures.StartA, null);

        _invokedFunction.Should().BeFalse();
    }

    [Fact]
    public void MapNullable_WhenTheSourceAndDestinationAreNotNull_InvokesMapper()
    {
        var testModelB = new TestModelB();

        _aToBMapper.MapNullable(ModelTestFixtures.StartA, testModelB);

        _invokedFunction.Should().BeFalse();
        testModelB.Should().BeEquivalentTo(ModelTestFixtures.FromAToB);
    }

    [Fact]
    public void MapNullableNew_WhenSourceIsNull_ReturnsNull()
    {
        var result = _aToBMapper.MapNullableNew(null);

        _invokedFunction.Should().BeFalse();
        result.Should().BeNull();
    }

    [Fact]
    public void MapNullableNew_WhenSourceIsNotNullAndNoFactoryIsUsed_InvokesMapper()
    {
        var result = _aToBMapper.MapNullableNew(ModelTestFixtures.StartA);

        _invokedFunction.Should().BeFalse();
        result.Should().BeEquivalentTo(ModelTestFixtures.FromAToB);
    }

    [Fact]
    public void MapNullableNew_WhenSourceIsNotNullAndFuncFactoryIsUsed_InvokesMapper()
    {
        var result = _aToBMapper.MapNullableNew(ModelTestFixtures.StartA, GetFromModelAToBFactory());

        _invokedFunction.Should().BeTrue();
        result.Should().BeEquivalentTo(ModelTestFixtures.FromAToB);
    }

    [Fact]
    public void MapNullableNew_WhenSourceIsNotNullAndTaskFuncFactoryIsUsed_InvokesMapper()
    {
        var result = _aToBMapper.MapNullableNew(ModelTestFixtures.StartA, GetFromModelAToBAsyncFactory());

        _invokedFunction.Should().BeTrue();
        result.Should().BeEquivalentTo(ModelTestFixtures.FromAToB);
    }

    [Fact]
    public void MapManyNew_WhenCalledToExistingCollectionWithNoFactory_MapsCorrectly()
    {
        var sources = GetInitialModelACollection().ToList();
        var destinations = new List<TestModelB>();

        _aToBMapper.MapManyNew(sources, destinations);

        TestDestinationModelBCollection(destinations);
    }

    [Fact]
    public void MapManyNew_WhenCalledToExistingCollectionWithFuncFactory_MapsCorrectly()
    {
        var sources = GetInitialModelACollection().ToList();
        var destinations = new List<TestModelB>();

        _aToBMapper.MapManyNew(sources, destinations, GetFromModelAToBFactory(true));

        TestDestinationModelBCollection(destinations);
    }

    [Fact]
    public void MapManyNew_WhenCalledToExistingCollectionWithTaskFuncFactory_MapsCorrectly()
    {
        var sources = GetInitialModelACollection().ToList();
        var destinations = new List<TestModelB>();

        _aToBMapper.MapManyNew(sources, destinations, GetFromModelAToBAsyncFactory(true));

        TestDestinationModelBCollection(destinations);
    }

    [Fact]
    public void MapManyNew_WhenCalledToNewCollectionWithNoFactory_MapsCorrectly()
    {
        var sources = GetInitialModelACollection().ToList();
        var destinations = _aToBMapper.MapManyNew(sources);

        TestDestinationModelBCollection(destinations);
    }

    [Fact]
    public void MapManyNew_WhenCalledToNewCollectionWithFuncFactory_MapsCorrectly()
    {
        var sources = GetInitialModelACollection().ToList();
        var destinations = _aToBMapper.MapManyNew(sources, GetFromModelAToBFactory(true));

        TestDestinationModelBCollection(destinations);
    }

    [Fact]
    public void MapManyNew_WhenCalledToNewCollectionWithAsyncFuncFactory_MapsCorrectly()
    {
        var sources = GetInitialModelACollection().ToList();
        var destinations = _aToBMapper.MapManyNew(sources, GetFromModelAToBAsyncFactory(true));

        TestDestinationModelBCollection(destinations);
    }

    [Fact]
    public void MapManyNew_WhenSourceIsNullAndCalledToExistingCollectionWithNoFactory_MapsCorrectly()
    {
        var destinations = new List<TestModelB>();

        _aToBMapper.MapManyNew(null, destinations);

        destinations.Should().BeEmpty();
    }

    [Fact]
    public void MapManyNew_WhenSourceIsNullAndCalledToExistingCollectionWithFuncFactory_MapsCorrectly()
    {
        var destinations = new List<TestModelB>();

        _aToBMapper.MapManyNew(null, destinations, GetFromModelAToBFactory(true));

        destinations.Should().BeEmpty();
    }

    [Fact]
    public void MapManyNew_WhenSourceIsNullAndCalledToExistingCollectionWithTaskFuncFactory_MapsCorrectly()
    {
        var destinations = new List<TestModelB>();

        _aToBMapper.MapManyNew(null, destinations, GetFromModelAToBAsyncFactory(true));

        destinations.Should().BeEmpty();
    }

    [Fact]
    public void MapManyNew_WhenSourceIsNullAndCalledToNewCollectionWithNoFactory_MapsCorrectly()
    {
        var destinations = _aToBMapper.MapManyNew(null);

        destinations.Should().BeEmpty();
    }

    [Fact]
    public void MapManyNew_WhenSourceIsNullAndCalledToNewCollectionWithFuncFactory_MapsCorrectly()
    {
        var destinations = _aToBMapper.MapManyNew(null, GetFromModelAToBFactory(true));

        destinations.Should().BeEmpty();
    }

    [Fact]
    public void MapManyNew_WhenSourceIsNullAndWhenCalledToNewCollectionWithAsyncFuncFactory_MapsCorrectly()
    {
        var destinations = _aToBMapper.MapManyNew(null, GetFromModelAToBAsyncFactory(true));

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

    private Func<TenjinXMapperNewContext<TestModelA>, TestModelB> GetFromModelAToBFactory(bool useContextIndex = false)
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

    private Func<TenjinXMapperNewContext<TestModelA>, Task<TestModelB>> GetFromModelAToBAsyncFactory(bool useContextIndex = false)
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
