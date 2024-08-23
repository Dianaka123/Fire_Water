using Cysharp.Threading.Tasks;
using System.Threading;
using Zenject;

namespace Assets.Scripts.StateMachine
{
    public class SMClient: ITickable
    {
        protected ISMContext _stateMachine;
        protected CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        public SMClient(ISMContext stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void Tick()
        {
            _stateMachine.Run(cancellationTokenSource.Token).Forget();
        }
    }
}
