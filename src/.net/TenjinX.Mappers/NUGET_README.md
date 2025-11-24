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