using Zenject;

namespace Project.Gameplay.Puzzles
{
    public class PuzzleInstaller : MonoInstaller
    {
        public override void InstallBindings() => BindPuzzles();
        private void BindPuzzles() => Container.Bind<MergesStepProcessor>().AsSingle();
    }
}