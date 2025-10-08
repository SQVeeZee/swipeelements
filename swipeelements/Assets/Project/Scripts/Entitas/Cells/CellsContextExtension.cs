using Project.Gameplay.Puzzles;

namespace Project.Entitas.Cells
{
    public static class CellsContextExtension
    {
        public static GameEntity CreateTile(this GameContext context, CellType cellType, Coord coord)
        {
            var entity = context.CreateEntity();
            entity.AddCell(cellType, CellState.Idle);
            entity.AddCoord(coord);
            entity.AddSpawn(cellType);
            entity.isTile = true;
            return entity;
        }

        public static GameEntity CreateCell(this GameContext context, CellType cellType, Coord coord)
        {
            var entity = context.CreateEntity();
            entity.AddCell(cellType, CellState.None);
            entity.AddCoord(coord);
            entity.isTile = false;
            return entity;
        }
    }
}