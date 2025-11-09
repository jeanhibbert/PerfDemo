# Performance Benchmarks Documentation

This project contains comprehensive benchmarks for common .NET performance scenarios using BenchmarkDotNet.

## Benchmark Classes

### 1. DictionaryBenchmarks
Tests `Dictionary<string, int?>` operations:
- **Add operations**: With and without capacity pre-allocation
- **Retrieval**: Direct access vs TryGetValue
- **Search**: ContainsKey for existing and non-existing keys
- **Iteration**: Foreach enumeration with nullable handling
- **Updates**: Modifying existing entries
- **Mixed operations**: Add or update patterns

**Key Insights:**
- Pre-allocating capacity significantly improves add performance
- TryGetValue is safer than direct indexer access
- ContainsKey + indexer is slower than TryGetValue

---

### 2. StringBenchmarks
Compares different string building approaches:
- **String concatenation**: `+` operator and `+=`
- **StringBuilder**: Default and with capacity
- **String interpolation**: `$"{a}{b}"`
- **String.Concat**: Direct method call
- **String.Join**: Array-based joining
- **String.Create**: Modern span-based approach

**Key Insights:**
- StringBuilder with capacity is fastest for many concatenations
- String.Create is most efficient for known-length scenarios
- Simple `+` creates many intermediate objects (avoid in loops)

---

### 3. LinqVsLoopBenchmarks
Compares LINQ methods vs traditional loops:
- **Filtering**: Where vs for/foreach
- **Transformation**: Select vs loops
- **Aggregation**: Sum, Count
- **Search**: Any, First
- **Complex queries**: Multiple operations

**Key Insights:**
- For loops are generally faster but less readable
- LINQ has overhead but offers better expressiveness
- For hot paths, consider loops; for readability, use LINQ
- Early termination (Any, First) benefits from short-circuit evaluation

---

### 4. SpanVsArrayBenchmarks
Demonstrates `Span<T>` and `Memory<T>` advantages:
- **Copying**: Array.Copy vs Span.CopyTo
- **Slicing**: ArraySegment vs Span vs LINQ
- **Reversing**: Array.Reverse vs Span.Reverse
- **Parsing**: Substring vs Span-based parsing
- **Filling**: Loop vs Span.Fill
- **Searching**: IndexOf implementations
- **Stack allocation**: Heap vs stackalloc

**Key Insights:**
- Span operations avoid allocations
- stackalloc is extremely fast for small arrays
- Span.Slice is zero-cost abstraction
- Great for parsing and text processing

---

### 5. FrozenCollectionBenchmarks (.NET 8+)
Compares read-optimized collections:
- **FrozenDictionary** vs Dictionary vs ImmutableDictionary
- **FrozenSet** vs HashSet vs ImmutableHashSet
- **Lookup performance**
- **Iteration performance**
- **Contains operations**

**Key Insights:**
- FrozenDictionary is fastest for read-heavy workloads
- Create once, read many times
- Perfect for configuration, lookup tables, caching
- ImmutableCollections have higher overhead but thread-safe

---

### 6. CollectionTypesBenchmarks
Comprehensive collection comparison:
- **List vs LinkedList vs HashSet**
- **Queue vs Stack**
- **SortedSet performance**
- **Dictionary vs SortedDictionary vs ConcurrentDictionary**
- **ConcurrentBag for thread-safe scenarios**
- **Add, iterate, search, and remove operations**

**Key Insights:**
- List is best general-purpose collection
- Pre-allocate capacity when size is known
- HashSet for fast lookups/uniqueness
- LinkedList for frequent insertions/deletions
- Use concurrent collections only when needed

---

## Running the Benchmarks

### Option 1: Interactive Menu
```bash
dotnet run -c Release --project PerfDemo/PerfDemo.csproj
```
Then select which benchmark to run from the menu.

### Option 2: Run Specific Benchmark
Modify `Program.cs` to run only the desired benchmark class.

### Option 3: Run All Benchmarks
Select option 7 from the menu or press Enter without input.

---

## Important Notes

### Always Run in Release Mode
```bash
dotnet run -c Release
```
Debug mode includes additional checks that skew results.

### Understanding Results
- **Mean**: Average execution time
- **Error**: Half of 99.9% confidence interval
- **StdDev**: Standard deviation
- **Gen0/Gen1/Gen2**: Garbage collection counts per 1000 operations
- **Allocated**: Memory allocated per operation
- **Rank**: Performance ranking (1 = fastest)

### Best Practices
1. **Warm-up**: BenchmarkDotNet automatically handles warm-up
2. **Iterations**: Multiple runs ensure statistical significance
3. **Isolation**: Each benchmark runs in a separate process
4. **System State**: Close unnecessary applications during benchmarking

---

## Performance Tips Summary

### Memory Allocation
- Pre-allocate collections with known sizes
- Use `Span<T>` for stack-allocated buffers
- Prefer `stackalloc` for small temporary arrays
- Avoid allocations in hot paths

### String Operations
- Use `StringBuilder` for multiple concatenations
- Use `String.Create` for formatted output
- Consider `Span<char>` for text parsing
- Avoid substring allocations with spans

### Collections
- Use appropriate collection for the use case
- `List<T>` for general sequential access
- `HashSet<T>` for uniqueness and fast lookups
- `Dictionary<TKey, TValue>` for key-value pairs
- `FrozenDictionary` for read-heavy scenarios
- Pre-size collections when count is known

### Loops vs LINQ
- Use loops for performance-critical code
- LINQ is fine for non-hot paths
- Avoid LINQ on hot paths unless profiling shows no impact
- Consider foreach vs for based on scenario

### Advanced Techniques
- Use `Span<T>` and `Memory<T>` for zero-allocation slicing
- Leverage frozen collections in .NET 8+
- Profile before optimizing
- Measure real-world impact, not just microbenchmarks

---

## Additional Resources

- [BenchmarkDotNet Documentation](https://benchmarkdotnet.org/)
- [Stephen Toub's Performance Blog Posts](https://devblogs.microsoft.com/dotnet/author/stephen-toub/)
- [.NET Performance Repository](https://github.com/dotnet/performance)
- [Frozen Collections in .NET 8](https://devblogs.microsoft.com/dotnet/performance-improvements-in-net-8/#frozen-collections)
