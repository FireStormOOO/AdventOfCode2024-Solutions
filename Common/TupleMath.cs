using System.Numerics;

namespace Common;

public static class ExtraMath<T> where T : INumber<T>, IExponentialFunctions<T>, ILogarithmicFunctions<T>
{
    public static T Add(T a, T b) => a + b;
    public static T Mul(T a, T b) => a * b;
    public static T Concat(T a, T b) => a * T.Exp10(T.Log10(b) + T.One) + b;
}

public static class TupleMath<T> where T : INumber<T>,IExponentialFunctions<T>,ILogarithmicFunctions<T>
{
    public static (T X, T Y) Add((T X, T Y) a, (T X, T Y) b) => (a.X + b.X, a.Y + b.Y);
    public static (T X, T Y) Sub((T X, T Y) a, (T X, T Y) b) => (a.X - b.X, a.Y - b.Y);
    public static (T X, T Y) ScalarMult(T a, (T X, T Y) b) => (a * b.X, a * b.Y);
    public static T DotProduct((T X, T Y) a, (T X, T Y) b) => a.X * b.X + a.Y * b.Y;
}