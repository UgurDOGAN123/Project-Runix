using UnityEngine;

namespace Runix.Animation
{
    public class AnimationHandler : MonoBehaviour
    {
        [SerializeField] Animator animator;

        void Start()
        {
            animator.SetInteger("Player", (TurnControlSystem.TurnControlManager.instance.CurrentPlayer - 1));
        }
    }
}
