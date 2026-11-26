using Runix.Ability;
using Runix.Character;
using UnityEngine;

namespace Runix.UI
{
    public class AbilityAnimPlayer : MonoBehaviour
    {
        [SerializeField] AbilityRunner abilityRunner;
        [SerializeField] GameObject AbilityAnimObject;

        private void OnEnable()
        {
            abilityRunner.Used += CreateAnim;
        }
        private void OnDisable()
        {
            abilityRunner.Used -= CreateAnim;
        }

        private void CreateAnim(AbilitySO obj)
        {
            var abilityAnim = Instantiate(AbilityAnimObject, Vector3.zero, Quaternion.identity);
            abilityAnim.GetComponent<UnityEngine.Animation>().AddClip(obj.animationClip, obj.name);
            abilityAnim.GetComponent<UnityEngine.Animation>().Play(obj.name);
            abilityAnim.GetComponent<SpriteRenderer>().sprite = obj.icon;
            Destroy(abilityAnim, obj.animationClip.length);
        }
    }
}
