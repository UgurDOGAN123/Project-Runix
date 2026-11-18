using NUnit.Framework;
using Runix.Effect;

namespace Runix.Tests.Effects
{
    public class StunEffectTest
    {
        readonly EffectContext ctx = new();   // Stun HP'ye dokunmaz, boþ baðlam yeter

        [Test]
        public void BlockedSlots_VerilenDegerdir()
        {
            var stun = new StunEffect(2, null, 2);

            Assert.AreEqual(2, stun.BlockedSlots);
        }

        [Test]
        public void BlockedSlots_VarsayilanTumSlotlar()
        {
            var stun = new StunEffect(1, null);

            Assert.AreEqual(StunEffect.AllSlots, stun.BlockedSlots);
        }

        [Test]
        public void OnTurnStart_SureyiAzaltmaz()
        {
            var stun = new StunEffect(2, null, 2);

            stun.OnTurnStart(ctx);

            Assert.AreEqual(2, stun.RemainingTurns);
        }

        [Test]
        public void OnTurnEnd_SureyiBirAzaltir()
        {
            var stun = new StunEffect(2, null, 2);

            stun.OnTurnStart(ctx);
            stun.OnTurnEnd(ctx);

            Assert.AreEqual(1, stun.RemainingTurns);
        }

        [Test]
        public void NTurSonra_SureBitmisOlur()
        {
            var stun = new StunEffect(2, null, 2);

            stun.OnTurnEnd(ctx);
            Assert.Greater(stun.RemainingTurns, 0, "1. turdan sonra hâlâ aktif olmalý");

            stun.OnTurnEnd(ctx);
            Assert.LessOrEqual(stun.RemainingTurns, 0, "2. turdan sonra bitmiþ olmalý (EffectManager bunu siler)");
        }

    }
}