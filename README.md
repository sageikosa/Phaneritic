# Phaneritic
Very visible .NET code framework for building services upon.  
Phaneritic rocks include granite and peridotite.  
They are igneous rocks with large crystals (phenocrysts) visible to the unaided eye. 

> Thanks to [Wikipedia](https://en.wikipedia.org/wiki/Phanerite) and `hemlock-qwen2.5-coder-14b` running in **LMStudio** hooked onto **VSCode** for helping "complete" the above description, but not this aside...(¡ that's all me !)

Like an igneous intrusion (or pluton), this code has cooled over time and worked it's way to the surface.  
By publicizing it, I am hoping its structures will be visible to the unaided developer eye, and might provide some utility for others.  
Also, since almost all my code has been hidden under corporate ground for most of my career, this intrusion demonstrates that there have been forces at work which could not be seen.

## Major Feature Areas
The following lists the major features areas in terse summary form.  
The first two I'd previously isolated and made some repositories to explain the basics, they live here in their "native" original form.

- [Primitive obsession avoidance](https://github.com/sageikosa/PrimitivelyStrong)
- Synchronizing access to [critical sections](https://github.com/sageikosa/TaskGateKeeper) in async code
- Database connection injection
- Multiple EF `DbContext` instances in one transaction
- `SqlCommand` invocation also within EF `DbContext` transaction
- Entity to Dto Packing
- Configuration and slow-changing code-available data caching
- Startup seeding for host builder type applications

## Database Connection Injection
Phaneritic supports two database connection types, the main transactional work connection and a logging connection.

## Multiple Interdependent Work Units in One Transaction
Let's assume for a moment that you are like me, and for reasons, you don't want one massive `DbContext` model to rule them all.  
But, you still need to ensure that changes in two (or more) `DbContext`s get committed together.  
```mermaid
graph LR;
Commit-->DbContext1
Commit-->DbContext2
```

---
Or more likely you may have two or more "higher-level" data-management classes that use the same `DbContext` to manipulate data, and you don't need to know about the databases per se.
```mermaid
graph LR;
Commit-->BoxManager
Commit-->CartManager
BoxManager-->DbContext(DbContext Inventory)
CartManager-->DbContext
```

---
Further, you may have a mix of EF updates and SQL SPROC calls that should _really_ commit or fail as a unit.
```mermaid
graph LR;
Commit-->BoxManager
Commit-->RateCounter
BoxManager-->DbContext(DbContext Inventory)
RateCounter-->DbCommands(DbCommands)
```

---
and so forth in increasingly complex connectivity and depth
```mermaid
graph LR;
Commit-->BoxManager
Commit-->CartManager
Commit-->OrderManager
BoxManager-->DbContext(DbContext Inventory)
BoxManager-->RateCounter
CartManager-->DbContext
CartManager-->RateCounter
OrderManager-->RateCounter
OrderManager-->DbContextO(DbContext Orders)
RateCounter-->DbCommands(DbCommands)
```


## Dto Packing
Data transfer objects (Dtos) are representations of entities packed for transport across system boundaries.  
Dtos are defined in the "Interfaces" project/assembly so that they can be imported into implementation and client projects.  
`IPackRecord<TModel,TDto>` is a contract that defines packing operations usable in implementations returning Dtos.  
I have deliberately left out unpacking.  
Conventionally, `FrozenSet<>` and `FrozenDictionary<,>` are used for collections to ensure immutability, necessary for caching scenarios.  
The base interface defines a default `GetFrozenSet` to support packing implementations.

```csharp
public interface IPackRecord<in TModel, TDto>
    where TDto: class, IEquatable<TDto>
{
    [return: NotNullIfNotNull(nameof(model))]
    TDto? Pack(TModel? model);

    /// <summary>Default interface implementation using <see cref="Pack(TModel?)"/></summary>
    public IEnumerable<TDto> GetDtos(IEnumerable<TModel> models)
        => models.Select(x => Pack(x)).OfType<TDto>();

    public FrozenSet<TDto> GetFrozenSet(IEnumerable<TModel>? models)
        => models == null
        ? []
        : models.Select(x => Pack(x)).OfType<TDto>().ToFrozenSet();
}
```

The `Pack` methods are all very similar and follow the same pattern.  
All the Dto packing implementations are hand-cranked, but assisted by Visual Studio's very generous AI auto-complete due to their regularity.  
I looked at using _AutoMapper_, but figured that the time to learn some elaborate tricks and annotate, versus me just pumping these out with auto-complete wasn't worth it at this time.
```csharp
    [return: NotNullIfNotNull(nameof(model))]
    public OptionGroupDto? Pack(OptionGroup? model)
        => model != null
        ? new()
        {
            OptionGroupKey = model.OptionGroupKey,
            Description = model.Description,
            ValidOptionTypeKeys = (model.OptionTypes?.Select(_ot => _ot.OptionTypeKey).ToHashSet() ?? []).ToFrozenSet()
        }
        : null;
```

## Lud Caching
Lookup data (Lud) caching keeps in-memory snapshots of relatively stable lookup data, indexed by key values.  
An `ILudDictionary<TKey, TDto>` can be used in place of a query to the underlying database table sourcing the Dtos in the LudDictionary.  
In the UI this often manifests as lookup stitching and drop-down list entries.  
`BaseLudDictionary<TKey, TLud>` defines the default implementation and is registered as a generic singleton.

Other relevant interfaces include:  
| Interface                         | Use  |
|-----------------------------------|------|
| `ILudCacheable<TKey>`             | Marks a Dto type as cacheable and defines the index key type, used to help ensure valid setup and use |
| `ILudCacheRefresher`              | Refreshes a single `LudDictionary`, typically implemented as a derived class of `LudCacheRefresherBase` |
| `ILudCacheFreshness`              | Manage current local process freshnesses |
| `ILudCacheGetFreshness<TRefresh>` | Get process local freshness for a specific `ILudCacheRefresher` |
| `ILudCacheFreshnessNotify`        | Implement on a class to contribute work on refresh notifications |
| `ILudCacheRefreshAll`             | Refreshes every `LudDictionary` that is missing or out of date; notifies relevant `ILudCacheRefreshNotify` dependencies |
| `ILudCacheUpdate<TRefresh>`       | To be used when the underlying tabular data is changed for a specific `ILudCacheRefresher` |



## Kick Starting Refreshables
