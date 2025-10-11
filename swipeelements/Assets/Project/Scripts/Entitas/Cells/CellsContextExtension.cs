using Project.Gameplay.Puzzles;
using UnityEngine;

namespace Project.Entitas
{
    public static class CellsContextExtension
    {
        public static GameEntity CreateTile(this GameContext gameContext, CellType cellType, Coord coord, Vector3 position)
        {
            var entity = gameContext.CreateEntity();
            entity.isTileTag = true;
            entity.isInteractive = true;
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

        public static void AddMoveComponent(this GameEntity gameEntity, GameContext gameContext, MoveData moveData)
        {
            gameEntity.isInteractive = false;
            var moveSettings = gameContext.moveConfig.MoveSettings;
            var fromPosition = gameContext.GetEntityWithCellCoord(moveData.From).cellPosition.value;
            var toPosition = gameContext.GetEntityWithCellCoord(moveData.To).cellPosition.value;
            gameEntity.RemoveTileCoord();
            gameEntity.AddMove(moveData, fromPosition, toPosition, 0f, moveSettings.Duration, moveSettings.Curve);
        }

        public static void RemoveMoveComponent(this GameEntity gameEntity, MoveData moveData)
        {
            gameEntity.RemoveMove();
            gameEntity.AddMoveFinished(moveData);
            gameEntity.isInteractive = true;
        }
    }
}