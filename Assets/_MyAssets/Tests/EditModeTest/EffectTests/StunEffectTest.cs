using NUnit.Framework;
using Runix.Effect;

namespace Runix.Tests.Effects
{
    public class StunEffectTest
    {
        StunEffect stunEffect;
        readonly EffectContext effectContext = new();


        [Test]
        public void StunEffect_BlokedSlotsCountTest()
        {
            stunEffect = new(2, null, 2);

            Assert.AreEqual(stunEffect.BlockedSlots, 2, "stun effect blocked slots count wrong coming");
        }



        [Test]
        public void StunEffect_TurnCountTest()
        {
            stunEffect = new(2, null, 2);

            stunEffect.OnTurnStart(effectContext);
            stunEffect.OnTurnEnd(effectContext);


            Assert.AreEqual(stunEffect.RemainingTurns, 1, "stun effect blocked slots count wrong coming");
        }
    }
}
