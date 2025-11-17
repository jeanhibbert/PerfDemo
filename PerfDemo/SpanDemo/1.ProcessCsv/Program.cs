//using ReflectionIT.HighPerformance.Buffers;
using System;
using System.Buffers;
using System.Buffers.Text;
using System.IO;
using System.Text;

public static class  ProcessCsvRunner
{
    public static void Run ()
    {
        // Warmup
        Console.WriteLine("Warmup Started");

        for (int i = 0; i < 32; i++)
        {
            OldSchool();
            //OldSchool2();
            //UseSpan();
            //UseSpanAndMemoryPool();
            //UseSpanMemoryPoolAndStringPool();
        }

        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();

        Console.WriteLine("Warmup Done");

        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        var memorySize = GC.GetTotalAllocatedBytes();

        OldSchool();
        //OldSchool2();
        //UseSpan();
        //UseSpanAndMemoryPool();
        //UseSpanMemoryPoolAndStringPool();

        stopwatch.Stop();
        Console.WriteLine($"Duration:  {stopwatch.Elapsed.TotalSeconds} sec");
        Console.WriteLine($"Allocated: {(GC.GetTotalAllocatedBytes() - memorySize) / 1024 / 1024} mb");
        Console.ReadKey();
    }


    static void OldSchool()
    {

        string[] lines = File.ReadAllLines("taxi-fare-train-utf8.csv");

        foreach (var line in lines.Skip(1))
        { // Skip the Header
            string[] values = line.Split(',');

            var ride = new TaxiRide(
                values[0], (RateCodes)int.Parse(values[1]), byte.Parse(values[2]), short.Parse(values[3]),
                double.Parse(values[4]), values[5], decimal.Parse(values[6])
            );

            //Console.WriteLine(ride.ToString());
        }
    }

    static void OldSchool2()
    {

        IEnumerable<string> lines = File.ReadLines("taxi-fare-train-utf8.csv");

        foreach (var line in lines.Skip(1))
        { // Skip the Header
            string[] values = line.Split(',');

            var ride = new TaxiRide(
                values[0], (RateCodes)int.Parse(values[1]), byte.Parse(values[2]), short.Parse(values[3]),
                double.Parse(values[4]), values[5], decimal.Parse(values[6])
            );

            //Console.WriteLine(ride.ToString());
        }
    }


    static void UseSpan()
    {

        byte[] bytes = File.ReadAllBytes("taxi-fare-train-utf8.csv");
        ReadOnlySpan<byte> span = bytes;

        bool first = true;
        foreach (Range range in span.Split((byte)'\n'))
        {
            if (first)
            {
                first = false;
                continue;
            }
            ReadOnlySpan<byte> line = span[range];

            TaxiRide ride = CreateTaxiRideFromSpan(line);

            //Console.WriteLine(ride.ToString());
        }
    }

    static void UseSpanAndMemoryPool()
    {

        // Open the file in a pooled byte array (Memory/Span) and store it in a ReadOnlySpan<byte>
        using Stream stream = File.OpenRead("taxi-fare-train-utf8.csv");
        var length = (int)stream.Length;

        using var memoryOwner = MemoryPool<byte>.Shared.Rent(length);

        var span = memoryOwner.Memory.Span.Slice(0, length); // 'remove' overcapacity
        stream.ReadExactly(span);
        ReadOnlySpan<byte> roSpan = span; // Use it readonly so we can use Split()

        bool first = true;
        foreach (Range range in roSpan.Split((byte)'\n'))
        {
            if (first)
            {
                first = false;
                continue;
            }
            ReadOnlySpan<byte> line = span[range];

            TaxiRide ride = CreateTaxiRideFromSpan(line);

            //Console.WriteLine(ride.ToString());
        }
    }

    static TaxiRide CreateTaxiRideFromSpan(ReadOnlySpan<byte> line)
    {
        string vendorId = default!;
        RateCodes rateCode = default;
        byte passengerCount = default;
        short tripTimeInSecs = default;
        double tripDistance = default;
        string paymentType = default!;
        decimal fareAmount = default;

        int t = 0;
        foreach (Range range in line.Split((byte)','))
        {
            ReadOnlySpan<byte> value = line[range];

            switch (t++)
            {
                case 0:
                    vendorId = Encoding.UTF8.GetString(value);
                    break;
                case 1:
                    Utf8Parser.TryParse(value, out int rc, out var _);
                    rateCode = (RateCodes)rc;
                    break;
                case 2:
                    Utf8Parser.TryParse(value, out passengerCount, out int _);
                    break;
                case 3:
                    Utf8Parser.TryParse(value, out tripTimeInSecs, out var _);
                    break;
                case 4:
                    Utf8Parser.TryParse(value, out tripDistance, out var _);
                    break;
                case 5:
                    paymentType = Encoding.UTF8.GetString(value);
                    break;
                case 6:
                    Utf8Parser.TryParse(value, out fareAmount, out var _);
                    break;
                default:
                    break;
            }
        }
        return new TaxiRide(vendorId, rateCode, passengerCount, tripTimeInSecs, tripDistance, paymentType, fareAmount);
    }

    //static void UseSpanMemoryPoolAndStringPool()
    //{

    //    var stringPool = new Utf8StringPool();

    //    // Open the file in a pooled byte array (Memory/Span) and store it in a ReadOnlySpan<byte>
    //    using Stream stream = File.OpenRead("taxi-fare-train-utf8.csv");
    //    var length = (int)stream.Length;

    //    using var memoryOwner = MemoryPool<byte>.Shared.Rent(length);

    //    var span = memoryOwner.Memory.Span.Slice(0, length); // 'remove' overcapacity
    //    stream.ReadExactly(span);
    //    ReadOnlySpan<byte> roSpan = span; // Use it readonly so we can use Split()

    //    bool first = true;
    //    foreach (Range range in roSpan.Split((byte)'\n'))
    //    {
    //        if (first)
    //        {
    //            first = false;
    //            continue;
    //        }
    //        ReadOnlySpan<byte> line = span[range];

    //        TaxiRide ride = CreateTaxiRideFromSpanUsingStringPool(line, stringPool);

    //        //Console.WriteLine(ride.ToString());
    //    }
    //}

    //static TaxiRide CreateTaxiRideFromSpanUsingStringPool(ReadOnlySpan<byte> line, Utf8StringPool stringPool)
    //{
    //    string vendorId = default!;
    //    RateCodes rateCode = default;
    //    byte passengerCount = default;
    //    short tripTimeInSecs = default;
    //    double tripDistance = default;
    //    string paymentType = default!;
    //    decimal fareAmount = default;

    //    int t = 0;
    //    foreach (Range range in line.Split((byte)','))
    //    {
    //        ReadOnlySpan<byte> value = line[range];

    //        switch (t++)
    //        {
    //            case 0:
    //                vendorId = stringPool.GetOrAdd(value);
    //                break;
    //            case 1:
    //                Utf8Parser.TryParse(value, out int rc, out var _);
    //                rateCode = (RateCodes)rc;
    //                break;
    //            case 2:
    //                Utf8Parser.TryParse(value, out passengerCount, out var _);
    //                break;
    //            case 3:
    //                Utf8Parser.TryParse(value, out tripTimeInSecs, out var _);
    //                break;
    //            case 4:
    //                Utf8Parser.TryParse(value, out tripDistance, out var _);
    //                break;
    //            case 5:
    //                paymentType = stringPool.GetOrAdd(value);
    //                break;
    //            case 6:
    //                Utf8Parser.TryParse(value, out fareAmount, out var _);
    //                break;
    //            default:
    //                break;
    //        }
    //    }
    //    return new TaxiRide(vendorId, rateCode, passengerCount, tripTimeInSecs, tripDistance, paymentType, fareAmount);
    //}


}
