using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

namespace Assets.Scripts.Extensions
{
    public static class AnimatorExtensions
    {
        public static async UniTask SetTriggerAsync(this Animator animator, int id, MonoBehaviour monoBehaviour)
        {
            animator.SetTrigger(id);
            
            await UniTask.WaitForEndOfFrame(monoBehaviour);

            await UniTask.WaitUntil(() =>
            {
                AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);
                return currentState.normalizedTime >= 1.0;
            });
        }
    }
}
