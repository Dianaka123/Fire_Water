using Assets.Scripts.StateMachine;
using Cysharp.Threading.Tasks;
using Zenject;

namespace Assets.Scripts.States.Contexts
{
    public class GameSM : SMContext, IInitializable
    {
        private readonly LazyInject<InitState> _initState;

        public GameSM(LazyInject<InitState> initState)
        {
            _initState = initState;
        }

        public void Initialize()
        {
            GoTo(_initState.Value).Forget();
        }
    }
}
