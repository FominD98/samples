namespace GeometryLibrary;

public class Triangle : IShape
{
    public double A { get; set; }
    public double B { get; set; }
    public double C { get; set; }

    public double GetArea()
    {
        var p = (A + B + C) / 2;
        return Math.Sqrt(p * (p - A) * (p - B) * (p - C));
    }

    public bool IsRight() => A * A + B * B == C * C || A * A + C * C == B * B || B * B + C * C == A * A;
}