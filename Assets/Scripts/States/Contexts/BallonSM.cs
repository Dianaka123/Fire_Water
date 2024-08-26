using Assets.Scripts.StateMachine;
using Cysharp.Threading.Tasks;
using Zenject;

namespace Assets.Scripts.States.Contexts
{
    public class BallonSM : SMContext, IInitializable
    {
        private readonly LazyInject<BalloonsState> _balloonsState;

        public BallonSM(LazyInject<BalloonsState> balloonsState)
        {
            _balloonsState = balloonsState;
        }

        public void Initialize()
        {
            GoTo(_balloonsState.Value).Forget();
        }
    }
}
