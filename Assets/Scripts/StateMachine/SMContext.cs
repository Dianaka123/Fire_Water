using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;

namespace Assets.Scripts.StateMachine
{
    internal enum Stage
    {
        Enter,
        Run,
        Exit
    }

    public abstract class SMContext : ISMContext
    {
        public IState CurrentState { get; private set; }

        private Stack<IState> _states = new Stack<IState>();
        private Stage _stage;

        public async UniTask Run(CancellationToken token)
        {
            if (_stage != Stage.Run)
            {
                return;
            }

            await CurrentState.Run(token);
        }

        public async UniTask GoTo(IState state, CancellationToken token)
        {
            _states.Push(state);
            await StateTransition(state, token);
        }

        public async UniTask Back(CancellationToken token)
        {
            if (_states.Count > 1)
            {
                _states.Pop();
                await StateTransition(_states.Peek(), token);
            }
        }

        private async UniTask StateTransition(IState newState, CancellationToken token = default)
        {
            if (CurrentState == newState)
            {
                return;
            }

            if (CurrentState != null)
            {
                _stage = Stage.Exit;
                await CurrentState.Exit(token);
            }

            CurrentState = newState;

            _stage = Stage.Enter;
            await CurrentState.Enter(token);

            _stage = Stage.Run;
        }
    }
}