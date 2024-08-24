using Assets.Scripts.StateMachine;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace Assets.Scripts.States
{
    public class EmptyState : State
    {
        public EmptyState(ISMContext context) : base(context)
        {
        }

        public override UniTask Run(CancellationToken token)
        {
            return UniTask.CompletedTask;
        }
    }
}
