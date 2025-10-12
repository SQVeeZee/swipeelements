using Project.Gameplay.Puzzles;
using UnityEngine;

namespace Project.Entitas
{
    public static class GameContextExtension
    {
        public static GameEntity CreateTile(this GameContext gameContext, CellType cellType, Coord coord, Vector3 position)
        {
            var entity = gameContext.CreateEntity();
            entity.isTileTag = true;
            entity.AddTileType(cellType);
            entity.AddTileCoord(coord);
            entity.AddTilePosition(position);
            entity.AddSpawn(cellType, coord, position);
            return entity;
        }

        public static GameEntity CreateCell(this GameContext gameContext, CellType cellType, Coord coord, Vector3 position)
        {
            var entity = gameContext.CreateEntity();
            entity.isCellTag = true;
            entity.AddCellType(cellType);
            entity.AddCellCoord(coord);
            entity.AddCellPosition(position);
            return entity;
        }

        public static bool TryGetEntityWithTileCoord(this GameContext gameContext, Coord coord, out GameEntity gameEntity)
        {
            gameEntity = gameContext.GetEntityWithTileCoord(coord);
            return gameEntity != null;
        }
    }
}