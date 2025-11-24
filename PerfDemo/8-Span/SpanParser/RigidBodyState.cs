using System.Globalization;

namespace PerfDemo.Examples;

public readonly record struct RigidBodyState(
    double X, double Y, double Z,
    double Vx, double Vy, double Vz)
{
    public override string ToString() =>
        $"{X:F3},{Y:F3},{Z:F3},{Vx:F3},{Vy:F3},{Vz:F3}";

    public static RigidBodyState Parse(string s)
    {
        var parts = s.Split(',');
        if (parts.Length != 6)
            throw new FormatException("Expected 6 components");

        return new RigidBodyState(double.Parse(parts[0]), 
            double.Parse(parts[1]), double.Parse(parts[2]),
            double.Parse(parts[3]), double.Parse(parts[4]), 
            double.Parse(parts[5])
        );
    }

    public static RigidBodyState ParseFast(ReadOnlySpan<char> input)
    {
        const int ExpectedComponents = 6;
        Span<double> values = stackalloc double[ExpectedComponents];
        Span<char> numberBuffer = stackalloc char[32];  

        int componentIndex = 0;
        int bufferPos = 0;

        for (int i = 0; i < input.Length; i++)
        {
            char c = input[i];

            if (c == ',' || i == input.Length - 1)
            {
                if (i == input.Length - 1 && c != ',')
                    numberBuffer[bufferPos++] = c;

                values[componentIndex++] = double.Parse(numberBuffer.Slice(0, bufferPos), CultureInfo.InvariantCulture);

                numberBuffer.Slice(0, bufferPos).Fill(' ');
                bufferPos = 0;

                if (componentIndex == ExpectedComponents)
                    break;
            }
            else if (c >= ' ' && c <= '~')
            {
                numberBuffer[bufferPos++] = c;
            }
        }

        if (componentIndex != ExpectedComponents)
            throw new FormatException($"Expected 6 components, got {componentIndex}");

        return new RigidBodyState(
            values[0], values[1], values[2],
            values[3], values[4], values[5]);
    }
}


