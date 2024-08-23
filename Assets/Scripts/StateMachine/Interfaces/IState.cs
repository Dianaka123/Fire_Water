using Cysharp.Threading.Tasks;
using System;
using System.Threading;

namespace Assets.Scripts.StateMachine
{
    public interface IState
    {
        UniTask Enter(CancellationToken token);

        UniTask Run(CancellationToken token);

        UniTask Exit(CancellationToken token);
    }
}
