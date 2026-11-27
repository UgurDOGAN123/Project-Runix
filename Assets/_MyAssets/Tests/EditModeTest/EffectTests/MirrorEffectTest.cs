using NUnit.Framework;
using Runix.Effect;

namespace Runix.Tests.Effects
{
    public class MirrorEffectTest
    {
        [Test]
        public void Reflect_YuzdeKadarYansitir()
        {
            var m = new MirrorEffect(2, 0.5f, null);
            Assert.AreEqual(50, m.Reflect(100));
        }

        [Test]
        public void Reflect_SifirVeNegatifHasar_Sifir()
        {
            var m = new MirrorEffect(2, 0.5f, null);
            Assert.AreEqual(0, m.Reflect(0));
            Assert.AreEqual(0, m.Reflect(-10));
        }

        [Test]
        public void Ratio_SinirlanirMi()
        {
            Assert.AreEqual(1f, new MirrorEffect(1, 3f, null).Ratio);
            Assert.AreEqual(0f, new MirrorEffect(1, -1f, null).Ratio);
        }

        [Test]
        public void SureHerTurSonundaDuser()
        {
            var m = new MirrorEffect(2, 0.5f, null);
            var ctx = new EffectContext { OwnerId = 0 };
            m.OnTurnEnd(ctx);
            Assert.AreEqual(1, m.RemainingTurns);
        }
    }
}
