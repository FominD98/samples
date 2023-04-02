using FluentAssertions;
using GeometryLibrary;

namespace Tests;

[TestFixture]
public class ShapeTests
{
    [TestCase(1)]
    [TestCase(0.5)]
    public void Circle_GetArea_ReturnsCorrectArea(double radius)
    {
        var area = new Circle { Radius = radius }.GetArea();
        area.Should().Be(Math.PI * radius * radius);
    }

    [TestCase(3, 4, 5, true)]
    [TestCase(2, 3, 4, false)]
    public void Triangle_GetArea_ReturnsCorrectArea(double a, double b, double c, bool isRight)
    {
        var triangle = new Triangle { A = a, B = b, C = c };
        var area = triangle.GetArea();

        triangle.IsRight().Should().Be(isRight);
        var p = (a + b + c) / 2;
        area.Should().Be(Math.Sqrt(p * (p - a) * (p - b) * (p - c)));
    }
}