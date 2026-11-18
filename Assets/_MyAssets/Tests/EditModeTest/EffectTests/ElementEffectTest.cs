using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Runix.Damage;
using Runix.Effect;

namespace Runix.Tests.Effects
{
    public class ElementEffectTest
    {
        // HpManager yerine geçen sahte hedef: gelen hasarlarý kaydeder
        class FakeTarget : IDirectDamageable
        {
            public readonly List<(int damage, Element element)> Hits = new();
            public void TakeDirectDamage(int attackValue, Element element = Element.None) =>
                Hits.Add((attackValue, element));
        }

        FakeTarget target;
        EffectContext ctx;

        [SetUp]
        public void SetUp()
        {
            target = new FakeTarget();
            ctx = new EffectContext { Hp = target, OwnerId = 1 };
        }

        [Test]
        public void OnTurnStart_ElementiylaHasarVerir()
        {
            var burn = new ElementEffect(3, 5, Element.Fire, null);

            burn.OnTurnStart(ctx);

            Assert.AreEqual(1, target.Hits.Count);
            Assert.AreEqual((5, Element.Fire), target.Hits[0]);
        }

        [Test]
        public void OnTurnEnd_HasarVermezSadeceSureyiAzaltir()
        {
            var burn = new ElementEffect(3, 5, Element.Fire, null);

            burn.OnTurnEnd(ctx);

            Assert.IsEmpty(target.Hits);
            Assert.AreEqual(2, burn.RemainingTurns);
        }

        [Test]
        public void TamSure_ToplamHasarTurSayisiKadarVurur()
        {
            var burn = new ElementEffect(3, 5, Element.Fire, null);

            // EffectManager sýrasý: sahibinin PlayTurn'ü (OnTurnStart), TurnDamage'ý (OnTurnEnd)
            int turns = 0;
            while (burn.RemainingTurns > 0 && turns < 10)
            {
                burn.OnTurnStart(ctx);
                burn.OnTurnEnd(ctx);
                turns++;
            }

            Assert.AreEqual(3, turns, "3 tur sürmeli");
            Assert.AreEqual(3, target.Hits.Count, "her turda bir kez vurmalý");
            Assert.AreEqual(15, target.Hits.ConvertAll(h => h.damage).Sum());
        }

        [Test]
        public void FarkliElementlerAyriKaydedilir()
        {
            var fire = new ElementEffect(1, 4, Element.Fire, null);
            var water = new ElementEffect(1, 2, Element.Water, null);

            fire.OnTurnStart(ctx);
            water.OnTurnStart(ctx);

            Assert.AreEqual((4, Element.Fire), target.Hits[0]);
            Assert.AreEqual((2, Element.Water), target.Hits[1]);
        }
    }
}