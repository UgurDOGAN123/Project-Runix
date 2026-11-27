using NUnit.Framework;
using Runix.Effect;

namespace Runix.Tests.Effects
{
    public class WeakenEffectTest
    {
        readonly EffectContext ctx = new EffectContext { OwnerId = 1 };

        [Test]
        public void Reduction_SinirlanirMi()
        {
            Assert.AreEqual(1f, new WeakenEffect(1, 5f, null).Reduction);
            Assert.AreEqual(0f, new WeakenEffect(1, -1f, null).Reduction);
        }

        [Test]
        public void IlkTurBasinda_SureDusmez()
        {
            var e = new WeakenEffect(2, 0.3f, null);
            e.OnTurnStart(ctx);
            Assert.AreEqual(2, e.RemainingTurns);
        }

        [Test]
        public void OnTurnEnd_SureyiDusurmez()
        {
            var e = new WeakenEffect(2, 0.3f, null);
            e.OnTurnStart(ctx);
            e.OnTurnEnd(ctx);
            Assert.AreEqual(2, e.RemainingTurns);
        }

        [Test]
        public void TurSayisiKadarSaldiriEtkilenir()
        {
            // turns = 2: 1. ve 2. sahip turunda hasar hesabında hâlâ aktif, 3. tur başında biter
            var e = new WeakenEffect(2, 0.3f, null);
            e.OnTurnStart(ctx); e.OnTurnEnd(ctx);   // 1. tur
            Assert.Greater(e.RemainingTurns, 0);
            e.OnTurnStart(ctx); e.OnTurnEnd(ctx);   // 2. tur
            Assert.Greater(e.RemainingTurns, 0);
            e.OnTurnStart(ctx);                     // 3. tur başı
            Assert.AreEqual(0, e.RemainingTurns);
        }
    }
}
