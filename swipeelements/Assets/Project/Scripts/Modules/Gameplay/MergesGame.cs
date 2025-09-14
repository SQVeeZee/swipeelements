using System;
using JetBrains.Annotations;
using Project.Profile;
using Zenject;

namespace Project.Gameplay.Puzzles
{
    [UsedImplicitly]
    public class MergesGame
    {
        private readonly SessionProfile _sessionProfile;
        private readonly CellsStateConverter _cellsStateConverter;
        private readonly MergesStepProcessor _stepProcessor;

        public event Action<MergesStep> OnStepApply;

        [Inject]
        private MergesGame(
            SessionProfile sessionProfile,
            CellsStateConverter cellsStateConverter,
            MergesStepProcessor stepProcessor)
        {
            _sessionProfile = sessionProfile;
            _cellsStateConverter = cellsStateConverter;
            _stepProcessor = stepProcessor;
        }

        public void Initialize(MergesState mergesState, ILevelData levelData)
        {
            var data = _stepProcessor.Initialize(mergesState, levelData);
            TryApplyStep(data);
        }

        public void InitializeContinue(MergesState mergesState, ILevelData levelData)
        {
            Initialize(mergesState, levelData);
            var state = _cellsStateConverter.ToState(_sessionProfile.MergesState);
            ResolveNormalizeDestroy(state);
        }

        public void RunNextStepIfNeeded(MergesStep mergesStep)
        {
            if (_sessionProfile.MergesState == null)
            {
                return;
            }
            var state = _cellsStateConverter.ToState(_sessionProfile.MergesState);
            if (TryWinGame(state))
            {
                return;
            }
            switch (mergesStep)
            {
                case DestroyCellsStep:
                    ResolveNormalizeDestroy(state);
                    break;
                case FallingCellsStep { AnyNewFalling: true }:
                    ResolveDestroyNormalize(state);
                    break;
                case MoveCellStep:
                case SwitchCellsStep:
                    ResolveNormalizeDestroy(state);
                    break;
            }
        }

        public void ApplySwipe((int X, int Y) from, (int X, int Y) to)
        {
            var state = _cellsStateConverter.ToState(_sessionProfile.MergesState);
            var step = _stepProcessor.ApplySwipe(state, from, to);
            TryApplyStep(step);
        }

        private void ResolveNormalizeDestroy(MergesState state)
        {
            var normalizeStep = _stepProcessor.NormalizeBoard(state);
            if(TryApplyStep(normalizeStep))
            {
                return;
            }
            var destroyCellsStep = _stepProcessor.DestroyCells(state);
            TryApplyStep(destroyCellsStep);
        }

        private void ResolveDestroyNormalize(MergesState state)
        {
            var destroyCellsStep = _stepProcessor.DestroyCells(state);
            if (TryApplyStep(destroyCellsStep))
            {
                return;
            }
            var normalizeStep = _stepProcessor.NormalizeBoard(state);
            TryApplyStep(normalizeStep);
        }

        private bool TryWinGame(MergesState state)
        {
            var stepData = _stepProcessor.TryWinGame(state);
            TryApplyStep(stepData);
            return stepData.Step.MakeSense;
        }

        private bool TryApplyStep(StepData stepData)
        {
            if (!stepData.Step.MakeSense)
            {
                return false;
            }
            RecordAction(stepData);
            OnStepApply?.Invoke(stepData.Step);
            return true;
        }

        private void RecordAction(StepData stepData)
        {
            var action = stepData.MergesAction;
            var step = stepData.Step;
            switch (action)
            {
                case MergesAction.None:
                    return;
                case MergesAction.Recordable:
                    _sessionProfile.MergesState = step.Final;
                    _sessionProfile.MergesState.LogState(step.GlobalId);
                    break;
                case MergesAction.Braking:
                    _sessionProfile.Clear();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(action), action, null);
            }
        }
    }
}
