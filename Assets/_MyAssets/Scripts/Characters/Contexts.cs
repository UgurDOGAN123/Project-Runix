using Runix.Effect;
using UnityEngine;


namespace Runix.Character
{
    [System.Serializable]
    public struct PassiveDefense
    {
        public Element element;
        [Range(0, 100)] public int Value;
    }

    // Karakter yeteneğinin sahneye erişim yolu
    public class AbilityContext
    {
        public int OwnerId;
        public EffectManager Own;       // yeteneği kullananın etkileri
        public EffectManager Opponent;  // rakibin etkileri
    }
}
