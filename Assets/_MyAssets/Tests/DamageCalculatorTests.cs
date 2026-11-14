using System.Collections.Generic;
using NUnit.Framework;
using Runix.Damage;

namespace Runix.Tests
{
    public class DamageCalculatorTests
    {
        private DamageCalculator _calc;

        [SetUp]
        public void SetUp() => _calc = new DamageCalculator();

        private static Dictionary<Element, int> D(Element e, int v) => new() { { e, v } };
        private static Dictionary<Element, int> Empty() => new();

        // ---------- TakeDamage ----------

        [Test]
        public void TakeDamage_CarpanKareyleCarpilir()
        {
            // 10 * (2*2) = 40
            int hp = _calc.TakeDamage(D(Element.Fire, 10), D(Element.Fire, 2), Empty(), Empty(), 100);
            Assert.AreEqual(60, hp);
        }

        [Test]
        public void TakeDamage_SavunmaHasariAzaltirVeTuketilir()
        {
            var defense = D(Element.Fire, 50);
            int hp = _calc.TakeDamage(D(Element.Fire, 10), D(Element.Fire, 2), defense, Empty(), 100);

            Assert.AreEqual(80, hp, "40 hasarýn %50'si = 20");
            Assert.IsFalse(defense.ContainsKey(Element.Fire), "Kullanýlan savunma silinmeli.");
        }

        [Test]
        public void TakeDamage_SavunmaPlusPasifSavunmaYuzuAsinca_HasarSifirOlmali()
        {
            int hp = _calc.TakeDamage(D(Element.Fire, 10), D(Element.Fire, 2),
                D(Element.Fire, 80), D(Element.Fire, 50), 100);
            Assert.AreEqual(100, hp, "Negatif hasar (iyileþme) olmamalý.");
        }

        [Test]
        public void TakeDamage_PasifSavunmaTuketilmez()
        {
            var passive = D(Element.Fire, 50);
            _calc.TakeDamage(D(Element.Fire, 10), D(Element.Fire, 1), Empty(), passive, 100);
            Assert.AreEqual(50, passive[Element.Fire]);
        }

        [Test]
        public void TakeDamage_BaskaElementinSavunmasiKorunur()
        {
            var defense = D(Element.Water, 30);
            int hp = _calc.TakeDamage(D(Element.Fire, 10), D(Element.Fire, 1), defense, Empty(), 100);

            Assert.AreEqual(90, hp, "Su savunmasý ateþ hasarýný azaltmaz.");
            Assert.AreEqual(30, defense[Element.Water], "Kullanýlmayan savunma silinmemeli.");
        }

        [Test]
        public void TakeDamage_CarpanYoksaHasarSifir_MevcutDavranisKaydi()
        {
            int hp = _calc.TakeDamage(D(Element.Fire, 10), Empty(), Empty(), Empty(), 100);
            Assert.AreEqual(100, hp);
        }

        [Test]
        public void TakeDamage_BirdenCokElementToplanir()
        {
            var attack = new Dictionary<Element, int> { { Element.Fire, 10 }, { Element.Water, 5 } };
            var mult = new Dictionary<Element, int> { { Element.Fire, 1 }, { Element.Water, 2 } };
            int hp = _calc.TakeDamage(attack, mult, Empty(), Empty(), 100);
            Assert.AreEqual(70, hp, "10 + 5*4 = 30");
        }

        // ---------- TakeDirectDamage ----------

        [Test]
        public void TakeDirectDamage_PasifSavunmaUygulanir()
        {
            int hp = _calc.TakeDirectDamage(10, Element.Fire, D(Element.Fire, 50), 100);
            Assert.AreEqual(95, hp);
        }

        [Test]
        public void TakeDirectDamage_PasifSavunmaYoksaTamHasar()
        {
            int hp = _calc.TakeDirectDamage(10, Element.Fire, Empty(), 100);
            Assert.AreEqual(90, hp);
        }

        [Test]
        public void TakeDirectDamage_PasifSavunmaYuzuAsinca_HasarSifir()
        {
            int hp = _calc.TakeDirectDamage(10, Element.Fire, D(Element.Fire, 150), 100);
            Assert.AreEqual(100, hp);
        }

        // ---------- AddDefense ----------

        [Test]
        public void AddDefense_AyniElementToplanir()
        {
            var result = _calc.AddDefense(D(Element.Fire, 20), D(Element.Fire, 30), 100);
            Assert.AreEqual(50, result[Element.Fire]);
        }

        [Test]
        public void AddDefense_MaxDefenseAsilmaz()
        {
            var result = _calc.AddDefense(D(Element.Fire, 80), D(Element.Fire, 50), 100);
            Assert.AreEqual(100, result[Element.Fire]);
        }

        [Test]
        public void AddDefense_YeniElementEklenir()
        {
            var result = _calc.AddDefense(Empty(), D(Element.Water, 25), 100);
            Assert.AreEqual(25, result[Element.Water]);
        }

        // ---------- DoHeal ----------

        [Test]
        public void DoHeal_SadeceIyilestirilebilirElementlerSayilir()
        {
            var heal = new Dictionary<Element, int> { { Element.Fire, 30 }, { Element.Water, 50 } };
            int hp = _calc.DoHeal(heal, 40, 100, new[] { Element.Fire });
            Assert.AreEqual(70, hp);
        }

        [Test]
        public void DoHeal_MaxHpAsilmaz()
        {
            int hp = _calc.DoHeal(D(Element.Fire, 80), 90, 100, new[] { Element.Fire });
            Assert.AreEqual(100, hp);
        }

        [Test]
        public void DoHeal_HealableBossaIyilesmez()
        {
            int hp = _calc.DoHeal(D(Element.Fire, 30), 40, 100, new Element[0]);
            Assert.AreEqual(40, hp);
        }
    }
}