using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Runix.Managers;

namespace Runix.Tests
{
    public class DeckModelTests
    {
        List<int> draw, trash, hand;
        DeckModel<int> model;

        [SetUp]
        public void SetUp()
        {
            draw = new List<int>();
            trash = new List<int>();
            hand = new List<int>();
            model = new DeckModel<int>(draw, trash, hand, new System.Random(42));
        }

        // Her işlemden sonra kart sayısı aynı kalmalı
        void AssertConserved(int expected) =>
            Assert.AreEqual(expected, model.TotalCount, "Kart kaybolmuş ya da çoğalmış.");

        [Test]
        public void StartBattle_TumListeleriSifirlarVeDesteyiDoldurur()
        {
            hand.Add(99); trash.Add(98);
            model.StartBattle(new[] { 1, 2, 3 });

            Assert.AreEqual(3, model.DrawCount);
            Assert.AreEqual(0, model.TrashCount);
            Assert.AreEqual(0, model.HandCount);
        }

        [Test]
        public void StartBattle_KartlariKaybetmezVeCogaltmaz()
        {
            model.StartBattle(Enumerable.Range(0, 20));
            CollectionAssert.AreEquivalent(Enumerable.Range(0, 20), draw);
        }

        [Test]
        public void TryDraw_KartiDesteyiAzaltipEleEkler()
        {
            model.StartBattle(new[] { 1, 2, 3 });

            Assert.IsTrue(model.TryDraw(out int card));
            Assert.AreEqual(2, model.DrawCount);
            Assert.AreEqual(1, model.HandCount);
            Assert.AreEqual(card, hand[0]);
            AssertConserved(3);
        }

        [Test]
        public void TryDraw_DesteBossaFalseDonerVeHataVermez()
        {
            bool result = true;
            Assert.DoesNotThrow(() => result = model.TryDraw(out _));
            Assert.IsFalse(result);
            Assert.AreEqual(0, model.HandCount);
        }

        [Test]
        public void NeedsReshuffle_SadeceDesteBosVeCopDoluyken()
        {
            Assert.IsFalse(model.NeedsReshuffle, "İkisi de boş");

            trash.Add(1);
            Assert.IsTrue(model.NeedsReshuffle, "Deste boş, çöp dolu");

            draw.Add(2);
            Assert.IsFalse(model.NeedsReshuffle, "Destede kart var");
        }

        [Test]
        public void Reshuffle_CopuDesteyeTasirKartKaybetmez()
        {
            trash.AddRange(new[] { 1, 2, 3, 4 });
            model.Reshuffle();

            Assert.AreEqual(0, model.TrashCount);
            CollectionAssert.AreEquivalent(new[] { 1, 2, 3, 4 }, draw);
            AssertConserved(4);
        }

        [Test]
        public void Reshuffle_MevcutDeAstekiKartlariKorur()
        {
            draw.Add(10);
            trash.AddRange(new[] { 1, 2 });
            model.Reshuffle();

            CollectionAssert.AreEquivalent(new[] { 10, 1, 2 }, draw);
        }

        [Test]
        public void IsHandFull_LimitteTrueOlur()
        {
            hand.AddRange(new[] { 1, 2, 3 });
            Assert.IsTrue(model.IsHandFull(3));
            Assert.IsFalse(model.IsHandFull(4));
        }

        [Test]
        public void ReturnToHand_ElDoluysaEklemez()
        {
            hand.AddRange(new[] { 1, 2 });

            Assert.IsFalse(model.ReturnToHand(3, 2));
            Assert.AreEqual(2, model.HandCount);
            Assert.IsTrue(model.ReturnToHand(3, 3));
            Assert.AreEqual(3, model.HandCount);
        }

        [Test]
        public void Discard_KartiCopeEkler()
        {
            model.Discard(5);
            Assert.AreEqual(1, model.TrashCount);
        }

        [Test]
        public void RemoveFromHand_OlmayanKartIcinFalseDoner()
        {
            hand.Add(1);
            Assert.IsTrue(model.RemoveFromHand(1));
            Assert.IsFalse(model.RemoveFromHand(1));
        }

        [Test]
        public void TamTur_CekOynaCopeAtKarisitir_KartSayisiHepSabit()
        {
            // Gerçek akış: çek, oyna (çöpe), deste bitince karıştır; 100 turda hiçbir kart kaybolmamalı
            model.StartBattle(Enumerable.Range(0, 10));

            for (int turn = 0; turn < 100; turn++)
            {
                for (int k = 0; k < 3; k++)
                {
                    if (model.NeedsReshuffle) model.Reshuffle();
                    if (!model.TryDraw(out _)) break;
                }
                while (hand.Count > 0)
                {
                    int card = hand[0];
                    model.RemoveFromHand(card);
                    model.Discard(card);
                }
                AssertConserved(10);
            }
        }
    }
}
