using FluentAssertions;
using Moq;
using TenjinX.Mappers.Extensions;
using TenjinX.Mappers.Interfaces.Mappers;
using TenjinX.Mappers.Moq.Extensions;
using TenjinX.Mappers.Moq.Models.Collection;
using TenjinX.Mappers.Moq.Tests.Fixtures;
using TenjinX.Mappers.Moq.Tests.Models;

namespace TenjinX.Mappers.Moq.Tests.ExtensionsTests;

public class TenjinXMapperMoqExtensionsTests
{
    private const int ManyIterations = 100;

    private readonly Mock<ITenjinXMapper<TestSourceModel, TestDestinationModel>> _mockMapper = new();

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void SetupMapAndVerify_WithDestinationParameterAndNoCallback_ShouldMapToProvidedDestination(bool verifyWithDestination)
    {
        var providedDestination = TestModelTestFixtures.RandomTestDestinationModel;
        var incomingDestination = TestModelTestFixtures.EmptyTestDestinationModel;

        _mockMapper.SetupMap(providedDestination);
        _mockMapper.Object.Map(TestModelTestFixtures.EmptyTestSourceModel, incomingDestination);

        if (verifyWithDestination)
        {
            _mockMapper.VerifyMap(Times.Once(), destination: incomingDestination);
        }
        else
        {
            _mockMapper.VerifyMap(Times.Once());
        }

        providedDestination.Should().NotBeSameAs(incomingDestination);
        providedDestination.Should().BeEquivalentTo(incomingDestination);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void SetupMapAndVerify_WithSourceAndDestination_ShouldMapToProvidedDestination(bool verifyWithDestination)
    {
        var source = TestModelTestFixtures.RandomTestSourceModel;
        var providedDestination = TestModelTestFixtures.RandomTestDestinationModel;
        var incomingDestination = TestModelTestFixtures.EmptyTestDestinationModel;

        _mockMapper.SetupMap(providedDestination, source: source);
        _mockMapper.Object.Map(source, incomingDestination);

        if (verifyWithDestination)
        {
            _mockMapper.VerifyMap(Times.Once(), source: source, destination: incomingDestination);
        }
        else
        {
            _mockMapper.VerifyMap(Times.Once(), source: source);
        }

        providedDestination.Should().NotBeSameAs(incomingDestination);
        providedDestination.Should().BeEquivalentTo(incomingDestination);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void SetupMapAndVerify_WithAllParametersAndCallback_ShouldInvokeCallbackAndMapToProvidedDestination(bool verifyWithDestination)
    {
        var source = TestModelTestFixtures.RandomTestSourceModel;
        var providedDestination = TestModelTestFixtures.RandomTestDestinationModel;
        var incomingDestination = TestModelTestFixtures.EmptyTestDestinationModel;

        _mockMapper.SetupMap
        (
            providedDestination,
            source: source,
            destination: incomingDestination,
            callback: (context) =>
            {
                context.IncomingDestination!.Property01 = "CallbackProperty01";
                context.IncomingDestination.Property02 = "CallbackProperty02";
            }
        );

        _mockMapper.Object.Map(source, incomingDestination);

        if (verifyWithDestination)
        {
            _mockMapper.VerifyMap(Times.Once(), source: source, destination: incomingDestination);
        }
        else
        {
            _mockMapper.VerifyMap(Times.Once(), source: source);
        }

        providedDestination.Should().NotBeSameAs(incomingDestination);
        incomingDestination.Property01.Should().Be("CallbackProperty01");
        incomingDestination.Property02.Should().Be("CallbackProperty02");
    }


    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void SetupMapAndVerify_WithMapNew_ShouldCreateNewDestinationObject(bool verifyWithDestination)
    {
        var destination = new TestDestinationModel();

        _mockMapper.SetupMapNew();
        _mockMapper.Object.MapNew(TestModelTestFixtures.EmptyTestSourceModel);

        if (verifyWithDestination)
        {
            _mockMapper.VerifyMap(Times.Once(), destination: destination);
        }
        else
        {
            _mockMapper.VerifyMap(Times.Once());
        }
    }

    [Fact]
    public void SetupMap_WhenUsingMapNullable_WorksAsExpected()
    {
        var source1 = TestModelTestFixtures.RandomTestSourceModel;
        var source2 = TestModelTestFixtures.RandomTestSourceModel;
        var incomingDestination1 = TestModelTestFixtures.EmptyTestDestinationModel;
        var incomingDestination2 = TestModelTestFixtures.EmptyTestDestinationModel;
        var incomingDestination3 = TestModelTestFixtures.EmptyTestDestinationModel;
        var providedDestination1 = TestModelTestFixtures.RandomTestDestinationModel;
        var providedDestination2 = TestModelTestFixtures.RandomTestDestinationModel;
        var providedDestination3 = TestModelTestFixtures.RandomTestDestinationModel;

        _mockMapper
            .SetupMap(providedDestination1, source: source1)
            .SetupMap(providedDestination2, source: source2);

        _mockMapper.Object
            .MapNullable(source1, incomingDestination1)
            .MapNullable(source2, incomingDestination2)
            .MapNullable(null, incomingDestination3)
            .MapNullable(source1, null);

        _mockMapper
            .VerifyMap(Times.Once(), source: source1)
            .VerifyMap(Times.Once(), source: source2)
            .VerifyMap(Times.Never(), destination: incomingDestination3);

        providedDestination1.Should().BeEquivalentTo(incomingDestination1);
        providedDestination2.Should().BeEquivalentTo(incomingDestination2);
    }

    [Fact]
    public void SetupMap_WhenUsingMapNew_WorksAsExpected()
    {
        var source1 = TestModelTestFixtures.RandomTestSourceModel;
        var source2 = TestModelTestFixtures.RandomTestSourceModel;
        var providedDestination1 = TestModelTestFixtures.RandomTestDestinationModel;
        var providedDestination2 = TestModelTestFixtures.RandomTestDestinationModel;

        _mockMapper
            .SetupMap(providedDestination1, source: source1)
            .SetupMap(providedDestination2, source: source2);

        var result1 = _mockMapper.Object.MapNew(source1);
        var result2 = _mockMapper.Object.MapNew(source2);

        _mockMapper
            .VerifyMap(Times.Once(), source: source1)
            .VerifyMap(Times.Once(), source: source2);

        providedDestination1.Should().BeEquivalentTo(result1);
        providedDestination2.Should().BeEquivalentTo(result2);
    }

    [Fact]
    public void SetupMap_WhenUsingMapNullableNew_WorksAsExpected()
    {
        var source1 = TestModelTestFixtures.RandomTestSourceModel;
        var source2 = TestModelTestFixtures.RandomTestSourceModel;
        var providedDestination1 = TestModelTestFixtures.RandomTestDestinationModel;
        var providedDestination2 = TestModelTestFixtures.RandomTestDestinationModel;

        _mockMapper
            .SetupMap(providedDestination1, source: source1)
            .SetupMap(providedDestination2, source: source2);

        var result1 = _mockMapper.Object.MapNullableNew(source1);
        var result2 = _mockMapper.Object.MapNullableNew(source2);
        var result3 = _mockMapper.Object.MapNullableNew(null);

        _mockMapper
            .VerifyMap(Times.Once(), source: source1)
            .VerifyMap(Times.Once(), source: source2);

        providedDestination1.Should().BeEquivalentTo(result1);
        providedDestination2.Should().BeEquivalentTo(result2);
        result3.Should().BeNull();
    }

    [Fact]
    public void SetupMapManyNew_WhenUsingMapManyToAnExistingCollection_WorksAsExpected()
    {
        var mocks = new List<TenjinXMapperMockMany<TestSourceModel, TestDestinationModel>>();

        for (var i = 0; i < ManyIterations; i++)
        {
            var mock = new TenjinXMapperMockMany<TestSourceModel, TestDestinationModel>
            {
                Source = TestModelTestFixtures.RandomTestSourceModel,
                ProvidedDestination = TestModelTestFixtures.RandomTestDestinationModel
            };

            mocks.Add(mock);
        }

        _mockMapper.SetupMapManyNew([.. mocks]);

        var sources = mocks.Select(x => x.Source);
        var results = new List<TestDestinationModel>();

        _mockMapper.Object!.MapManyNew(sources, results);

        for (var i = 0; i < ManyIterations; i++)
        {
            var expected = mocks[i].ProvidedDestination;
            var actual = results[i];

            expected.Should().BeEquivalentTo(actual);

            _mockMapper.VerifyMap(Times.Once(), source: mocks[i].Source);
        }

        _mockMapper.VerifyMapMany(Times.Once(), [.. mocks]);
    }

    [Fact]
    public void SetupMapManyNew_WhenUsingMapManyToNewCollection_WorksAsExpected()
    {
        var mocks = new List<TenjinXMapperMockMany<TestSourceModel, TestDestinationModel>>();

        for (var i = 0; i < ManyIterations; i++)
        {
            var mock = new TenjinXMapperMockMany<TestSourceModel, TestDestinationModel>
            {
                Source = TestModelTestFixtures.RandomTestSourceModel,
                ProvidedDestination = TestModelTestFixtures.RandomTestDestinationModel
            };

            mocks.Add(mock);
        }

        _mockMapper.SetupMapManyNew([.. mocks]);

        var sources = mocks.Select(x => x.Source);
        var results = _mockMapper.Object!.MapManyNew(sources).ToList();

        for (var i = 0; i < ManyIterations; i++)
        {
            var expected = mocks[i].ProvidedDestination;
            var actual = results[i];

            expected.Should().BeEquivalentTo(actual);

            _mockMapper.VerifyMap(Times.Once(), source: mocks[i].Source);
        }

        _mockMapper.VerifyMapMany(Times.Once(), [.. mocks]);
    }
}
