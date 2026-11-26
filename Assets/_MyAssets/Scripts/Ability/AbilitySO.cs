using Runix.Character;
using UnityEngine;

namespace Runix.Ability
{
    [CreateAssetMenu(fileName = "New Ability", menuName = "SO/Ability")]
    public abstract class AbilitySO : ScriptableObject
    {
        [Header("Görünüm (seçim ekranı için)")]
        public string displayName;
        [TextArea] public string description;
        public Sprite icon;
        [SerializeField] protected AnimationClip animationClip;

        [Header("Kullanım")]
        [SerializeField, Min(0)] int cooldownTurns = 3;   // kullanıldıktan kaç tur sonra tekrar kullanılabilir (3: her 3. turda)
        public int CooldownTurns => cooldownTurns;

        // Şu an kullanılabilir mi? (örn. temizlenecek efekt yoksa Arınma kullanılamaz)
        public virtual bool CanUse(AbilityContext abilityContext) => true;

        public abstract void UseAbility(AbilityContext abilityContext);
        private void OnValidate()
        {
            if (string.IsNullOrWhiteSpace(displayName))
                Debug.LogWarning($"{name} isim yok.", this);
            if (string.IsNullOrWhiteSpace(description))
                Debug.LogWarning($"{name} açıklama yok.", this);
        }

    }
}

