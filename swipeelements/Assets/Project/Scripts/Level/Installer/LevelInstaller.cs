using Project.Gameplay;
using Project.Gameplay.Puzzles;
using UnityEngine;
using Zenject;

namespace Level
{
    public class LevelInstaller : MonoInstaller
    {
        [Header("level")]
        [SerializeField]
        private BoardSettings _boardSettings;
        [SerializeField]
        private LevelsConfig _levelsConfig;

        [Header("cells")]
        [SerializeField]
        private CellsConfig _cellsConfig;
        [SerializeField]
        private TileIdleTimerConfig _tileIdleTimerConfig;

        public override void InstallBindings()
        {
            BindLevel();
            BindPuzzles();
            BindCells();
        }

        private void BindLevel()
        {
            Container.Bind<GamePoolManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<LevelController>().AsSingle();
            Container.Bind<LevelProgress>().AsSingle();
            Container.Bind<LevelsConfig>().FromInstance(_levelsConfig).AsSingle();
            Container.BindInterfacesAndSelfTo<BoardSettings>().FromInstance(_boardSettings).AsSingle();
        }

        private void BindPuzzles()
        {
            Container.Bind<MergesGame>().AsSingle();
            Container.Bind<MergesBoard>().AsSingle();
        }

        private void BindCells()
        {
            Container.Bind<CellOrderController>().AsSingle();
            Container.Bind<CellsConfig>().FromInstance(_cellsConfig).AsSingle();
            Container.Bind<CellsContainer>().AsSingle();
            Container.BindFactory<MergesCell, Vector3, Transform, CellObject, CellsFactory>().FromFactory<CellsFactory>();

            BindTiles();
        }

        private void BindTiles()
        {
            Container.Bind<TileAnimatorController>().AsSingle();
            Container.Bind<TileIdleTimerConfig>().FromInstance(_tileIdleTimerConfig).AsSingle();
        }
    }
}
