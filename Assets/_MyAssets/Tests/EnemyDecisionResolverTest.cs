using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Runix.Card;
using Runix.Enemy;
using UnityEngine;

namespace Runix.Tests
{
    public class EnemyDecisionResolverTest
    {
        private EnemyDecisionResolver _resolver;
        private AdaptiveEnemyProfileSO _profile;
        private List<Object> _createdAssets;

        [SetUp]
        public void SetUp()
        {
            _resolver = new EnemyDecisionResolver();
            _createdAssets = new List<Object>();

            _profile = Track(ScriptableObject.CreateInstance<AdaptiveEnemyProfileSO>());
            _profile.maxPlaySlots = 4;
            _profile.healthTiers = new List<HealthTierDecision>();
        }

        [TearDown]
        public void TearDown()
        {
            foreach (var asset in _createdAssets)
            {
                if (asset != null) Object.DestroyImmediate(asset);
            }
            _createdAssets.Clear();
            _profile = null;
        }

        // ---------- Yardýmcýlar ----------

        // Oluþturulan her ScriptableObject'i kaydeder, TearDown'da silinir
        private T Track<T>(T asset) where T : Object
        {
            _createdAssets.Add(asset);
            return asset;
        }

        // Tek yardýmcý: Attack, Defense, Heal... hepsini bununla üretirsiniz
        private T CreateCard<T>(CardType type, Element element = Element.Fire, int value = 10) where T : CardSO
        {
            var card = Track(ScriptableObject.CreateInstance<T>());
            card.cardType = type;
            card.element = element;
            card.Value = value;
            return card;
        }

        private EnemyCardPatternSO CreatePattern(params (CardType type, int count)[] requirements)
        {
            var pattern = Track(ScriptableObject.CreateInstance<EnemyCardPatternSO>());
            pattern.cardRequirements = requirements
                .Select(r => new CardRequirement { CardType = r.type, count = r.count })
                .ToList();
            return pattern;
        }

        private void AddTier(float minHpPercent, params EnemyCardPatternSO[] patterns)
        {
            _profile.healthTiers.Add(new HealthTierDecision
            {
                minHpPercent = minHpPercent,
                prioritizedPatterns = patterns.ToList()
            });
        }

        private static int CountOf(List<CardSO> cards, CardType type) =>
            cards.Count(c => c.cardType == type);

        // ---------- Testler ----------

        [Test]
        public void ResolveTurnCards_ElDeseneUydugunda_DogruKombinasyonuDonmeli()
        {
            AddTier(0.5f, CreatePattern((CardType.Attack, 2), (CardType.Defense, 2)));

            var hand = new List<CardSO>
            {
                CreateCard<Attack>(CardType.Attack),
                CreateCard<Attack>(CardType.Attack),
                CreateCard<Defense>(CardType.Defense),
                CreateCard<Defense>(CardType.Defense),
                CreateCard<Heal>(CardType.Heal),
            };

            var selected = _resolver.ResolveTurnCards(_profile, 0.8f, hand);

            Assert.IsNotNull(selected);
            Assert.AreEqual(4, selected.Count, "Desendeki kart sayýsý kadar kart seçilmeli.");
            Assert.AreEqual(2, CountOf(selected, CardType.Attack), "Saldýrý sayýsý desendeki kadar olmalý.");
            Assert.AreEqual(2, CountOf(selected, CardType.Defense), "Savunma sayýsý desendeki kadar olmalý.");
            Assert.AreEqual(0, CountOf(selected, CardType.Heal), "Ýyileþtirme kartý seçilmemeli.");
        }

        [Test]
        public void ResolveTurnCards_DesenSlotLimitiniAstiginda_MaxPlaySlotsKadarKartDonmeli()
        {
            _profile.maxPlaySlots = 3;
            AddTier(0.2f, CreatePattern((CardType.Attack, 5)));

            var hand = Enumerable.Range(0, 5)
                .Select(_ => (CardSO)CreateCard<Attack>(CardType.Attack))
                .ToList();

            var selected = _resolver.ResolveTurnCards(_profile, 0.9f, hand);

            Assert.IsNotNull(selected);
            Assert.AreEqual(3, selected.Count, "Seçilen kart sayýsý maxPlaySlots sýnýrýný aþmamalý.");
        }

        [Test]
        public void ResolveTurnCards_CanEnDusukTierAltinaIndiginde_NullHatasiVermedenEslesenKartlarDonmeli()
        {
            AddTier(0.3f, CreatePattern((CardType.Attack, 1)));

            var hand = new List<CardSO>
            {
                CreateCard<Attack>(CardType.Attack),
                CreateCard<Defense>(CardType.Defense)
            };

            List<CardSO> selected = null;
            Assert.DoesNotThrow(
                () => selected = _resolver.ResolveTurnCards(_profile, 0.1f, hand),
                "Can en düþük tier'ýn altýna inince hata fýrlatýlmamalý.");

            // Desen sadece 1 saldýrý istiyor: yalnýzca eþleþen kart döner
            Assert.IsNotNull(selected);
            Assert.AreEqual(1, selected.Count);
            Assert.AreEqual(CardType.Attack, selected[0].cardType);
        }

        [Test]
        public void ResolveTurnCards_HicbirDesenUymazsa_EldekiIlkKartlarDonmeli()
        {
            // Desen iyileþtirme istiyor ama elde yok: yedek yol çalýþmalý
            AddTier(0.0f, CreatePattern((CardType.Heal, 1)));

            var hand = new List<CardSO>
            {
                CreateCard<Attack>(CardType.Attack),
                CreateCard<Defense>(CardType.Defense)
            };

            var selected = _resolver.ResolveTurnCards(_profile, 0.5f, hand);

            Assert.IsNotNull(selected);
            Assert.AreEqual(2, selected.Count, "Desen uymazsa eldeki kartlar (slot sýnýrýna kadar) oynanmalý.");
        }

        [Test]
        public void ResolveTurnCards_IlkDesenUymazsa_SiradakiDeseneDusmeli()
        {
            // Oyundaki gerçek durumun testi: 2 savunma yoksa 1 savunma + 1 iyileþtirmeli desen seçilir
            AddTier(0.0f,
                CreatePattern((CardType.Attack, 2), (CardType.Defense, 2)),
                CreatePattern((CardType.Attack, 2), (CardType.Heal, 1), (CardType.Defense, 1)));

            var hand = new List<CardSO>
            {
                CreateCard<Attack>(CardType.Attack),
                CreateCard<Attack>(CardType.Attack),
                CreateCard<Defense>(CardType.Defense),   // sadece 1 savunma
                CreateCard<Heal>(CardType.Heal),
            };

            var selected = _resolver.ResolveTurnCards(_profile, 0.5f, hand);

            Assert.AreEqual(2, CountOf(selected, CardType.Attack));
            Assert.AreEqual(1, CountOf(selected, CardType.Defense));
            Assert.AreEqual(1, CountOf(selected, CardType.Heal));
        }
    }
}
