# C# Code Optimization - Performance Talk
## PowerPoint Slide Deck Content

---

## Slide 1: Title Slide
**Title:** C# Code Optimization: Tips and Tricks for High Performance

**Subtitle:** Mastering .NET 8 Performance Best Practices

**Your Name**
**Date**

---

## Slide 2: Agenda
### Today's Journey

1. Diagnosing Performance Issues
2. General Do's and Don'ts
3. String Operations & Span<T>
4. LINQ vs Traditional Loops
5. Collection Performance
6. Frozen Collections (.NET 8)
7. Emergency Optimizations
8. Live Benchmarks Demo

---

## Slide 3: Section Title - Diagnosing Performance
# Part 1: Finding the Problem
### "You can't optimize what you can't measure"

---

## Slide 4: Diagnostic Tools
### Essential Tools for Performance Analysis

**Visual Studio Profiler**
- CPU Usage profiling
- Memory profiling
- Identify hot paths
- Find allocation bottlenecks

**dotnet-trace & dotnet-counters**
- Command-line diagnostics
- Production-ready tools
- Low overhead monitoring

**BenchmarkDotNet**
- Microbenchmarking
- Statistical analysis
- Memory diagnostics

---

## Slide 5: Understanding the Garbage Collector
### The GC: Your Friend and Foe

**Three Generations:**
- **Gen 0**: Short-lived objects (frequent collections)
- **Gen 1**: Mid-term objects (buffer between Gen 0 & 2)
- **Gen 2**: Long-lived objects (expensive collections)

**Key Metrics:**
- Gen 0/1/2 collection counts
- Allocation rate
- GC pause time

**Goal:** Minimize allocations, avoid Gen 2 collections

---

## Slide 6: Memory Analysis Workflow
### Using dotnet Memory Collector

```bash
# Capture memory dump
dotnet-gcdump collect -p [PID]

# Analyze with PerfView or Visual Studio
```

**What to look for:**
- Large object heap (LOH) allocations
- String duplicates
- Unexpectedly large collections
- Memory leaks

**Pro Tip:** Use AI tools to analyze CSV exports for patterns

---

## Slide 7: Section Title - Best Practices
# Part 2: General Do's and Don'ts
### Building Performance-First Code

---

## Slide 8: Logging Best Practices (C# 7+)
### Use Structured Logging

? **Don't:**
```csharp
logger.LogInformation($"User {userId} logged in at {timestamp}");
```
*Allocates string even if logging is disabled*

? **Do:**
```csharp
logger.LogInformation("User {UserId} logged in at {Timestamp}", 
    userId, timestamp);
```
*Zero allocation when logging level is disabled*

**Source Generators (C# 10+):**
```csharp
[LoggerMessage(Level = LogLevel.Information, 
    Message = "User {UserId} logged in")]
partial void LogUserLogin(int userId);
```

---

## Slide 9: Never Throw Exceptions for Control Flow
### Exceptions Are Expensive

? **Don't:**
```csharp
try {
    return int.Parse(input);
} catch {
    return -1;
}
```

? **Do:**
```csharp
if (int.TryParse(input, out var result))
    return result;
return -1;
```

**Or use the Result Pattern:**
```csharp
public record Result<T>(bool Success, T? Value, string? Error);
```

**Why?** Exceptions involve stack unwinding, very slow!

---

## Slide 10: Keep Code Self-Documenting
### Readability vs Performance

**The Balance:**
- Write clear, maintainable code first
- Profile to find actual bottlenecks
- Optimize only the hot path
- Document why optimizations exist

```csharp
// Clear intent, compiler may optimize anyway
var evenNumbers = numbers.Where(n => n % 2 == 0).ToList();

// Optimized hot path - add comment explaining why
// Hot path: Process 1M+ items per second, LINQ too slow
var evenNumbers = new List<int>(numbers.Count / 2);
for (int i = 0; i < numbers.Count; i++)
    if (numbers[i] % 2 == 0)
        evenNumbers.Add(numbers[i]);
```

---

## Slide 11: Know Your Enemy: I/O
### I/O is Always the Bottleneck

**Database Access:**
- Use async/await consistently
- Batch operations when possible
- Consider caching (Redis, Memory Cache)
- Use pagination (Skip/Take)
- Index your queries!

**File I/O:**
- Use buffered streams
- Async file operations
- Consider memory-mapped files for large files

**Network I/O:**
- Connection pooling
- Keep-alive connections
- HTTP/2 or gRPC for APIs

---

## Slide 12: Section Title - String Performance
# Part 3: Working with Strings
### Strings are Immutable - Plan Accordingly

---

## Slide 13: String Concatenation Performance
### Benchmark Results Comparison

| Method | Mean | Allocated |
|--------|------|-----------|
| String + operator | 50 ?s | 5 KB |
| String += | 45 ?s | 4.8 KB |
| StringBuilder | 2 ?s | 1 KB |
| StringBuilder (capacity) | **0.8 ?s** | **512 B** |
| String.Create | **0.7 ?s** | **512 B** |

**Takeaway:** Use StringBuilder with capacity for multiple concatenations

---

## Slide 14: String Operations - Best Practices
### Choose the Right Tool

**Few concatenations (< 5):**
```csharp
var result = $"{firstName} {lastName}"; // Fine!
```

**Many concatenations in loop:**
```csharp
var sb = new StringBuilder(capacity: estimatedSize);
foreach (var item in items)
    sb.Append(item);
return sb.ToString();
```

**Known format with spans:**
```csharp
return string.Create(totalLength, data, (span, state) => {
    // Write directly to span - zero intermediate allocations
});
```

---

## Slide 15: Introducing Span<T>
### Zero-Allocation Memory Slicing

**What is Span<T>?**
- Stack-only type (ref struct)
- Window into contiguous memory
- No heap allocations
- Blazing fast

**Use Cases:**
- String parsing
- Array slicing
- Buffer manipulation
- Text processing

---

## Slide 16: Span<T> Examples
### Real-World Performance Wins

**String parsing:**
```csharp
? string sub = text.Substring(1, 3);  // Allocates new string
? ReadOnlySpan<char> span = text.AsSpan(1, 3);  // Zero allocation
```

**Array slicing:**
```csharp
? var slice = array.Skip(10).Take(20).ToArray();  // Multiple allocations
? var slice = array.AsSpan().Slice(10, 20);  // Zero allocation
```

**Stack allocation (small buffers):**
```csharp
Span<int> buffer = stackalloc int[100];  // On stack, not heap!
```

**Performance:** 10-100x faster, zero allocations!

---

## Slide 17: Section Title - LINQ Performance
# Part 4: LINQ vs Loops
### When to Use Each

---

## Slide 18: LINQ Performance Characteristics
### The Trade-offs

**LINQ Advantages:**
- Expressive, readable code
- Composable operations
- Less boilerplate
- Easier to maintain

**LINQ Disadvantages:**
- Allocates enumerators
- Virtual method calls
- Overhead per operation
- Harder to optimize by compiler

**Rule of Thumb:** LINQ for readability, loops for hot paths

---

## Slide 19: LINQ vs Loop Benchmarks
### Real Performance Numbers

| Operation | LINQ | For Loop | Difference |
|-----------|------|----------|------------|
| Filter (Where) | 15 ?s | 8 ?s | 1.9x |
| Transform (Select) | 18 ?s | 9 ?s | 2x |
| Sum | 12 ?s | 4 ?s | 3x |
| Complex query | 45 ?s | 18 ?s | 2.5x |
| First (found early) | 2 ?s | 0.5 ?s | 4x |

**Allocations:** LINQ always allocates more (enumerators, closures)

---

## Slide 20: When to Use LINQ vs Loops
### Decision Matrix

**Use LINQ when:**
- Code readability is priority
- Not in a hot path
- Prototyping/development speed matters
- Business logic layer

**Use Loops when:**
- Inside loops (nested iterations)
- Called millions of times
- Performance profiling shows bottleneck
- Hot path identified
- Real-time/gaming scenarios

**Pro Tip:** Start with LINQ, optimize to loops if profiling shows it matters

---

## Slide 21: Section Title - Collections
# Part 5: Collection Performance
### Choosing the Right Data Structure

---

## Slide 22: Collection Types Overview
### Pick the Right Tool

| Collection | Best For | Access | Add/Remove |
|------------|----------|--------|------------|
| `List<T>` | Sequential access | O(1) | O(1) end, O(n) middle |
| `LinkedList<T>` | Frequent insert/delete | O(n) | O(1) |
| `HashSet<T>` | Uniqueness, lookups | O(1) | O(1) |
| `Dictionary<K,V>` | Key-value pairs | O(1) | O(1) |
| `SortedSet<T>` | Sorted data | O(log n) | O(log n) |
| `Queue<T>` | FIFO operations | O(1) | O(1) |
| `Stack<T>` | LIFO operations | O(1) | O(1) |

---

## Slide 23: Pre-Allocate Capacity!
### The Easiest Win

**Without capacity:**
```csharp
var list = new List<int>();
for (int i = 0; i < 1000; i++)
    list.Add(i);
// Multiple internal array resizes and copies!
```

**With capacity:**
```csharp
var list = new List<int>(1000);
for (int i = 0; i < 1000; i++)
    list.Add(i);
// Single allocation, no resizing!
```

**Performance:** 2-3x faster, 50% less memory allocated

**Apply to:** List, Dictionary, HashSet, StringBuilder, etc.

---

## Slide 24: Dictionary Performance Tips
### Optimize Key-Value Operations

**TryGetValue over ContainsKey + indexer:**
```csharp
? if (dict.ContainsKey(key))
    var value = dict[key];  // Two lookups!

? if (dict.TryGetValue(key, out var value))
  // Use value  // One lookup!
```

**CollectionsMarshal for zero-allocation access (.NET 5+):**
```csharp
ref var value = ref CollectionsMarshal.GetValueRefOrNullRef(dict, key);
if (!Unsafe.IsNullRef(ref value))
    value++; // Modify in-place, zero allocation!
```

---

## Slide 25: Section Title - Frozen Collections
# Part 6: Frozen Collections (.NET 8)
### Read-Optimized Immutability

---

## Slide 26: What are Frozen Collections?
### The New Performance Champions

**Introduced in .NET 8:**
- `FrozenDictionary<K, V>`
- `FrozenSet<T>`

**Characteristics:**
- Immutable after creation
- Optimized for reading
- Faster than regular collections
- Lower memory overhead
- Perfect for lookups

**Use When:**
- Configuration data
- Lookup tables
- Reference data
- Cache keys
- Created once, read many times

---

## Slide 27: Frozen Collections Benchmarks
### Performance Comparison

| Operation | Dictionary | FrozenDictionary | ImmutableDictionary |
|-----------|-----------|------------------|---------------------|
| Lookup | 10 ns | **6 ns** | 25 ns |
| Iteration | 800 ns | **700 ns** | 1200 ns |
| Contains | 12 ns | **7 ns** | 28 ns |
| Creation | Fast | Medium | Slow |

**Speedup:** 40-60% faster lookups than Dictionary
**Memory:** 20-30% less memory

**When NOT to use:** Data that changes frequently

---

## Slide 28: Frozen Collections Example
### Real-World Usage

```csharp
// Configuration lookup table - perfect use case
public class ConfigService
{
    private readonly FrozenDictionary<string, string> _config;
    
    public ConfigService(IConfiguration configuration)
    {
        // Create once at startup
     _config = configuration
            .GetSection("AppSettings")
            .GetChildren()
       .ToFrozenDictionary(x => x.Key, x => x.Value);
    }
    
    public string GetValue(string key) => 
        _config.TryGetValue(key, out var value) ? value : string.Empty;
        // Lightning fast lookups!
}
```

---

## Slide 29: Frozen vs Immutable Collections
### Understanding the Differences

**ImmutableDictionary:**
- Copy-on-write semantics
- Can create modified versions
- Tree-based structure
- Slower but more flexible

**FrozenDictionary:**
- Truly immutable (no modifications)
- Optimized internal structure
- Faster lookups
- Less memory

**Choose Frozen when:** Data never changes after creation
**Choose Immutable when:** Need to create modified versions

---

## Slide 30: Section Title - Emergency Optimizations
# Part 7: Preparing for War
### When You Need to Go Fast

---

## Slide 31: Optimization Strategy
### The Right Approach

**1. Prepare Your Codebase:**
- Isolate code using dependency injection
- Unit test to protect behavior
- Create benchmark project
- Establish baseline metrics

**2. Measure & Profile:**
- Find the actual bottleneck
- Don't guess!
- Profile real workloads

**3. Optimize:**
- Focus on hot path only
- One change at a time
- Measure each change

**4. Validate:**
- Run tests (behavior unchanged)
- Run benchmarks (performance improved)
- Document why optimization exists

---

## Slide 32: Evaluate Code by "Lowering"
### Understanding What the Compiler Does

**Use Tools:**
- **ILSpy / dnSpy:** View IL (Intermediate Language)
- **SharpLab.io:** See C# ? IL ? ASM online
- **BenchmarkDotNet Disassembly:** View native ASM

**What to Look For:**
- Virtual calls (can't be inlined)
- Allocations (newobj instructions)
- Bounds checking
- Boxing (value ? reference type)

**Example:** See if LINQ compiles to optimal code

---

## Slide 33: Advanced Span<T> Optimizations
### Going Deeper

**Dictionary with Span keys (.NET 5+):**
```csharp
// Lookup without allocating string!
var dict = new Dictionary<string, int>();
ReadOnlySpan<char> spanKey = "someKey".AsSpan();

// Use alternate lookup to avoid allocation
ref var value = ref CollectionsMarshal.GetValueRefOrAddDefault(
    dict, spanKey.ToString(), out bool exists);
```

**Parsing without allocation:**
```csharp
// Parse CSV line without substring allocations
ReadOnlySpan<char> line = "John,Doe,25";
var comma1 = line.IndexOf(',');
var firstName = line.Slice(0, comma1);
var rest = line.Slice(comma1 + 1);
var comma2 = rest.IndexOf(',');
var lastName = rest.Slice(0, comma2);
var ageSpan = rest.Slice(comma2 + 1);
var age = int.Parse(ageSpan);
```

---

## Slide 34: ArrayPool<T> for Buffer Reuse
### Stop Allocating Temporary Arrays

**Without ArrayPool:**
```csharp
byte[] buffer = new byte[4096];
// Use buffer
// GC collects later
```

**With ArrayPool:**
```csharp
byte[] buffer = ArrayPool<byte>.Shared.Rent(4096);
try
{
    // Use buffer
}
finally
{
    ArrayPool<byte>.Shared.Return(buffer);
}
```

**Benefits:**
- Reuses arrays across requests
- Reduces GC pressure
- Great for high-throughput scenarios
- ASP.NET Core uses this internally

---

## Slide 35: Async Best Practices
### Don't Block the Thread!

? **Do:**
```csharp
public async Task<Result> GetDataAsync()
{
 return await _httpClient.GetAsync(url);
}
```

? **Don't:**
```csharp
public Result GetData()
{
    return GetDataAsync().Result;  // DEADLOCK RISK!
}
```

**Tips:**
- Async all the way (don't mix sync/async)
- Use `ConfigureAwait(false)` in libraries
- Avoid `async void` (except event handlers)
- Use `ValueTask<T>` for hot paths

---

## Slide 36: Stephen Toub's Performance Blog
### Learn from the Master

**Essential Reading:**
- "Performance Improvements in .NET 8" (and 7, 6, 5...)
- Deep dives into framework optimizations
- Real-world patterns
- Benchmarking methodology

**Key Learnings:**
- Frozen collections
- Span<T> usage
- Regex improvements
- LINQ optimizations
- GC improvements

**URL:** devblogs.microsoft.com/dotnet/author/stephen-toub/

---

## Slide 37: Section Title - Demo Time
# Live Benchmark Demonstrations
### Seeing is Believing

---

## Slide 38: Demo Overview
### What We'll Benchmark

**We'll run these live:**
1. String concatenation methods
2. LINQ vs Loop performance
3. Span<T> vs Array operations
4. Frozen Collections lookups
5. Collection types comparison

**Using BenchmarkDotNet:**
- Statistical analysis
- Memory diagnostics
- Multiple runs for accuracy
- Real numbers, not guesses

---

## Slide 39: Running BenchmarkDotNet
### Live Demo Commands

```bash
# Navigate to project
cd PerfDemo

# Run specific benchmark
dotnet run -c Release

# Select from menu:
# 1. Dictionary Benchmarks
# 2. String Benchmarks
# 3. LINQ vs Loop Benchmarks
# 4. Span vs Array Benchmarks
# 5. Frozen Collection Benchmarks
# 6. Collection Types Benchmarks
```

**Watch for:**
- Mean execution time
- Memory allocated
- GC collections
- Rank (fastest to slowest)

---

## Slide 40: Key Takeaways
### Performance Principles Summary

**1. Measure First**
- Profile before optimizing
- Use proper tools
- Understand the bottleneck

**2. Allocations Matter**
- Minimize heap allocations
- Pre-allocate when possible
- Use Span<T> and stackalloc

**3. Choose Wisely**
- Right collection for the job
- LINQ vs loops based on context
- Frozen collections for read-heavy

**4. Document & Test**
- Explain why optimizations exist
- Protect behavior with tests
- Measure real-world impact

---

## Slide 41: Performance Checklist
### Quick Reference Guide

? **Do:**
- Pre-allocate collection capacity
- Use StringBuilder for string building
- Use Span<T> for parsing/slicing
- TryGetValue over ContainsKey + indexer
- Async/await for I/O
- Profile to find hot paths
- Use frozen collections for lookups

? **Don't:**
- Throw exceptions for control flow
- Use string concatenation in loops
- Guess at performance problems
- Over-optimize before measuring
- Use LINQ in hot paths (unless fast enough)
- Allocate in tight loops

---

## Slide 42: Resources
### Continue Learning

**Tools:**
- BenchmarkDotNet: benchmarkdotnet.org
- PerfView: Microsoft's profiler
- dotnet-trace & dotnet-counters

**Reading:**
- Stephen Toub's blog posts
- .NET Performance repo: github.com/dotnet/performance
- Pro .NET Memory Management book
- Writing High-Performance .NET Code book

**Communities:**
- .NET Performance discussions
- r/dotnet on Reddit
- Stack Overflow [performance] tag

---

## Slide 43: Questions?
# Thank You!

**Contact Information:**
[Your Email]
[Your GitHub]
[Your LinkedIn]

**Demo Code:**
github.com/[your-repo]/PerfDemo

**Remember:** 
*"Premature optimization is the root of all evil... but measured optimization is the path to success!"*

---

## Slide 44: Bonus Slide - Quick Wins
### Easy Performance Improvements

**5-Minute Wins:**
1. Add capacity to List/Dictionary initialization
2. Change string concatenation to StringBuilder
3. Use TryGetValue instead of ContainsKey + indexer
4. Use async/await for database calls
5. Add indexes to frequently queried columns

**10-Minute Wins:**
1. Convert hot-path LINQ to loops
2. Use Span<T> for string parsing
3. Implement ArrayPool for buffers
4. Add response caching to APIs
5. Use frozen collections for config/lookups

**Measure the impact of each!**

---

# END OF SLIDES

## Presenter Notes

### General Tips:
- Start each section with a relatable example
- Show actual benchmark results from the demo project
- Keep code samples short and focused
- Use humor - performance talks can be dry!
- Interactive demos keep audience engaged

### Timing Suggestions:
- Introduction: 2 minutes
- Diagnosing: 5 minutes
- Best Practices: 8 minutes
- String/Span: 7 minutes
- LINQ: 6 minutes
- Collections: 8 minutes
- Frozen Collections: 5 minutes
- Emergency Opts: 7 minutes
- Live Demo: 10 minutes
- Q&A: 5+ minutes
**Total: ~60 minutes**

### Demo Tips:
- Have benchmarks pre-run in case of issues
- Show both fast and slow approaches side-by-side
- Explain the "Allocated" column importance
- Point out Gen0/Gen1/Gen2 collection counts
- Emphasize real-world impact, not just numbers
