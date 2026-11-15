using Runix.Character;
using UnityEngine;

namespace Runix.Ability
{
    [CreateAssetMenu(fileName = "New Ability", menuName = "SO/Ability")]
    public abstract class AbilitySO : ScriptableObject
    {
        [Header("Görünüm (seçim ekraný için)")]
        public string displayName;
        [TextArea] public string description;
        public Sprite icon;

        public abstract void UseAbility(AbilityContext abilityContext);
    }
}

