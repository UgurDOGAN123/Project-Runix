using System;
using System.Reflection;
using NUnit.Framework;
using Runix.Character;
using Runix.Damage;
using Runix.Enemy;
using Runix.Managers;
using Runix.UI;
using UnityEngine;

namespace Runix.Tests.PlayMode
{
    public class HpManagerTest
    {
        const int MaxHp = 100;

        GameObject deskGo;
        GameObject hpGo;
        GameObject sliderGo;
        EnemySO enemySO;
        DeskManager desk;
        HpManager hpManager;
        int deathCount;

        [SetUp]
        public void SetUp()
        {
            deathCount = 0;

            // 1) DeskManager: HpManager Awake/OnEnable'da DeskManager.instance'a ihtiyac duyar
            deskGo = new GameObject("Desk");
            desk = deskGo.AddComponent<DeskManager>();
            var hpValues = new HpValues { maxHp = MaxHp, maxDefense = 50 };
            desk.Player = new PlayerLoadout("Test", "", null, null, null, hpValues, default);

            enemySO = ScriptableObject.CreateInstance<EnemySO>();
            SetPrivate(enemySO, "hpValues", hpValues);
            desk.Enemy = enemySO;
        }

        [TearDown]
        public void TearDown()
        {
            if (hpGo != null) UnityEngine.Object.DestroyImmediate(hpGo);
            if (sliderGo != null) UnityEngine.Object.DestroyImmediate(sliderGo);
            if (deskGo != null) UnityEngine.Object.DestroyImmediate(deskGo);   // OnDestroy instance'i temizler
            if (enemySO != null) UnityEngine.Object.DestroyImmediate(enemySO);
        }

        // ---------------- Yardimcilar ----------------

        static void SetPrivate(object target, string name, object value)
        {
            var field = target.GetType().GetField(name, BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.IsNotNull(field, $"'{name}' alani bulunamadi (adi degismis olabilir)");
            field.SetValue(target, value);
        }

        static object GetPrivate(object target, string name) =>
            target.GetType().GetField(name, BindingFlags.NonPublic | BindingFlags.Instance).GetValue(target);

        // Slider UnityEngine.UI'da; test asmdef'ine referans eklememek icin tip adiyla ekliyoruz
        HpSlider CreateHpSlider()
        {
            sliderGo = new GameObject("Slider");
            sliderGo.SetActive(false);
            var hpSlider = sliderGo.AddComponent<HpSlider>();
            var sliderType = Type.GetType("UnityEngine.UI.Slider, UnityEngine.UI");
            Assert.IsNotNull(sliderType, "UnityEngine.UI bulunamadi");
            var slider = sliderGo.AddComponent(sliderType);
            SetPrivate(hpSlider, "slider", slider);
            sliderGo.SetActive(true);
            return hpSlider;
        }

        void CreateHpManager(int playerId)
        {
            var slider = CreateHpSlider();
            hpGo = new GameObject("Hp");
            hpGo.SetActive(false);                       // alanlar ayarlanmadan Awake calismasin
            hpManager = hpGo.AddComponent<HpManager>();
            SetPrivate(hpManager, "playerID", playerId);
            SetPrivate(hpManager, "hpSlider", slider);
            hpManager.OnDeath += () => deathCount++;
            hpGo.SetActive(true);                        // Awake + OnEnable
        }

        void SetHp(int hp) => SetPrivate(hpManager, "hp", hp);
        int CurrentHp => (int)GetPrivate(hpManager, "hp");

        static TurnDataHolder Attack(Element element, int attack, int multiplier = 1)
        {
            var d = new TurnDataHolder();
            d.AttackValues[element] = attack;
            d.MultiplierValues[element] = multiplier;   // 0 olursa hasar 0 (carpan kare alinir)
            return d;
        }

        // ---------------- Baslangic ----------------

        [Test]
        public void Player_MaxHpIleBaslar()
        {
            CreateHpManager(PlayerIds.Player);

            Assert.AreEqual(1f, hpManager.HpPercent, 0.001f);
        }

        [Test]
        public void Player_OnEnableYenidenCalisinca_HpSifirlanmaz()
        {
            CreateHpManager(PlayerIds.Player);
            hpManager.TakeDirectDamage(50);

            hpGo.SetActive(false);
            hpGo.SetActive(true);

            Assert.AreEqual(0.5f, hpManager.HpPercent, 0.001f, "oyuncunun hp'si savaslar arasinda korunmali");
        }

        [Test]
        public void Enemy_OnEnableIleMaxHpyeDoner()
        {
            CreateHpManager(PlayerIds.Enemy);
            SetHp(10);

            hpGo.SetActive(false);
            hpGo.SetActive(true);

            Assert.AreEqual(1f, hpManager.HpPercent, 0.001f);
        }

        // ---------------- Dogrudan hasar ----------------

        [Test]
        public void TakeDirectDamage_HpyiAzaltir()
        {
            CreateHpManager(PlayerIds.Player);

            hpManager.TakeDirectDamage(30);

            Assert.AreEqual(70, CurrentHp);
            Assert.AreEqual(0.7f, hpManager.HpPercent, 0.001f);
        }

        [Test]
        public void TakeDirectDamage_OlumdenOnce_OnDeathTetiklenmez()
        {
            CreateHpManager(PlayerIds.Player);

            hpManager.TakeDirectDamage(99);

            Assert.AreEqual(0, deathCount);
        }

        [Test]
        public void TakeDirectDamage_HpSifirlaninca_OnDeathBirKezTetiklenir()
        {
            CreateHpManager(PlayerIds.Player);

            hpManager.TakeDirectDamage(100);

            Assert.AreEqual(1, deathCount);
        }

        [Test]
        public void TakeDirectDamage_OlduktenSonra_HpDegismezVeOnDeathTekrarlanmaz()
        {
            CreateHpManager(PlayerIds.Player);
            hpManager.TakeDirectDamage(100);
            int hpAfterDeath = CurrentHp;

            hpManager.TakeDirectDamage(50);
            hpManager.TakeDirectDamage(50);

            Assert.AreEqual(hpAfterDeath, CurrentHp, "olu karakter hasar almamali");
            Assert.AreEqual(1, deathCount, "OnDeath tek kez");
        }

        // ---------------- AddData (iyilesme / savunma / saldiri) ----------------

        [Test]
        public void AddData_Saldiri_HpyiAzaltir()
        {
            CreateHpManager(PlayerIds.Player);

            hpManager.AddData(new TurnDataHolder(), Attack(Element.Fire, 10));

            Assert.AreEqual(90, CurrentHp);
        }

        [Test]
        public void AddData_OldurucuSaldiri_OnDeathBirKezTetiklenir()
        {
            CreateHpManager(PlayerIds.Player);

            hpManager.AddData(new TurnDataHolder(), Attack(Element.Fire, 200));
            hpManager.AddData(new TurnDataHolder(), Attack(Element.Fire, 200));

            Assert.AreEqual(1, deathCount);
        }

        [Test]
        public void AddData_SavunmaAyniTurdaSaldiriyiAzaltir()
        {
            CreateHpManager(PlayerIds.Player);
            var defense = new TurnDataHolder();
            defense.DefenseValues[Element.Fire] = 50;     // %50 azaltma

            hpManager.AddData(defense, Attack(Element.Fire, 20));

            Assert.AreEqual(90, CurrentHp, "20 saldiri %50 savunmayla 10 olmali");
        }

        [Test]
        public void AddData_SavunmaBirSaldiridaTukenir()
        {
            CreateHpManager(PlayerIds.Player);
            var defense = new TurnDataHolder();
            defense.DefenseValues[Element.Fire] = 50;
            hpManager.AddData(defense, Attack(Element.Fire, 20));   // 100 -> 90

            hpManager.AddData(new TurnDataHolder(), Attack(Element.Fire, 20));

            Assert.AreEqual(70, CurrentHp, "savunma tukenmeli, ikinci saldiri tam vurmali");
        }

        [Test]
        public void AddData_Iyilesme_MaxHpyiAsmaz()
        {
            CreateHpManager(PlayerIds.Player);
            hpManager.TakeDirectDamage(10);
            var heal = new TurnDataHolder();
            heal.HealValues[Element.Fire] = 50;

            hpManager.AddData(heal, new TurnDataHolder());

            Assert.LessOrEqual(CurrentHp, MaxHp);
        }

        // ---------------- Yeniden baslama ----------------

        [Test]
        public void OnEnable_IsDeadSifirlanir_OnDeathYenidenTetiklenebilir()
        {
            CreateHpManager(PlayerIds.Enemy);
            hpManager.TakeDirectDamage(100);
            Assert.AreEqual(1, deathCount, "on kosul: ilk olum");

            hpGo.SetActive(false);
            hpGo.SetActive(true);                       // yeni dusman: hp = MaxHp, isDead = false
            hpManager.TakeDirectDamage(100);

            Assert.AreEqual(2, deathCount);
        }
    }
}
