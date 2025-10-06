namespace Project.Gameplay.Puzzles
{
    public static class IndexExtensions
    {
        public static bool IsHorizontalNeighbor(this (int X, int Y) index1, (int X, int Y) index2) => index1 == index2.Left() || index1 == index2.Right();
        public static (int X, int Y) Shift(this (int X, int Y) index, int x, int y) => (index.X + x, index.Y + y);

        public static (int X, int Y) Top(this (int X, int Y) index) => index.Shift(0, 1);
        public static (int X, int Y) Right(this (int X, int Y) index) => index.Shift(1, 0);
        public static (int X, int Y) Bottom(this (int X, int Y) index) => index.Shift(0, -1);
        public static (int X, int Y) Left(this (int X, int Y) index) => index.Shift(-1, 0);

    }
}