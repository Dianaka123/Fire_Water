using Assets.Scripts.Managers;
using Assets.Scripts.StateMachine;
using Assets.Scripts.States.Contexts;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace Assets.Scripts.States
{
    public class BalloonsState : State
    {
        private readonly BalloonManager _balloonManager;

        public BalloonsState(BallonSM context, BalloonManager balloonManager) : base(context)
        {
            _balloonManager = balloonManager;
        }

        public override UniTask Enter(CancellationToken token = default)
        {
            _balloonManager.SpawnBallons();
            return base.Enter(token);
        }

        public override UniTask Run(CancellationToken token)
        {
            _balloonManager.MoveBalloonsBySin();
            return UniTask.CompletedTask;
        }
    }
}
