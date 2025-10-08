using System;

namespace Project.Entitas
{
    public readonly struct Coord : IEquatable<Coord>
    {
        public int X { get; }
        public int Y { get; }

        public Coord(int x, int y)
        {
            X = x;
            Y = y;
        }

        public void Deconstruct(out int x, out int y)
        {
            x = X;
            y = Y;
        }

        public bool Equals(Coord other) => X == other.X && Y == other.Y;
        public override bool Equals(object obj) => obj is Coord other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(X, Y);

        public static bool operator ==(Coord left, Coord right) => left.Equals(right);
        public static bool operator !=(Coord left, Coord right) => !left.Equals(right);

        public override string ToString() => $"({X},{Y})";
    }
}