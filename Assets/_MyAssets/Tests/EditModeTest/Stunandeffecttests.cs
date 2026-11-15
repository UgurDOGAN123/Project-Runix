using NUnit.Framework;
using Runix.Character;
using Runix.Effect;
using Runix.Enemy;
using UnityEngine;

namespace Runix.Tests
{
    public class StunAndEffectTests
    {
        private GameObject _effectGo;
        private EffectManager _opponentEffectManager;
        private StunBoss _stunBoss;

        [SetUp]
        public void SetUp()
        {
            _effectGo = new GameObject("TestEffectManager");
            _opponentEffectManager = _effectGo.AddComponent<EffectManager>();

            _stunBoss = ScriptableObject.CreateInstance<StunBoss>();
            _stunBoss.stunChance = 1f; // Testte kesin çalýþmasý için %100 olasýlýk
            _stunBoss.stunTurns = 1;
            _stunBoss.stunBlockedSlots = 2;
        }

        [TearDown]
        public void TearDown()
        {
            if (_effectGo != null)
            {
                Object.DestroyImmediate(_effectGo);
            }
            if (_stunBoss != null)
            {
                Object.DestroyImmediate(_stunBoss);
            }
        }

        [Test]
        public void StunEffect_Eklendiginde_BlockedSlotCountArtmali()
        {
            // 1. Eylem (Act): EffectManager'a 3 slot kilitleyen 1 turluk StunEffect eklenir
            _opponentEffectManager.Add(new StunEffect(1, 3));

            // 2. Doðrulama (Assert): Kilitlenen slot sayýsý 3 olmalý
            Assert.AreEqual(3, _opponentEffectManager.BlockedSlotCount, "StunEffect eklendiðinde BlockedSlotCount doðru güncellenmedi.");
        }

        [Test]
        public void StunBoss_UseAbility_RakipSersemDegilseStunUygulamali()
        {
            // 1. Hazýrlýk (Arrange)
            var context = new AbilityContext
            {
                OwnerId = PlayerIds.Enemy,
                Own = null,
                Opponent = _opponentEffectManager
            };

            // 2. Eylem (Act): StunBoss %100 þansla yeteneðini kullanýr
            _stunBoss.UseAbility(context);

            // 3. Doðrulama (Assert): Rakibin 2 slotu kilitlenmeli
            Assert.AreEqual(2, _opponentEffectManager.BlockedSlotCount, "StunBoss yeteneði rakibin slotlarýný kilitlemedi.");
        }

        [Test]
        public void StunBoss_UseAbility_RakipZatenSersemse_TekrarStunEklememeli()
        {
            // 1. Hazýrlýk (Arrange): Rakibin zaten 1 slotu kilitli
            _opponentEffectManager.Add(new StunEffect(1, 1));
            var context = new AbilityContext
            {
                OwnerId = PlayerIds.Enemy,
                Own = null,
                Opponent = _opponentEffectManager
            };

            // 2. Eylem (Act): StunBoss yeteneðini tekrar kullanmayý dener
            _stunBoss.UseAbility(context);

            // 3. Doðrulama (Assert): Rakip zaten sersem olduðu için yeni kilit eklenmemeli (1 olarak kalmalý)
            Assert.AreEqual(1, _opponentEffectManager.BlockedSlotCount, "Rakip zaten sersemken StunBoss üst üste Stun eklememeli.");
        }
    }
}