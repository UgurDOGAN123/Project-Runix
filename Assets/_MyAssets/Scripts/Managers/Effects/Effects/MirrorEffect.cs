using UnityEngine;

namespace Runix.Effect
{
    /// <summary>
    /// Yansıma: sahibinin kart saldırılarından aldığı hasarın bir yüzdesi saldırana doğrudan hasar olarak geri döner.
    /// "turns" = sahibin, rakibin kaç saldırısına karşı korunacağı (süre kendi tur sonunda düşer).
    /// Zehir, vampir gibi doğrudan hasarlar yansıtılmaz.
    /// </summary>
    public class MirrorEffect : Effect
    {
        readonly float ratio;   // 0..1

        public float Ratio => ratio;

        public MirrorEffect(int turns, float ratio, Sprite icon) : base(turns, icon)
        {
            this.ratio = Mathf.Clamp01(ratio);
        }

        /// <summary>Alınan hasardan geri yansıyacak miktar.</summary>
        public int Reflect(int damageTaken) =>
            damageTaken <= 0 ? 0 : Mathf.RoundToInt(damageTaken * ratio);
    }
}
