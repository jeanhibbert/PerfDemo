# Struct vs Ref Struct vs Readonly Ref Struct

## Overview

This document explains the differences between regular structs, ref structs, and readonly ref structs in C#, along with their performance characteristics and use cases.

---

## Regular Struct

## Comparison Table

| Feature | Struct | Ref Struct | Readonly Ref Struct |
|---------|--------|------------|---------------------|
| **Stack allocation** | ✅ | ✅ | ✅ |
| **Heap allocation** | ✅ | ❌ | ❌ |
| **Implement interfaces** | ✅ | ❌ | ❌ |
| **Boxing** | ✅ | ❌ | ❌ |
| **Class field** | ✅ | ❌ | ❌ |
| **Array storage** | ✅ | ❌ | ❌ |
| **Async methods** | ✅ | ❌ | ❌ |
| **LINQ** | ✅ | ❌ | ❌ |
| **Lambda capture** | ✅ | ❌ | ❌ |
| **Mutable** | ✅ | ✅ | ❌ |
| **GC pressure** | Medium | None | None |
| **Performance** | Good | Excellent | Best |

---

## Performance Characteristics

### Memory Allocation

**Regular Struct:**
```csharp
MyStruct s = new MyStruct(); // Stack if local, heap if in class
MyStruct[] arr = new MyStruct[100]; // Heap (array itself)
```

**Ref Struct:**
```csharp
MyRefStruct s = new MyRefStruct(); // Always stack
// MyRefStruct[] arr = new MyRefStruct[100]; // ❌ COMPILER ERROR!
```

**Readonly Ref Struct:**
```csharp
MyReadOnlyStruct s = new MyReadOnlyStruct(); // Always stack, immutable
// s.X = 5; // ❌ COMPILER ERROR - readonly!
```

### Passing Parameters

**By Value (Copy):**
```csharp
void Process(MyStruct s) { } // Copies all fields
```
- ⚠️ Can be expensive for large structs
- Creates a full copy

**By Reference (in):**
```csharp
void Process(in MyStruct s) { } // Pass by reference
```
- ✅ No copy, passes pointer
- ⚠️ May create defensive copy if struct is mutable

**Ref Struct by Reference:**
```csharp
void Process(in MyRefStruct s) { } // Most efficient
```
- ✅ Always stack, no defensive copies needed
- ✅ Compiler knows it won't escape

**Readonly Ref Struct by Reference:**
```csharp
void Process(in MyReadOnlyStruct s) { } // Optimal
```
- ✅ Zero copies guaranteed
- ✅ Compiler can optimize aggressively
- ✅ No defensive copies ever needed

---

## When to Use Each

### Use Regular Struct When:
- ✅ Representing a simple value type (Point, Color, etc.)
- ✅ Type needs to implement interfaces
- ✅ Type needs to be stored in collections
- ✅ Type needs to be used in async methods
- ✅ Type needs flexibility of heap/stack allocation
- ✅ Size is small (< 16 bytes recommended)

**Example:**
```csharp
public struct Point
{
    public int X { get; init; }
    public int Y { get; init; }
}
```

### Use Ref Struct When:
- ✅ Working with `Span<T>` or buffers
- ✅ Temporary computations within a method
- ✅ High-performance parsing scenarios
- ✅ Zero GC pressure is critical
- ✅ Lifetime is guaranteed within stack frame
- ✅ Need to avoid boxing at all costs

**Example:**
```csharp
public ref struct StringParser
{
    private ReadOnlySpan<char> _data;
    
    public StringParser(ReadOnlySpan<char> data)
    {
        _data = data;
    }
    
    public int ParseNextInt()
    {
        // Parse without allocating strings
        int comma = _data.IndexOf(',');
        var slice = comma >= 0 ? _data.Slice(0, comma) : _data;
        return int.Parse(slice);
    }
}
```

### Use Readonly Ref Struct When:
- ✅ All the ref struct benefits PLUS
- ✅ Data should never be mutated
- ✅ Creating views over existing memory
- ✅ Maximum performance is required
- ✅ Want to prevent defensive copies
- ✅ API contract guarantees immutability

**Example:**
```csharp
public readonly ref struct ReadOnlyPoint3D
{
    private readonly Span<double> _coordinates;
    
    public ReadOnlyPoint3D(Span<double> coords)
    {
        _coordinates = coords;
    }
    
    public double X => _coordinates[0];
    public double Y => _coordinates[1];
    public double Z => _coordinates[2];
    
    public double Distance()
    {
        return Math.Sqrt(X * X + Y * Y + Z * Z);
    }
}
```

---

## Common Pitfalls

### 1. Defensive Copies with Mutable Structs

```csharp
// BAD - causes defensive copy
public struct MutableStruct
{
    public int Value;
    public void Increment() => Value++;
}

void Process(in MutableStruct s)
{
    s.Increment(); // Compiler creates copy to prevent mutation!
}
```

**Solution:** Use `readonly` methods or make struct immutable.

### 2. Boxing Regular Structs

```csharp
MyStruct s = new MyStruct { X = 5 };
object obj = s; // ❌ Boxing - heap allocation!
ISomeInterface i = s; // ❌ Boxing - heap allocation!
```

**Solution:** Avoid boxing or use ref struct (can't implement interfaces).

### 3. Trying to Store Ref Struct on Heap

```csharp
// ❌ COMPILER ERROR
class MyClass
{
    public MyRefStruct Field; // ERROR: ref struct can't be class field
}

// ❌ COMPILER ERROR
async Task ProcessAsync()
{
    MyRefStruct s = new(); // ERROR: can't use in async method
    await Task.Delay(100);
}
```

**Solution:** Use regular struct if you need these features.

### 4. Large Struct Copies

```csharp
// BAD - struct is too large, copying is expensive
public struct LargeStruct
{
    public long A, B, C, D, E, F, G, H; // 64 bytes!
}

void Process(LargeStruct s) { } // Expensive copy!
```

**Solution:** Use `in` parameter or make it a class.

---

## Benchmark Results Interpretation

When running the `StructVsClassBenchmark`:

**Expected Results:**
1. **MyReadOnlyStruct** - Fastest, zero allocations
2. **MyRefStruct** - Very fast, zero allocations
3. **MyStruct** - Fast, zero allocations (but may have defensive copies)
4. **MyClass** - Slowest, allocates on heap (GC pressure)

**Memory Allocations:**
- Ref structs: 0 bytes allocated
- Regular structs: 0 bytes (unless boxed)
- Classes: Heap allocation per iteration

**Key Takeaway:**
The benchmark shows that ref structs eliminate GC pressure entirely while maintaining excellent performance. Readonly ref structs are the most optimized due to immutability guarantees.

---

## Best Practices

### ✅ Do:
- Keep structs small (< 16 bytes if possible)
- Make structs immutable when possible
- Use `readonly struct` for immutable value types
- Use `ref struct` for temporary, high-performance scenarios
- Use `readonly ref struct` for read-only views over memory
- Pass structs by `in` reference for large structs
- Use `Span<T>` and `ReadOnlySpan<T>` for buffer operations

### ❌ Don't:
- Make large, mutable structs (> 16 bytes)
- Box structs unnecessarily
- Use ref structs when you need heap allocation
- Implement mutable methods on readonly structs
- Copy large structs by value
- Use structs with many fields (use class instead)

---

## Real-World Use Cases

### Span-based String Parsing (Ref Struct)
```csharp
public ref struct CsvParser
{
    private ReadOnlySpan<char> _line;
    
    public bool TryGetNextField(out ReadOnlySpan<char> field)
    {
        // Zero-allocation parsing
        int comma = _line.IndexOf(',');
        if (comma < 0)
        {
            field = _line;
            _line = ReadOnlySpan<char>.Empty;
            return field.Length > 0;
        }
        
        field = _line.Slice(0, comma);
        _line = _line.Slice(comma + 1);
        return true;
    }
}
```

### Immutable Value Type (Readonly Struct)
```csharp
public readonly struct Money
{
    public decimal Amount { get; init; }
    public string Currency { get; init; }
    
    public Money Add(Money other)
    {
        if (Currency != other.Currency)
            throw new InvalidOperationException("Currency mismatch");
            
        return new Money 
        { 
            Amount = Amount + other.Amount, 
            Currency = Currency 
        };
    }
}
```

### High-Performance Buffer Operations (Readonly Ref Struct)
```csharp
public readonly ref struct BufferReader
{
    private readonly ReadOnlySpan<byte> _buffer;
    
    public int ReadInt32(int offset)
    {
        return BitConverter.ToInt32(_buffer.Slice(offset, 4));
    }
    
    public ReadOnlySpan<byte> ReadBytes(int offset, int length)
    {
        return _buffer.Slice(offset, length);
    }
}
```

---

## Additional Resources

- [Microsoft Docs: Struct types](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/struct)
- [Microsoft Docs: ref struct](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/ref-struct)
- [Microsoft Docs: readonly struct](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/struct#readonly-struct)
- [Performance Improvements in .NET](https://devblogs.microsoft.com/dotnet/performance-improvements-in-net-8/)
- [Span<T> documentation](https://docs.microsoft.com/en-us/dotnet/api/system.span-1)

---

## Summary

| Type | Allocation | Mutability | Use Case |
|------|-----------|------------|----------|
| **struct** | Stack/Heap | Mutable | General value types, small data structures |
| **readonly struct** | Stack/Heap | Immutable | Immutable value types, prevent defensive copies |
| **ref struct** | Stack only | Mutable | Temporary buffers, high-performance scenarios |
| **readonly ref struct** | Stack only | Immutable | Read-only memory views, maximum performance |

**Key Principle:** Choose the most restrictive type that meets your needs for the best performance and safety guarantees.
