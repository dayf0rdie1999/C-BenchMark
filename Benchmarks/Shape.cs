using System;

namespace RmApp.Benchmarks;

public abstract class ShapeBase
{
    public virtual double Area() => 0;
    public virtual int CornerCount => 0;
}

public class Square : ShapeBase
{
    public Square(double s)
    {
        Side = s;
    }
    public double Side { get; set; }
    public override double Area()
    {
        return Side * Side;
    }
    public override int CornerCount => 4;
}

public class Rectangle : ShapeBase
{
    public Rectangle(double width, double height)
    {
        Width = width;
        Height = height;
    }
    public double Width { get; set; }
    public double Height { get; set; }
    public override double Area()
    {
        return Width * Height;
    }

    public override int CornerCount => 4;
}

public class Triangle : ShapeBase
{
    public double Base { get; set; }
    public double Height { get; set; }
    public Triangle(double width, double height)
    {
        Base = width;
        Height = height;
    }
    public override double Area()
    {
        return 0.5 * Base * Height;
    }

    public override int CornerCount => 3;
}

public class Circle(double r) : ShapeBase
{
    public double Radius { get; set; } = r;

    public override double Area()
    {
        return Math.PI * Radius * Radius;
    }
    public override int CornerCount => 0;
}
