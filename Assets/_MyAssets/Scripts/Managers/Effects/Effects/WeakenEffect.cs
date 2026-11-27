using UnityEngine;

namespace Runix.Effect
{
    /// <summary>
    /// Zayıflatma: sahibinin kart saldırılarını yüzde olarak azaltır (zehir, vampir gibi doğrudan hasarlar etkilenmez).
    /// "turns" = sahibinin kaç saldırısının zayıflatılacağı.
    /// Hasar, TurnDamage'de efektler güncellendikten SONRA hesaplandığı için süre OnTurnEnd'de değil,
    /// bir sonraki PlayTurn başında düşer. Böylece son saldırı da zayıflamış olur.
    /// </summary>
    public class WeakenEffect : Effect
    {
        readonly float reduction;     // 0..1
        bool armed;                   // ilk PlayTurn başlangıcında süre düşmez

        public float Reduction => reduction;

        public WeakenEffect(int turns, float reduction, Sprite icon) : base(turns, icon)
        {
            this.reduction = Mathf.Clamp01(reduction);
        }

        public override void OnTurnStart(EffectContext ctx)
        {
            if (armed) RemainingTurns--;
            armed = true;
        }

        public override void OnTurnEnd(EffectContext ctx) { }   // süre OnTurnStart'ta düşer
    }
}
