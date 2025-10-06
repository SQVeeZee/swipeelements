namespace Project.Gameplay.Puzzles
{
    public static class TileActionStep
    {
        public static MergesStep Create(MergesState state, MoveData moveData)
        {
            if (!state.IsValid(moveData.To) ||
                !state[moveData.From].IsInteractable ||
                !state[moveData.To].CanBeSwiped)
            {
                return new NonSenseStep(state);
            }

            if (state[moveData.From].IsTile && state[moveData.To].IsTile)
            {
                return SwitchCellsStep.CalculateStep(state, moveData.From, moveData.To);
            }
            if (state[moveData.From].IsTile && state[moveData.To].IsEmpty && moveData.From.IsHorizontalNeighbor(moveData.To))
            {
                return MoveCellStep.CalculateStep(state, moveData.From, moveData.To);
            }

            return new NonSenseStep(state);
        }
    }
}