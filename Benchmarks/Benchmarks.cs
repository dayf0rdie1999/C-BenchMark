using System;
using BenchmarkDotNet;
using BenchmarkDotNet.Attributes;

namespace RmApp.Benchmarks
{
    [MemoryDiagnoser]
    public class Benchmarks
    {

        [Params(1_000, 10_000, 100_000)]
        public int ShapeCount { get; set; }

        private ShapeBase[] _shapes = Array.Empty<ShapeBase>();
        private ShapeUnion[] _shapeUnions = Array.Empty<ShapeUnion>();

        [GlobalSetup]
        public void Setup()
        {
            var rnd = new Random(42);

            _shapes = new ShapeBase[ShapeCount];
            _shapeUnions = new ShapeUnion[ShapeCount];

            for (int i = 0; i < ShapeCount; i++)
            {
                // Alternate types for variety
                switch (i % 4)
                {
                    case 0:
                        var s = rnd.NextDouble() * 10 + 1;
                        _shapes[i] = new Square(s);
                        _shapeUnions[i] = new ShapeUnion { Type = ShapeType.Shape_Square, Width = s, Height = s };
                        break;
                    case 1:
                        var w = rnd.NextDouble() * 10 + 1;
                        var h = rnd.NextDouble() * 10 + 1;
                        _shapes[i] = new Rectangle(w, h);
                        _shapeUnions[i] = new ShapeUnion { Type = ShapeType.Shape_Rectangle, Width = w, Height = h };
                        break;
                    case 2:
                        w = rnd.NextDouble() * 10 + 1;
                        h = rnd.NextDouble() * 10 + 1;
                        _shapes[i] = new Triangle(w, h);
                        _shapeUnions[i] = new ShapeUnion { Type = ShapeType.Shape_Triangle, Width = w, Height = h };
                        break;
                    default:
                        var r = rnd.NextDouble() * 10 + 1;
                        _shapes[i] = new Circle(r);
                        _shapeUnions[i] = new ShapeUnion { Type = ShapeType.Shape_Circle, Width = r, Height = r };
                        break;
                }
            }
        }

        // [Benchmark]
        // public double TotalAreaVTBL()
        // {
        //     double accum = 0d;
        //     var shapes = _shapes;
        //     for (int i = 0; i < shapes.Length; ++i)
        //         accum += shapes[i].Area();
        //     return accum;
        // }

        // [Benchmark]
        // public double TotalAreaSwitch()
        // {
        //     // Implement your benchmark here
        //     double accum = 0d;
        //     var shapeUnions = _shapeUnions;
        //     for (int i = 0; i < shapeUnions.Length; ++i)
        //         accum += GetAreaSwitch(shapeUnions[i]);
        //     return accum;
        // }

        // [Benchmark]
        // public double TotalAreaUnion()
        // {
        //     // Implement your benchmark here
        //     double accum = 0d;
        //     var shapeUnions = _shapeUnions;
        //     for (int i = 0; i < shapeUnions.Length; ++i)
        //         accum += GetAreaUnion(shapeUnions[i]);
        //     return accum;
        // }

        // [Benchmark]
        // public double CornerAreaVTBL()
        // {
        //     double accum = 0d;
        //     var shapes = _shapes;
        //     for (int i = 0; i < shapes.Length; ++i)
        //     {
        //         int corners = shapes[i].CornerCount;             // e.g., 0 for circle, 3, 4, etc.
        //         double weight = 1.0 / (1.0 + corners);           // (1 / (1 + CornerCount))
        //         accum += weight * shapes[i].Area();              // * Area()
        //     }
        //     return accum;
        // }

        [Benchmark]
        public double CornerAreaVTBL4()
        {
            double accum0 = 0d, accum1 = 0d, accum2 = 0d, accum3 = 0d;

            var shapes = _shapes;
            int i = 0, len = shapes.Length;

            // Process 4 at a time
            for (; i <= len - 4; i += 4)
            {
                var s0 = shapes[i];
                var s1 = shapes[i + 1];
                var s2 = shapes[i + 2];
                var s3 = shapes[i + 3];

                accum0 += 1.0 / (1.0 + s0.CornerCount) * s0.Area();
                accum1 += 1.0 / (1.0 + s1.CornerCount) * s1.Area();
                accum2 += 1.0 / (1.0 + s2.CornerCount) * s2.Area();
                accum3 += 1.0 / (1.0 + s3.CornerCount) * s3.Area();
            }

            double result = accum0 + accum1 + accum2 + accum3;

            // Tail for leftover elements (0..3)
            for (; i < len; i++)
            {
                var s = shapes[i];
                result += 1.0 / (1.0 + s.CornerCount) * s.Area();
            }

            return result;
        }

        // [Benchmark]
        // public double CornerAreaSwitch()
        // {
        //     double accum = 0d;
        //     var shapeUnions = _shapeUnions;
        //     for (int i = 0; i < shapeUnions.Length; ++i)
        //     {
        //         int corners = GetCornerCountSwitch(shapeUnions[i].Type);
        //         double weight = 1.0 / (1.0 + corners);
        //         accum += weight * GetAreaSwitch(shapeUnions[i]);
        //     }
        //     return accum;
        // }

        [Benchmark]
        public double CornerAreaSwitch4()
        {
            double accum0 = 0d, accum1 = 0d, accum2 = 0d, accum3 = 0d;

            var a = _shapeUnions;
            int i = 0, len = a.Length;

            // Process 4 at a time
            for (; i <= len - 4; i += 4)
            {
                var s0 = a[i];
                var s1 = a[i + 1];
                var s2 = a[i + 2];
                var s3 = a[i + 3];

                int c0 = GetCornerCountSwitch(s0.Type);
                int c1 = GetCornerCountSwitch(s1.Type);
                int c2 = GetCornerCountSwitch(s2.Type);
                int c3 = GetCornerCountSwitch(s3.Type);

                accum0 += 1.0 / (1.0 + c0) * GetAreaSwitch(s0);
                accum1 += 1.0 / (1.0 + c1) * GetAreaSwitch(s1);
                accum2 += 1.0 / (1.0 + c2) * GetAreaSwitch(s2);
                accum3 += 1.0 / (1.0 + c3) * GetAreaSwitch(s3);
            }

            double result = accum0 + accum1 + accum2 + accum3;

            // Tail for remaining 0..3 elements
            for (; i < len; i++)
            {
                var s = a[i];
                int c = GetCornerCountSwitch(s.Type);
                result += 1.0 / (1.0 + c) * GetAreaSwitch(s);
            }

            return result;
        }

        // [Benchmark]
        // public double CornerAreaUnion()
        // {
        //     double accum = 0d;
        //     var shapeUnions = _shapeUnions;
        //     for (int i = 0; i < shapeUnions.Length; ++i)
        //     {
        //         accum += GetCornerAreaUnion(shapeUnions[i]);
        //     }
        //     return accum;
        // }

        [Benchmark]
        public double CornerAreaUnion4()
        {
            double accum0 = 0d, accum1 = 0d, accum2 = 0d, accum3 = 0d;

            var a = _shapeUnions;
            int i = 0, len = a.Length;

            // Process 4 at a time
            for (; i <= len - 4; i += 4)
            {
                var s0 = a[i];
                var s1 = a[i + 1];
                var s2 = a[i + 2];
                var s3 = a[i + 3];

                accum0 += GetCornerAreaUnion(s0);
                accum1 += GetCornerAreaUnion(s1);
                accum2 += GetCornerAreaUnion(s2);
                accum3 += GetCornerAreaUnion(s3);
            }

            double result = accum0 + accum1 + accum2 + accum3;

            // Tail for remaining 0..3 elements
            for (; i < len; i++)
            {
                var s = a[i];
                result += GetCornerAreaUnion(s);
            }

            return result;
        }

        public static double GetAreaSwitch(ShapeUnion shape)
        {
            double result = 0f;

            switch (shape.Type)
            {
                case ShapeType.Shape_Square: result = shape.Width * shape.Width; break;
                case ShapeType.Shape_Rectangle: result = shape.Width * shape.Height; break;
                case ShapeType.Shape_Triangle: result = 0.5f * shape.Width * shape.Height; break;
                case ShapeType.Shape_Circle: result = MathF.PI * shape.Width * shape.Width; break;
                case ShapeType.Shape_Count: break;
            }

            return result;
        }

        public static int GetCornerCountSwitch(ShapeType type) =>
        type switch
        {
            ShapeType.Shape_Square => 4,
            ShapeType.Shape_Rectangle => 4,
            ShapeType.Shape_Triangle => 3,
            ShapeType.Shape_Circle => 0,
            _ => 0
        };

        private static readonly double[] CTable = [1.0, 1.0, 0.5, Math.PI];
        public static double GetAreaUnion(ShapeUnion shape)
            => CTable[(int)shape.Type] * shape.Width * shape.Height;
            
        private static readonly double[] CornerAreaTable = { 1.0 / 5.0, 1.0 / 5.0, 0.5 / 4.0, Math.PI };
        public static double GetCornerAreaUnion(ShapeUnion shape)
            => CornerAreaTable[(int)shape.Type] * shape.Width * shape.Height;
    }
}

public enum ShapeType : int
{
    Shape_Square = 0,
    Shape_Rectangle = 1,
    Shape_Triangle = 2,
    Shape_Circle = 3,
    Shape_Count = 4
}

public struct ShapeUnion
{
    public ShapeType Type;
    public double Width;
    public double Height;
}