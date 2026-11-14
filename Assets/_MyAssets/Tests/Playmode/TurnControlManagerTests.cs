using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using Runix.TurnControlSystem;
using UnityEngine;
using UnityEngine.TestTools;

namespace Runix.Tests.PlayMode
{
    public class TurnControlManagerTests
    {
        GameObject go;
        TurnControlManager mgr;
        List<string> log;

        [SetUp]
        public void SetUp()
        {
            log = new List<string>();

            // Pasif oluþtur: Awake/OnEnable (ve autoStart) ayarlarý yapana kadar çalýþmasýn
            go = new GameObject("TurnManagerTest");
            go.SetActive(false);
            mgr = go.AddComponent<TurnControlManager>();

            SetField("autoStart", false);
            SetField("startDelay", 0f);
            SetField("turnEndDelay", 0f);
            SetField("damageFallbackDelay", 0f);
            SetField("playerCount", 2);

            go.SetActive(true);
            mgr.NewTurn += (stage, player) => log.Add($"{stage}:{player}");
        }

        [TearDown]
        public void TearDown()
        {
            if (go != null) Object.DestroyImmediate(go);   // OnDestroy singleton'ý temizler
        }

        void SetField(string name, object value) =>
            typeof(TurnControlManager)
                .GetField(name, BindingFlags.NonPublic | BindingFlags.Instance)
                .SetValue(mgr, value);

        IEnumerator WaitFor(TurnEnum stage, int player, float timeout = 3f)
        {
            float t = 0f;
            while (!(mgr.CurrentStage == stage && mgr.CurrentPlayer == player))
            {
                t += Time.unscaledDeltaTime;
                if (t > timeout)
                    Assert.Fail($"Zaman aþýmý: {stage}:{player} beklendi, þu an {mgr.CurrentStage}:{mgr.CurrentPlayer}. Log: {string.Join(", ", log)}");
                yield return null;
            }
        }

        [UnityTest]
        public IEnumerator Akis_DogruSiradaIlerler()
        {
            mgr.StartBattle();

            yield return WaitFor(TurnEnum.PlayTurn, 0);
            mgr.RequestEndTurn(0);

            yield return WaitFor(TurnEnum.PlayTurn, 1);
            mgr.RequestEndTurn(1);

            yield return WaitFor(TurnEnum.PlayTurn, 0);   // yeni tur baþladý

            CollectionAssert.AreEqual(new[]
            {
                "BattleStart:0",
                "PlayTurn:0", "TurnDamage:0",
                "PlayTurn:1", "TurnDamage:1",
                "TurnEnd:0",
                "PlayTurn:0",
            }, log);
        }

        [UnityTest]
        public IEnumerator SirasiOlmayanOyuncuTuruBitiremez()
        {
            mgr.StartBattle();
            yield return WaitFor(TurnEnum.PlayTurn, 0);

            mgr.RequestEndTurn(1);   // sýra 0'da, 1 çaðýrýyor
            yield return null;
            yield return null;

            Assert.AreEqual(TurnEnum.PlayTurn, mgr.CurrentStage);
            Assert.AreEqual(0, mgr.CurrentPlayer);
            CollectionAssert.DoesNotContain(log, "TurnDamage:0");
        }

        [UnityTest]
        public IEnumerator CiftRequestEndTurn_TekSayilir()
        {
            mgr.StartBattle();
            yield return WaitFor(TurnEnum.PlayTurn, 0);

            mgr.RequestEndTurn(0);
            mgr.RequestEndTurn(0);   // ayný karede ikinci çaðrý
            yield return WaitFor(TurnEnum.PlayTurn, 1);
            yield return null;
            yield return null;

            Assert.AreEqual(1, log.FindAll(l => l == "TurnDamage:0").Count);
            Assert.AreEqual(1, log.FindAll(l => l == "PlayTurn:1").Count);
        }

        [UnityTest]
        public IEnumerator EndBattle_OyuncuBeklerkenCagrilirsa_SavasTemizBiter()
        {
            mgr.StartBattle();
            yield return WaitFor(TurnEnum.PlayTurn, 0);

            mgr.EndBattle();
            yield return null;
            yield return null;

            Assert.AreEqual(TurnEnum.BattleEnd, mgr.CurrentStage);
            Assert.IsFalse(mgr.IsBattleRunning);
        }

        [Test]
        public void SavasBaslamadanRequestEndTurn_HataVermez()
        {
            Assert.DoesNotThrow(() => mgr.RequestEndTurn(0));
        }

        [UnityTest]
        public IEnumerator IsPlayerInputAllowed_SadeceOyuncununPlayTurnundaTrue()
        {
            Assert.IsFalse(mgr.IsPlayerInputAllowed, "Savaþ baþlamadan kapalý olmalý.");

            mgr.StartBattle();
            yield return WaitFor(TurnEnum.PlayTurn, 0);
            Assert.IsTrue(mgr.IsPlayerInputAllowed);

            mgr.RequestEndTurn(0);
            yield return WaitFor(TurnEnum.PlayTurn, 1);
            Assert.IsFalse(mgr.IsPlayerInputAllowed, "Düþmanýn sýrasýnda kapalý olmalý.");
        }
    }
}