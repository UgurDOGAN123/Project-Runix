using UnityEngine;

namespace Runix.Animation
{
    public class AnimationHandler : MonoBehaviour
    {
        [SerializeField] Animator animator;
        [SerializeField] TurnEnum turn;

        void Start()
        {
            print((TurnControlSystem.TurnControlManager.instance.CurrentPlayer - 1));
            animator.SetInteger("Player", (TurnControlSystem.TurnControlManager.instance.CurrentPlayer - 1));
        }
    }
}
