using System.Collections.Generic;
using NUnit.Framework;
using Runix.Ability;
using Runix.Card;
using Runix.Character;
using UnityEngine;

namespace Runix.Tests
{
    public class LoadoutTests
    {
        List<Object> created;

        [SetUp] public void SetUp() => created = new List<Object>();

        [TearDown]
        public void TearDown()
        {
            foreach (var o in created) if (o != null) Object.DestroyImmediate(o);
            created.Clear();
        }

        // ---------- Yardımcılar ----------

        T Track<T>(T o) where T : Object { created.Add(o); return o; }

        CardSO Card() => Track(ScriptableObject.CreateInstance<Attack>());

        DeckSO Deck(DeckKind kind, Element element, params CardSO[] cards)
        {
            var d = Track(ScriptableObject.CreateInstance<DeckSO>());
            d.kind = kind;
            d.element = element;
            d.cards = new List<CardSO>(cards);
            d.healable = new Element[0];
            d.passiveDefense = new PassiveDefense[0];
            return d;
        }

        static PlayerLoadout Loadout(DeckSO element, DeckSO special) =>
            new PlayerLoadout("test", "test", element, special, null, default, default);

        // Sıralamayı biz belirlemek için (PlayerLoadout hep element → özel sıralar)
        class FakeLoadout : ICharaterLoadout
        {
            public string Name => "fake";
            public string Description => "fake";
            public IReadOnlyList<DeckSO> DeckSO { get; set; } = new List<DeckSO>();
            public AbilitySO Ability => null;
            public HpValues HpValues => default;
            public DeckLimits DeckLimits => default;
        }

        // ---------- BuildCards ----------

        [Test]
        public void BuildCards_ElementVeOzelDesteyiBirlestirir()
        {
            CardSO a = Card(), b = Card(), c = Card();
            var loadout = Loadout(Deck(DeckKind.Element, Element.Fire, a, b), Deck(DeckKind.Special, Element.None, c));

            CollectionAssert.AreEquivalent(new[] { a, b, c }, loadout.BuildCards());
        }

        [Test]
        public void BuildCards_OzelDesteYoksaSadeceElementKartlari()
        {
            var a = Card();
            var loadout = Loadout(Deck(DeckKind.Element, Element.Fire, a), null);

            CollectionAssert.AreEqual(new[] { a }, loadout.BuildCards());
        }

        [Test]
        public void BuildCards_HicDesteYoksa_BosListeDonerHataVermez()
        {
            Assert.IsEmpty(Loadout(null, null).BuildCards());
        }

        [Test]
        public void BuildCards_KartListesiNullSaMesela_Atlanir()
        {
            var a = Card();
            var broken = Deck(DeckKind.Special, Element.None);
            broken.cards = null;
            var loadout = Loadout(Deck(DeckKind.Element, Element.Fire, a), broken);

            Assert.AreEqual(1, loadout.BuildCards().Count);
        }

        [Test]
        public void BuildCards_BosElemanAtlanir()
        {
            var a = Card();
            var loadout = Loadout(Deck(DeckKind.Element, Element.Fire, a, null), null);

            Assert.AreEqual(1, loadout.BuildCards().Count);
        }

        [Test]
        public void BuildCards_AyniKartIkiDestedeyse_IkisiDeKalir()
        {
            var a = Card();
            var loadout = Loadout(Deck(DeckKind.Element, Element.Fire, a), Deck(DeckKind.Special, Element.None, a));

            Assert.AreEqual(2, loadout.BuildCards().Count);
        }

        [Test]
        public void BuildCards_HerCagridaYeniListeDoner()
        {
            var loadout = Loadout(Deck(DeckKind.Element, Element.Fire, Card()), null);

            var first = loadout.BuildCards();
            first.Clear();   // dışarıdan değiştirilirse asıl deste bozulmamalı

            Assert.AreEqual(1, loadout.BuildCards().Count);
        }

        [Test]
        public void Uzantilar_NullLoadoutIcinHataVermez()
        {
            ICharaterLoadout none = null;

            Assert.IsEmpty(none.BuildCards());
            Assert.IsEmpty(none.GetHealable());
            Assert.IsEmpty(none.GetPassiveDefense());
            Assert.AreEqual(Element.None, none.GetPrimaryElement());
        }

        // ---------- GetHealable ----------

        [Test]
        public void GetHealable_DesteleriBirlestirirTekrarsiz()
        {
            var fire = Deck(DeckKind.Element, Element.Fire, Card());
            fire.healable = new[] { Element.Fire, Element.Nature };
            var special = Deck(DeckKind.Special, Element.None, Card());
            special.healable = new[] { Element.Nature, Element.Water };

            var healable = Loadout(fire, special).GetHealable();

            CollectionAssert.AreEquivalent(new[] { Element.Fire, Element.Nature, Element.Water }, healable);
        }

        [Test]
        public void GetHealable_HealableNullsaAtlanir()
        {
            var fire = Deck(DeckKind.Element, Element.Fire, Card());
            fire.healable = null;

            Assert.IsEmpty(Loadout(fire, null).GetHealable());
        }

        // ---------- GetPassiveDefense ----------

        [Test]
        public void GetPassiveDefense_AyniElementiToplar()
        {
            var fire = Deck(DeckKind.Element, Element.Fire, Card());
            fire.passiveDefense = new[] { new PassiveDefense { element = Element.Fire, Value = 20 } };
            var special = Deck(DeckKind.Special, Element.None, Card());
            special.passiveDefense = new[] { new PassiveDefense { element = Element.Fire, Value = 15 } };

            var result = Loadout(fire, special).GetPassiveDefense();

            Assert.AreEqual(35, result[Element.Fire]);
        }

        [Test]
        public void GetPassiveDefense_FarkliElementleriAyriTutar()
        {
            var fire = Deck(DeckKind.Element, Element.Fire, Card());
            fire.passiveDefense = new[]
            {
                new PassiveDefense { element = Element.Fire, Value = 20 },
                new PassiveDefense { element = Element.Water, Value = 10 },
            };

            var result = Loadout(fire, null).GetPassiveDefense();

            Assert.AreEqual(20, result[Element.Fire]);
            Assert.AreEqual(10, result[Element.Water]);
            Assert.AreEqual(2, result.Count);
        }

        [Test]
        public void GetPassiveDefense_NullDizideHataVermez()
        {
            var fire = Deck(DeckKind.Element, Element.Fire, Card());
            fire.passiveDefense = null;

            Assert.IsEmpty(Loadout(fire, null).GetPassiveDefense());
        }

        // ---------- GetPrimaryElement ----------

        [Test]
        public void GetPrimaryElement_ElementDestesininElementiniVerir()
        {
            var loadout = Loadout(Deck(DeckKind.Element, Element.Fire, Card()), Deck(DeckKind.Special, Element.Water, Card()));

            Assert.AreEqual(Element.Fire, loadout.GetPrimaryElement());
        }

        [Test]
        public void GetPrimaryElement_OzelDesteListedeOnceGelseDeElementDestesiSecilir()
        {
            var fake = new FakeLoadout
            {
                DeckSO = new List<DeckSO>
                {
                    Deck(DeckKind.Special, Element.Water, Card()),   // önce özel
                    Deck(DeckKind.Element, Element.Fire, Card()),
                }
            };

            Assert.AreEqual(Element.Fire, fake.GetPrimaryElement());
        }

        [Test]
        public void GetPrimaryElement_ElementDestesiYoksa_None()
        {
            var loadout = Loadout(null, Deck(DeckKind.Special, Element.Water, Card()));

            Assert.AreEqual(Element.None, loadout.GetPrimaryElement());
        }

        // ---------- PlayerLoadout ----------

        [Test]
        public void PlayerLoadout_DeckSO_ElementOnceOzelSonra_BoslariAtlar()
        {
            var element = Deck(DeckKind.Element, Element.Fire, Card());
            var special = Deck(DeckKind.Special, Element.None, Card());

            CollectionAssert.AreEqual(new[] { element, special }, Loadout(element, special).DeckSO);
            CollectionAssert.AreEqual(new[] { element }, Loadout(element, null).DeckSO);
            Assert.IsEmpty(Loadout(null, null).DeckSO);
        }

        [Test]
        public void PlayerLoadout_ConstructorDegerleriniAcigaVurur()
        {
            var hp = new HpValues { maxHp = 800, maxDefense = 60 };
            var limits = new DeckLimits { MaxHandCardCount = 8, StartCardCount = 6, EveryTurnAddCardCount = 4 };
            var loadout = new PlayerLoadout("Ugur", "aciklama", null, null, null, hp, limits);

            Assert.AreEqual("Ugur", loadout.Name);
            Assert.AreEqual("aciklama", loadout.Description);
            Assert.AreEqual(800, loadout.HpValues.maxHp);
            Assert.AreEqual(60, loadout.HpValues.maxDefense);
            Assert.AreEqual(8, loadout.DeckLimits.MaxHandCardCount);
            Assert.AreEqual(6, loadout.DeckLimits.StartCardCount);
            Assert.AreEqual(4, loadout.DeckLimits.EveryTurnAddCardCount);
            Assert.IsNull(loadout.Ability);
        }
    }
}