using Assets.Scripts.StateMachine;
using Zenject;

namespace Assets.Scripts.States
{
    public class GameSMClient : SMClient, IInitializable
    {
        private readonly InitState _initState;

        public GameSMClient(ISMContext stateMachine, InitState initState) : base(stateMachine)
        {
            _initState = initState;
        }

        public void Initialize()
        {
            _stateMachine.GoTo(_initState, cancellationTokenSource.Token);
        }
    }
}
