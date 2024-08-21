using UnityEngine;

namespace Assets.Scripts.Views
{
    [RequireComponent(typeof(Animator))]
    public class Block : MonoBehaviour
    {
        [SerializeField]
        private Animator _animator;

    }
}