using UnityEngine;

namespace Runix.Animation
{
    public class AnimationHandler : MonoBehaviour
    {
        [SerializeField] Animator animator;

        void Start()
        {
            if (animator.parameterCount != 0)
                animator.SetInteger("Player", (TurnControlSystem.TurnControlManager.instance.CurrentPlayer));
        }
    }
}
