using Project.Entitas;

namespace Project.Gameplay.Puzzles
{
    public static class IndexExtensions
    {
        public static bool IsHorizontalNeighbor(this Coord index1, Coord index2) => index1 == index2.Left() || index1 == index2.Right();
        public static Coord Shift(this Coord index, int x, int y) => new (index.X + x, index.Y + y);

        public static Coord Top(this Coord index) => index.Shift(0, 1);
        public static Coord Right(this Coord index) => index.Shift(1, 0);
        public static Coord Bottom(this Coord index) => index.Shift(0, -1);
        public static Coord Left(this Coord index) => index.Shift(-1, 0);

    }
}