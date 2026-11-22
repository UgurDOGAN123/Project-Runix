using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Runix.Damage;
using Runix.Effect;

namespace Runix.Tests.Effects
{
    public class PoisonEffectTest
    {
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
        public void IlkVurus_BaslangicHasariniVerir()
        {
            var poison = new PoisonEffect(3, 2, 1, Element.Nature, null);

            poison.OnTurnStart(ctx);

            Assert.AreEqual((2, Element.Nature), target.Hits[0]);
        }

        [Test]
        public void HerVurustaHasarArtar()
        {
            var poison = new PoisonEffect(3, 2, 1, Element.Nature, null);

            for (int i = 0; i < 3; i++) poison.OnTurnStart(ctx);

            CollectionAssert.AreEqual(new[] { 2, 3, 4 }, target.Hits.Select(h => h.damage).ToArray());
        }

        [Test]
        public void ArtisSifirsa_SabitHasarVerir()
        {
            var poison = new PoisonEffect(3, 5, 0, Element.Nature, null);

            for (int i = 0; i < 3; i++) poison.OnTurnStart(ctx);

            CollectionAssert.AreEqual(new[] { 5, 5, 5 }, target.Hits.Select(h => h.damage).ToArray());
        }

        [Test]
        public void OnTurnEnd_HasarVermezSadeceSureyiAzaltir()
        {
            var poison = new PoisonEffect(3, 2, 1, Element.Nature, null);

            poison.OnTurnEnd(ctx);

            Assert.IsEmpty(target.Hits);
            Assert.AreEqual(2, poison.RemainingTurns);
            Assert.AreEqual(2, poison.CurrentDamage, "OnTurnEnd hasari artirmamali");
        }

        [Test]
        public void TamSure_ToplamHasarDogruHesaplanir()
        {
            var poison = new PoisonEffect(3, 2, 1, Element.Nature, null);

            int turns = 0;
            while (poison.RemainingTurns > 0 && turns < 10)
            {
                poison.OnTurnStart(ctx);
                poison.OnTurnEnd(ctx);
                turns++;
            }

            Assert.AreEqual(3, turns, "3 tur surmeli");
            Assert.AreEqual(2 + 3 + 4, target.Hits.Sum(h => h.damage));
        }

        [Test]
        public void IkiAyriZehir_BirbirindenBagimsizArtar()
        {
            var a = new PoisonEffect(2, 1, 1, Element.Nature, null);
            var b = new PoisonEffect(2, 10, 0, Element.Nature, null);

            a.OnTurnStart(ctx);
            b.OnTurnStart(ctx);
            a.OnTurnStart(ctx);
            b.OnTurnStart(ctx);

            CollectionAssert.AreEqual(new[] { 1, 10, 2, 10 }, target.Hits.Select(h => h.damage).ToArray());
        }
    }
}
