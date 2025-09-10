using System;
using Project.Gameplay.Puzzles.Gameplay;

namespace Project.Gameplay.Puzzles
{
    public class MergesGame
    {
        public MergesState State { get; private set; }
        protected ILevelData LevelData;

        public event Action<MergesAction, MergesState, MergesStep> OnGameChanged;

        public void Initialize(MergesState state, ILevelData levelData)
        {
            State = state;
            LevelData = levelData;

            var dict = LevelData.InitialValues.ToDictionary();
            if (dict != null)
            {
                foreach (var cell in dict)
                {
                    var coord = cell.Key;
                    if (State[coord].CellType == CellType.None)
                    {
                        State[coord] = new MergesCell(cell.Value);
                    }
                }
            }

            var gridStep = InitializeGridStep.CalculateStep(State, levelData);
            ApplyStep(gridStep, MergesAction.None);
        }

        public void ApplySwipe((int X, int Y) from, (int X, int Y) to)
        {
            var moveData = new MoveData(from, to);
            var step = TileActionStep.Create(State, moveData);
            if (!step.MakeSense)
            {
                return;
            }

            ApplyStep(step, MergesAction.Recordable);
            ResolveBoard();
            TryWinGame(State);
        }

        private void ResolveBoard()
        {
            bool changed;
            do
            {
                var gravity = BoardGravityStep.CalculateStep(State);
                ApplyStep(gravity, MergesAction.Recordable);
                var destroy = BoardDestroyStep.CalculateStep(State);
                ApplyStep(destroy, MergesAction.Recordable);

                changed = destroy.MakeSense || gravity.MakeSense;
            } while (changed);
        }

        private bool TryWinGame(MergesState state)
        {
            if (state.CountTiles() > 0)
            {
                return false;
            }

            var winStep = WinGameStep.CalculateStep(state);
            ApplyStep(winStep, MergesAction.Braking);
            return true;
        }

        public bool ApplyStep(MergesStep step, MergesAction action)
        {
            if (!step.MakeSense)
            {
                return false;
            }

            var prev = State;
            State = step.Final;
            OnGameChanged?.Invoke(action, prev, step);
            return true;
        }
    }
}
