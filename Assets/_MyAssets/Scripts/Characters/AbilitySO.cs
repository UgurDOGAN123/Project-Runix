using Runix.Character;
using UnityEngine;

namespace Runix.Ability
{
    public abstract class AbilitySO : MonoBehaviour
    {
        [Header("Görünüm (seçim ekraný için)")]
        public string displayName;
        [TextArea] public string description;
        public Sprite icon;

        public abstract void UseAbility(AbilityContext abilityContext);
    }
}

