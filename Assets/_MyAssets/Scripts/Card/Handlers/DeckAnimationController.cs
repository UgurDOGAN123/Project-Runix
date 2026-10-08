using UnityEngine;

namespace Runix.Card
{
    public class DeckAnimationController : MonoBehaviour
    {
        [SerializeField] Animator myAnimator;

        public void PlayAnim(string trigger, byte myPlayer)
        {
            myAnimator.SetInteger("Player", myPlayer);
            myAnimator.SetTrigger(trigger);
            Destroy(gameObject, 1.5f);
        }
    }
}