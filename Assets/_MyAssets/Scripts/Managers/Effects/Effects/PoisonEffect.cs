using Runix.Damage;
using UnityEngine;

namespace Runix.Effect
{
    /// <summary>
    /// Zehir: sahibinin her PlayTurn başında hasar verir ve her vuruştan sonra bir sonraki hasar artar.
    /// Örn. baslangic 2, artis 1: 2, 3, 4, ... Aktif savunmayı yok sayar, sadece pasif savunma azaltır.
    /// </summary>
    public class PoisonEffect : Effect
    {
        readonly Element element;
        readonly int increasePerTick;
        int currentDamage;

        public int CurrentDamage => currentDamage;   // testler ve ileride UI için

        public PoisonEffect(int turns, int startDamage, int increasePerTick, Element element, Sprite icon)
            : base(turns, icon)
        {
            currentDamage = startDamage;
            this.increasePerTick = increasePerTick;
            this.element = element;
        }

        public override void OnTurnStart(EffectContext ctx)
        {
            ctx.Hp.TakeDirectDamage(currentDamage, element);
            currentDamage += increasePerTick;     // bir sonraki vuruş daha güçlü
        }
    }
}
