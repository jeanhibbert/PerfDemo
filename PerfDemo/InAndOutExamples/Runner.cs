
using BenchmarkDotNet.Running;
using InAndOutExamples;


public class Runner
{
    public void Run()
    {
        BenchmarkRunner.Run<InAndOutBenchmarks>();
    }


    double CalculateDistance(in Point point1, in Point point2)
    {
        double xDifference = point1.X - point2.X;
        double yDifference = point1.Y - point2.Y;
        double zDifference = point1.Z - point2.Z;

        return Math.Sqrt(xDifference * xDifference + yDifference * yDifference + zDifference * zDifference);
    }

    struct Point
    {
        public double X { get; set; }

        public double Y { get; set; }

        public double Z { get; set; }

        public Point(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }

}