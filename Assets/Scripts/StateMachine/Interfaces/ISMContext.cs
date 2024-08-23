using Cysharp.Threading.Tasks;
using System.Threading;

namespace Assets.Scripts.StateMachine
{
    public interface ISMContext
    {
        IState CurrentState { get; }

        UniTask Run(CancellationToken token);

        UniTask GoTo(IState state, CancellationToken token);
        UniTask Back(CancellationToken token);
    }
}
