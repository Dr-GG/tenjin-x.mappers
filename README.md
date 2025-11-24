| TenjinX-Mappers | SonarCloud | Badges |
|-|-|-|
|[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=Dr-GG_tenjin-x.mappers&metric=alert_status)](https://sonarcloud.io/summary/overall?id=Dr-GG_tenjin-x.mappers&branch=main)|[![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=Dr-GG_tenjin-x.mappers&metric=reliability_rating)](https://sonarcloud.io/summary/overall?id=Dr-GG_tenjin-x.mappers&branch=main)|[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=Dr-GG_tenjin-x.mappers&metric=coverage)](https://sonarcloud.io/summary/overall?id=Dr-GG_tenjin-x.mappers&branch=main)|
|[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=Dr-GG_tenjin-x.mappers&metric=security_rating)](https://sonarcloud.io/summary/overall?id=Dr-GG_tenjin-x.mappers&branch=main)|[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=Dr-GG_tenjin-x.mappers&metric=sqale_rating)](https://sonarcloud.io/summary/overall?id=Dr-GG_tenjin-x.mappers&branch=main)|[![Vulnerabilities](https://sonarcloud.io/api/project_badges/measure?project=Dr-GG_tenjin-x.mappers&metric=vulnerabilities)](https://sonarcloud.io/summary/overall?id=Dr-GG_tenjin-x.mappers&branch=main)|

# TenjinX-Mappers - A Manual .NET Mapping Library

TenjinX-Mappers is a small, light-weight manual mapping library for .NET. The library provides a simple DI-friendly way to implement and run explicit object-to-object mappers. The library also provides a global runtime entry point responsible for discovering and invoking the mappers.

## What the TenjinX-Mapper Library Provides

- A minimal and explicit mapping interface for small, performant and testable manual mappers.

- DI support for all mapper instances.

- DI support for a common service to discover and use all registered mapper instances.

- A collection of ergonomic extension helpers for common mapping scenarios (`MapNew`, `MapManyNew`, `MapNullable`) while keeping the core mapping simple and predictable.

## Benefits of the TenjinX-Mapper Library

- Predictable, performant and explicit mapping logic - no magic and no code‑generation.

- Small surface area and testability. Each mapper is a class implementation of the `ITenjinXMapper<TSource, TDestination>` interface.

- First class DI support where mappers are registered in the `IServiceCollection` and resolved automatically.

- Useful for explicit mapping with low runtime overhead and easy DI integration.

Use this library when:

- Manual mappings are preferred over reflection-heavy or source-generated frameworks.

- Mappers can be injected separately or used from one common service.

- Require a high run-time throughput.

## TenjinX-Mapper Library Tutorial

### Instal Nuget Package

Install the TenjinX.Mappers nuget package 

```bash
Install-Package TenjinX.Mappers -Version 1.0.0 
```

### Create Manual Mappers

For this example, assume two different classes exist, namely the ``PersonDto`` and ``PersonEntity``. The next step is to create the mapper implementations between these classes.

```cs
using TenjinX.Mappers.Interfaces.Mappers;

// Alternative #1 - Use this approach when one mapper is needed.
public class PersonMapper : ITenjinXMapper<PersonDto, PersonEntity>, ITenjinXMapper<PersonEntity, PersonDto>
{
    // Map from DTO to Entity.
    public ITenjinXMapper<PersonDto, PersonEntity> Map(PersonDto source, PersonEntity target, object? context = null)
    {
        source.Id = target.Id;
        source.FirstName = target.FirstName;
        source.LastName = target.LastName;
        source.DateOfBirth = DateTime.Parse(target.DateOfBirth);

        return this;
    }

    // Map from Entity to DTO.
    public ITenjinXMapper<PersonEntity, PersonDto> Map(PersonEntity source, PersonDto target, object? context = null)
    {
        source.Id = target.Id;
        source.FirstName = target.FirstName;
        source.LastName = target.LastName;
        source.DateOfBirth = target.DateOfBirth.ToString();

        return this;
    }
}

// Alternative #2 - Use this approach when single mappers are needed.
public class PersonDtoToEntityMapper : ITenjinXMapper<PersonDto, PersonEntity>
{
    public void Map(PersonDto source, PersonEntity target)
    {
        source.Id = target.Id;
        source.FirstName = target.FirstName;
        source.LastName = target.LastName;
        source.DateOfBirth = DateTime.Parse(target.DateOfBirth);
    }
}

```

### Add to DI

Add the TenjinX-Mapper implementations to the DI engine using ``IServicesCollection``.

```cs
using TenjinX.Mappers.Enums;
using TenjinX.Mappers.Extensions;

// Alternative #1 - Register the mappers manually.
services.AddSingleton<ITenjinXMapper<PersonDto, PersonEntity>, PersonMapper>();
services.AddSingleton<ITenjinXMapper<PersonEntity, PersonDto>, PersonMapper>();

// Alternative #2 - Register all ITenjinXMapper<,> instances from an assembly.
var assembly = typeof(CustomMapperModels).Assembly; // Get the assembly where the mappers are located.

services.AddTenjinXMappers(assembly, TenjinXMapperScope.Singleton);
```

### Inject the Mappers

One can use the mappers in one of two ways. 

### Use of Mappers Alternative #1 - Use the ITenjinXMapperService - Preferred Way 

```cs
using TenjinX.Mappers.Interfaces.Mappers;

// The service responsible for handling the PersonDto class.
public class PersonService(ITenjinXMapperService mapperService)
{
    private readonly ITenjinXMapperService _mapper = mapperService;

    // Add a new PersonDto to the database.
    public void AddPerson(PersonDto personDto)
    {
        var personEntity = new PersonEntity();

        _mapper.Map(personDto, personEntity);

        personEntity.Save();
    }

    // Get an existing PersonDto from the database using the ID.
    public PersonDto GetPerson(int id)
    {
        var personEntity = new PersonEntity(id);
        var personDto = new PersonDto();

        _mapper.Map(personEntity, personDto);

        return personDto;
    }
}
```

To use the ``ITenjinXMapperService``, add it to the DI engine using:

```cs
using TenjinX.Mappers.Enums;
using TenjinX.Mappers.Extensions;

// Alternative #1 - Add only the service.
services.AddTenjinXMapperService(TenjinXMapperScope.Singleton);

// Alternative #2 - Add all ITenjinXMapper<,> and the ITenjinXMapperService instances.
var assembly = typeof(CustomMapperModels).Assembly;

services.AddTenjinXMappersAndService(assembly, TenjinXMapperScope.Singleton);
```

### Use of Mappers Alternative #2 - Use the ITenjinXMapper interfaces

```cs
using TenjinX.Mappers.Interfaces.Mappers;

// The service responsible for handling the PersonDto class.
public class PersonService(
    ITenjinXMapper<PersonDto, PersonEntity> dtoToEntityMapper,
    ITenjinXMapper<PersonEntity, PersonDto> entityToDtoMapper
)
{
    private readonly ITenjinXMapper<PersonDto, PersonEntity> _dtoToEntityMapper = dtoToEntityMapper;
    private readonly ITenjinXMapper<PersonEntity, PersonDto> _entityToDtoMapper = entityToDtoMapper;

    // Add a new PersonDto to the database.
    public void AddPerson(PersonDto personDto)
    {
        var personEntity = new PersonEntity();

        _dtoToEntityMapper.Map(personDto, personEntity);

        personEntity.Save();
    }

    // Get an existing PersonDto from the database using the ID.
    public PersonDto GetPerson(int id)
    {
        var personEntity = new PersonEntity(id);
        var personDto = new PersonDto();

        _entityToDtoMapper.Map(personEntity, personDto);

        return personDto;
    }
}
```

## Useful Extensions

The TenjinX.Mapper library has a number of extension methods for both ``ITenjinXMapper<,>`` and ``ITenjinXMapperService`` interfaces, to support more convenient and natural mapping operations. Below are the examples that can be used for both the ``ITenjinXMapper<,>`` and ``ITenjinXMapperService`` interfaces:

```cs
using TenjinX.Mappers.Extensions;

// MapNew method
var personDto = new PersonDto(1);

var personEntity1 = mapper.MapNew<PersonEntity>(personDto);
var personEntity2 = mapper.MapNew<PersonEntity>((_) => new PersonEntity(1));
var personEntity3 = mapper.MapNew<PersonEntity>((_) => PersonEntity.LoadAsync(1));

// MapNullable method
PersonDto? nullablePersonDto = null;
PersonEntity? nullablePersonEntity = null;

mapper.MapNullable<PersonEntity>(nullablePersonDto, nullablePersonEntity);

// MapNullableNew method
PersonDto? nullablePersonDto = null;

var nullablePersonEntity1 = mapper.MapNullableNew<PersonEntity>(nullablePersonDto);
var nullablePersonEntity2 = mapper.MapNullableNew<PersonEntity>((_) => new PersonEntity(1));
var nullablePersonEntity3 = mapper.MapNullableNew<PersonEntity>((_) => PersonEntity.LoadAsync(1));

// MapManyNew method: Alternative #1 - Using an existing collection.
var personEntities = new List<PersonEntity>();
var personDtos = 
[
    new PersonDto(1),
    new PersonDto(2),
    new PersonDto(3)
];

mapper.MapManyNew(personDtos, personEntities);
mapper.MapManyNew(personDtos, personEntities, (context) => new PersonEntity(((PersonDto)context.Source).Id));
mapper.MapManyNew(personDtos, personEntities, (context) => PersonEntity.LoadAsync(((PersonDto)context.Source).Id));

// MapManyNew method: Alternative #1 - Creating a new collection collection.
var personDtos = 
[
    new PersonDto(1),
    new PersonDto(2),
    new PersonDto(3)
];

mapper.MapManyNew(personDtos);
mapper.MapManyNew(personDtos, (context) => new PersonEntity(((PersonDto)context.Source).Id));
mapper.MapManyNew(personDtos, (context) => PersonEntity.LoadAsync(((PersonDto)context.Source).Id));

```

## Extra's

The methods and extension method of the ``ITenjinXMapper<,>`` and ``ITenjinXMapperService`` support a number of method overloads:

### Context

Each method and extension method of the ``ITenjinXMapper<,>`` and ``ITenjinXMapperService`` support the pass-through of a parameter called ``object? context``. This parameter can be used if a ``ITenjinXMapper<,>`` needs additional services, context or data to map one object to another.

### IServiceProvider

Each method of the ``ITenjinXMapperService`` supports the pass-through of a parameter called ``IServiceProvider serviceProvider``. This parameter can be used if ``ITenjinXMapper<,>`` instances have been registered on a different DI scope than the ``ITenjinXMapperService`` instance.

### The TenjinXMapperNewContext Class

When using methods of the ``ITenjinXMapper<,>`` and ``ITenjinXMapperService`` that use factories or map collections, each callback is provided with the ``TenjinXMapperNewContext<TSource>`` class. The ``TenjinXMapperNewContext<TSource>`` provides data or context to create the new target object being mapped to:

* **Index**: The zero-based index of the current item being mapped from in a collection. This parameter is only populated when mapping from a collection.

* **Source**: The object being mapped from.

* **Content**: The original ``object? context`` parameter that was passed through.

### DI Scopes

When using the ``AddTenjinXMappers``, ``AddTenjinXMapperService`` and ``AddTenjinXMappersAndService`` extension methods, one can specify the DI scopes (singleton, scoped or transient). By default, the scope is set to singleton.

***For the most optimal performance, add all TenjinX.Mapper instances as singletons.***

### Preloading Assemblies

When using the ``AddTenjinXMappers``, ``AddTenjinXMapperService`` and ``AddTenjinXMappersAndService`` extension methods, one can set the flag ``preloadAssembly``. By default, the ``preloadAssembly`` is set to ``true``. 

The ``ITenjinXMapperService`` caches reflection data of ``ITenjinXMapper<,>`` instances. Thus, the ``ITenjinXMapperService`` instance can pre-load caching data of ``ITenjinXMapper<,>`` instances from the assemblies that were marked to be pre-loaded.

The ``ITenjinXMapperService`` does support on the fly caching and does not require pre-loading. However, when registering the ``ITenjinXMapperService`` as a singleton, this can greatly improve performance.

The extension method ``AddTenjinXMapperPreloadAssemblies`` can be used to mark specific assemblies to be pre-loaded into ``ITenjinXMapperService``.

***Using pre-loaded assemblies with scoped or transient DI modes can cause significant performance degradation, because each instance creation of the ``ITenjinXMapperService`` results in a pre-loading operation. Only use assembly pre-loading when ``ITenjinXMapperService`` is a singleton.***

