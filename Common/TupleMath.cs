using System.Diagnostics;
using System.Numerics;

namespace Common;

public static class ExtraMath<T> where T : INumber<T>
{
    public static T Gcd(T a, T b)
    {
        if (b == T.Zero) return a;
        return Gcd(b, a % b);
    }

    public static T Lcm(T a, T b) => a * b / Gcd(a, b);

    public static void RunTests()
    {
        var gcd = ExtraMath<int>.Gcd;
        Debug.Assert(gcd(24,6) == 6);
        Debug.Assert(gcd(24,9) == 3);
        Debug.Assert(gcd(9,24) == 3);
        Debug.Assert(gcd(48,18) == 6);
        Debug.Assert(gcd(7,13) == 1);
    }
}

public static class ExtraMath
{
    public static long Concat(long a, long b) => (long)(a * Math.Pow(10, (long)Math.Log10(b) + 1L)) + b;
    public static int Concat(int a, int b) => (int)(a * Math.Pow(10, (int)Math.Log10(b) + 1)) + b;
}

public static class TupleMath<T> where T : INumber<T>
{
    public static (T X, T Y) Add((T X, T Y) a, (T X, T Y) b) => (a.X + b.X, a.Y + b.Y);
    public static (T X, T Y) Sub((T X, T Y) a, (T X, T Y) b) => (a.X - b.X, a.Y - b.Y);
    public static (T X, T Y) ScalarMult(T a, (T X, T Y) b) => (a * b.X, a * b.Y);
    public static (T X, T Y) ScalarDiv(T a, (T X, T Y) b) => (b.X / a, b.Y / a);
    public static T DotProduct((T X, T Y) a, (T X, T Y) b) => a.X * b.X + a.Y * b.Y;
}