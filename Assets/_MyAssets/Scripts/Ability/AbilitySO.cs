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
        [SerializeField] protected AnimationClip animationClip;

        public abstract void UseAbility(AbilityContext abilityContext);
        private void OnValidate()
        {
            if (string.IsNullOrWhiteSpace(displayName))
                Debug.LogWarning($"{name} isim yok.", this);
            if (string.IsNullOrWhiteSpace(description))
                Debug.LogWarning($"{name} açýklama yok.", this);
        }

    }
}

