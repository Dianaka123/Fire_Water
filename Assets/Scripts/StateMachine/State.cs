using Cysharp.Threading.Tasks;
using System.Threading;
using Zenject;

namespace Assets.Scripts.StateMachine
{
    public abstract class State : IState
    {
        protected ISMContext Context;
        protected CancellationToken Token;

        private CancellationTokenSource _tokenSource;

        protected State(ISMContext context)
        {
            Context = context;
        }

        public UniTask GoTo(IState state, CancellationToken token = default) => Context.GoTo(state, token);
        public void Back(CancellationToken token = default) => Context.Back(token);

        public virtual UniTask Enter(CancellationToken token = default)
        {
            _tokenSource = new CancellationTokenSource();
            token = _tokenSource.Token;

            return UniTask.CompletedTask;
        }

        public abstract UniTask Run(CancellationToken token);
        
        public virtual UniTask Exit()
        {
            _tokenSource.Cancel();
            _tokenSource.Dispose();
            return UniTask.CompletedTask;
        }
    }
}
