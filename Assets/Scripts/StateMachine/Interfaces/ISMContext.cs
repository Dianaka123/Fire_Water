using Cysharp.Threading.Tasks;
using System.Threading;

namespace Assets.Scripts.StateMachine
{
    public interface ISMContext
    {
        IState CurrentState { get; }

        UniTask Run(CancellationToken token = default);

        UniTask GoTo(IState state, CancellationToken token = default);
        UniTask Back(CancellationToken token = default);
    }
}
