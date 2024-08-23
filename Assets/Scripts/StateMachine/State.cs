using Cysharp.Threading.Tasks;
using System.Threading;

namespace Assets.Scripts.StateMachine
{
    public abstract class State : IState
    {
        protected ISMContext Context;

        protected State(ISMContext context)
        {
            Context = context;
        }

        public UniTask GoTo(IState state, CancellationToken token) => Context.GoTo(state, token);
        public void Back(CancellationToken token) => Context.Back(token);

        public virtual UniTask Enter(CancellationToken token) => UniTask.CompletedTask;
        public abstract UniTask Run(CancellationToken token);
        public virtual UniTask Exit(CancellationToken token) => UniTask.CompletedTask;
    }
}
