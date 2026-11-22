using System.Reflection;
using NUnit.Framework;
using Runix.Damage;
using Runix.Effect;
using UnityEngine;

namespace Runix.Tests.PlayMode
{
    public class EffectManagerTest
    {
        const int Me = 0;
        const int Enemy = 1;

        GameObject go;
        TestEffectManager effectManager;
        FakeHpManager hpManager;
        EffectContext effectContext;

        // TurnCalculator protected oldugu icin testten erismek icin alt sinif
        class TestEffectManager : EffectManager
        {
            public void Advance(TurnEnum stage, int player, EffectContext context)
            {
                TurnCalculator(stage, player, context);
            }
        }

        class FakeHpManager : IDirectDamageable
        {
            public int hp = 100;
            public void TakeDirectDamage(int attackValue, Element element = Element.None)
            {
                hp -= attackValue;
            }
        }

        [SetUp]
        public void SetUp()
        {
            hpManager = new FakeHpManager { hp = 100 };
            effectContext = new EffectContext { OwnerId = Me, Hp = hpManager };

            go = new GameObject("TestObject");
            go.SetActive(false);                       // Awake, alanlar ayarlanana kadar calismasin
            effectManager = go.AddComponent<TestEffectManager>();
            SetField("ownerId", Me);
            SetField("debugLog", false);
            go.SetActive(true);
        }

        [TearDown]
        public void TearDown()
        {
            if (go != null) Object.DestroyImmediate(go);
        }

        void SetField(string name, object value) =>
            typeof(EffectManager)
                .GetField(name, BindingFlags.NonPublic | BindingFlags.Instance)
                .SetValue(effectManager, value);

        // Sadece turu ilerletir; kontrolleri her test kendisi yazar
        void Advance(TurnEnum stage, int player = Me) =>
            effectManager.Advance(stage, player, effectContext);

        void AssertState(string step, int count, int hp, int blockedSlots)
        {
            Assert.AreEqual(count, effectManager.Effects.Count, $"{step}: efekt sayisi");
            Assert.AreEqual(hp, hpManager.hp, $"{step}: hp");
            Assert.AreEqual(blockedSlots, effectManager.BlockedSlotCount, $"{step}: kilitli slot");
        }

        // ---------------- Ekleme ----------------

        [Test]
        public void Add_Null_EfektEklenmez()
        {
            effectManager.Add(null);
            Assert.AreEqual(0, effectManager.Effects.Count);
        }

        [Test]
        public void Add_NullIkonla_HataVermez()
        {
            Assert.DoesNotThrow(() => effectManager.Add(new StunEffect(1, null, 2)));
            Assert.AreEqual(1, effectManager.Effects.Count);
        }

        // ---------------- Sersemletme ----------------

        [Test]
        public void Stun_IkiTur_SlotlariTamSureKilitler()
        {
            effectManager.Add(new StunEffect(2, null, 4));

            Advance(TurnEnum.PlayTurn);
            AssertState("1. PlayTurn", 1, 100, 4);
            Assert.AreEqual(2, effectManager.Effects[0].RemainingTurns, "1. PlayTurn: PlayTurn sureyi azaltmamali");

            Advance(TurnEnum.TurnDamage);
            AssertState("1. TurnDamage", 1, 100, 4);
            Assert.AreEqual(1, effectManager.Effects[0].RemainingTurns, "1. TurnDamage: sure 1'e dusmeli");

            Advance(TurnEnum.PlayTurn);
            AssertState("2. PlayTurn", 1, 100, 4);

            Advance(TurnEnum.TurnDamage);
            AssertState("2. TurnDamage", 0, 100, 0);
        }

        [Test]
        public void Stun_TekTur_IlkTurnDamagedeSilinir()
        {
            effectManager.Add(new StunEffect(1, null, 3));

            Advance(TurnEnum.PlayTurn);
            AssertState("PlayTurn", 1, 100, 3);

            Advance(TurnEnum.TurnDamage);
            AssertState("TurnDamage", 0, 100, 0);
        }

        [Test]
        public void Stun_IkiFarkliSure_KilitliSlotKademeliAzalir()
        {
            effectManager.Add(new StunEffect(2, null, 2));
            effectManager.Add(new StunEffect(1, null, 2));

            Advance(TurnEnum.PlayTurn);
            AssertState("1. PlayTurn", 2, 100, 4);
            Assert.AreEqual(2, effectManager.Effects[0].RemainingTurns, "1. PlayTurn: ilk efekt suresi");
            Assert.AreEqual(1, effectManager.Effects[1].RemainingTurns, "1. PlayTurn: ikinci efekt suresi");

            Advance(TurnEnum.TurnDamage);
            AssertState("1. TurnDamage", 1, 100, 2);
            Assert.AreEqual(1, effectManager.Effects[0].RemainingTurns, "1. TurnDamage: kalan efekt suresi");

            Advance(TurnEnum.PlayTurn);
            AssertState("2. PlayTurn", 1, 100, 2);

            Advance(TurnEnum.TurnDamage);
            AssertState("2. TurnDamage", 0, 100, 0);
        }

        // ---------------- Element (yanma) ----------------

        [Test]
        public void Element_HasariPlayTurndeVurur_SureTurnDamagedeAzalir()
        {
            effectManager.Add(new ElementEffect(2, 5, Element.Fire, null));

            Advance(TurnEnum.PlayTurn);
            AssertState("1. PlayTurn", 1, 95, 0);

            Advance(TurnEnum.TurnDamage);
            AssertState("1. TurnDamage", 1, 95, 0);
            Assert.AreEqual(1, effectManager.Effects[0].RemainingTurns, "1. TurnDamage: sure");

            Advance(TurnEnum.PlayTurn);
            AssertState("2. PlayTurn", 1, 90, 0);

            Advance(TurnEnum.TurnDamage);
            AssertState("2. TurnDamage", 0, 90, 0);
        }

        [Test]
        public void Element_IkiEfekt_HasarlarToplanir()
        {
            effectManager.Add(new ElementEffect(2, 5, Element.Fire, null));
            effectManager.Add(new ElementEffect(2, 5, Element.Fire, null));

            Advance(TurnEnum.PlayTurn);
            AssertState("1. PlayTurn", 2, 90, 0);

            Advance(TurnEnum.TurnDamage);
            AssertState("1. TurnDamage", 2, 90, 0);

            Advance(TurnEnum.PlayTurn);
            AssertState("2. PlayTurn", 2, 80, 0);

            Advance(TurnEnum.TurnDamage);
            AssertState("2. TurnDamage", 0, 80, 0);
        }

        [Test]
        public void Element_TekTur_BirKereVurupSilinir()
        {
            effectManager.Add(new ElementEffect(1, 5, Element.Fire, null));

            Advance(TurnEnum.PlayTurn);
            Advance(TurnEnum.TurnDamage);

            AssertState("1 turluk efekt sonrasi", 0, 95, 0);
        }

        [Test]
        public void ElementVeStun_BirlikteBagimsizCalisir()
        {
            effectManager.Add(new ElementEffect(2, 5, Element.Fire, null));
            effectManager.Add(new StunEffect(2, null, 4));

            Advance(TurnEnum.PlayTurn);
            AssertState("1. PlayTurn", 2, 95, 4);

            Advance(TurnEnum.TurnDamage);
            AssertState("1. TurnDamage", 2, 95, 4);

            Advance(TurnEnum.PlayTurn);
            AssertState("2. PlayTurn", 2, 90, 4);

            Advance(TurnEnum.TurnDamage);
            AssertState("2. TurnDamage", 0, 90, 0);
        }

        // ---------------- Tur sahibi kontrolu ----------------

        [Test]
        public void RakipPlayTurn_EfektTetiklenmez()
        {
            effectManager.Add(new ElementEffect(2, 5, Element.Fire, null));

            Advance(TurnEnum.PlayTurn, Enemy);

            AssertState("rakip PlayTurn", 1, 100, 0);
        }

        [Test]
        public void RakipTurnDamage_SureAzalmaz()
        {
            effectManager.Add(new ElementEffect(2, 5, Element.Fire, null));

            Advance(TurnEnum.PlayTurn, Enemy);
            Advance(TurnEnum.TurnDamage, Enemy);

            AssertState("rakip turu", 1, 100, 0);
            Assert.AreEqual(2, effectManager.Effects[0].RemainingTurns, "rakip turu sureyi azaltmamali");
        }

        // ---------------- Arada eklenen efekt ----------------

        [Test]
        public void PlayTurnIleTurnDamageArasindaEklenen_AyniTurdaHasarVurmaz_SureAzalir()
        {
            Advance(TurnEnum.PlayTurn);
            effectManager.Add(new ElementEffect(2, 5, Element.Fire, null));

            Advance(TurnEnum.TurnDamage);

            AssertState("arada eklenen", 1, 100, 0);
            Assert.AreEqual(1, effectManager.Effects[0].RemainingTurns, "arada eklenen efektin suresi");
        }

        // ---------------- Savas basi / sonu ----------------

        [Test]
        public void ClearAll_EfektleriVeKilidiTemizler()
        {
            effectManager.Add(new ElementEffect(2, 5, Element.Fire, null));
            effectManager.Add(new StunEffect(2, null, 4));

            effectManager.ClearAll();

            AssertState("ClearAll", 0, 100, 0);
        }

        [Test]
        public void BattleStart_EfektleriVeKilidiTemizler()
        {
            effectManager.Add(new ElementEffect(2, 5, Element.Fire, null));
            effectManager.Add(new StunEffect(2, null, 4));

            Advance(TurnEnum.BattleStart);

            AssertState("BattleStart", 0, 100, 0);
        }

        [Test]
        public void BattleEnd_EfektleriVeKilidiTemizler()
        {
            effectManager.Add(new ElementEffect(2, 5, Element.Fire, null));
            effectManager.Add(new StunEffect(2, null, 4));

            Advance(TurnEnum.BattleEnd);

            AssertState("BattleEnd", 0, 100, 0);
        }

        [Test]
        public void BattleStart_RakipIcinDeTemizler()
        {
            effectManager.Add(new StunEffect(2, null, 4));

            Advance(TurnEnum.BattleStart, Enemy);

            AssertState("rakip BattleStart", 0, 100, 0);
        }
    }
}