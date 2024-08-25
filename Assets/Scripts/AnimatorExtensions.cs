using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

namespace Assets.Scripts
{
    public static class AnimatorExtensions
    {
        public static async UniTask SetTriggerAsync(this Animator animator, int id, CancellationToken cancellationToken = default)
        {
            animator.SetTrigger(id);
            
            await UniTask.Yield(cancellationToken);

            await UniTask.WaitUntil(() =>
            {
                AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);
                return currentState.normalizedTime >= 1.0;
            });
        }
    }
}
