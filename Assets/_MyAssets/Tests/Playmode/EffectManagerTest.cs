using System.Collections;
using System.Reflection;
using NUnit.Framework;
using Runix.Damage;
using Runix.Effect;
using UnityEngine;
using UnityEngine.TestTools;

namespace Runix.Tests.PlayMode
{
    public class EffectManagerTest
    {
        GameObject go;
        TestEffectmanager effectManager;
        readonly FakeHpManager hpManager = new();
        EffectContext effectContext;

        class TestEffectmanager : EffectManager
        {
            public void TurnManagerNewTurn(TurnEnum stage, int player, EffectContext context)
            {
                base.TurnCalculator(stage, player, context);
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
            effectContext = new EffectContext
            {
                OwnerId = 0,
                Hp = hpManager
            };
            go = new GameObject("TestObject");
            go.SetActive(false);
            effectManager = go.AddComponent<TestEffectmanager>();
            SetField("ownerId", 0);
            SetField("debugLog", false);
            SetField("ctx", effectContext);
            go.SetActive(true);

        }

        void SetField(string name, object value) =>
           typeof(EffectManager)
               .GetField(name, BindingFlags.NonPublic | BindingFlags.Instance)
               .SetValue(effectManager, value);

        void SetTest(TurnEnum stage, int player, int EffectCount, int RemainingTurns)
        {
            effectManager.TurnManagerNewTurn(stage, player, effectContext);
            Assert.AreEqual(effectManager.Effects.Count, EffectCount, $"More Effect Added effect count: {effectManager.Effects.Count}");
            for (int i = 0; i < effectManager.Effects.Count; i++)
            {
                Assert.AreEqual(effectManager.Effects[i].RemainingTurns, RemainingTurns, $"Coming more turn {effectManager.Effects[i]}");
            }

        }


        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator AddEffect_ANewStunEffectAdd()
        {
            effectManager.TurnManagerNewTurn(TurnEnum.BattleStart, 0, effectContext);
            Assert.AreEqual(effectManager.Effects.Count, 0, "More Effect Added");

            yield return null;

            effectManager.Add(new StunEffect(2, null, 4));

            yield return null;

            SetTest(TurnEnum.PlayTurn, 0, 1, 2);
            Assert.AreEqual(effectManager.BlockedSlotCount, 4, "Wrong Slot Blokked");

            yield return null;

            SetTest(TurnEnum.TurnDamage, 0, 1, 1);
            Assert.AreEqual(effectManager.BlockedSlotCount, 4, "Wrong Slot Blokked");

            yield return null;

            SetTest(TurnEnum.PlayTurn, 0, 1, 1);
            Assert.AreEqual(effectManager.BlockedSlotCount, 4, "Wrong Slot Blokked");

            yield return null;

            SetTest(TurnEnum.TurnDamage, 0, 0, 0);
            Assert.AreEqual(effectManager.BlockedSlotCount, 0, "Wrong Slot Blokked");
        }

        [UnityTest]
        public IEnumerator AddEffect_TwoNewStunEffectAdd()
        {
            effectManager.TurnManagerNewTurn(TurnEnum.BattleStart, 0, effectContext);
            Assert.AreEqual(effectManager.Effects.Count, 0, "More Effect Added");

            yield return null;

            effectManager.Add(new StunEffect(2, null, 2));
            effectManager.Add(new StunEffect(2, null, 2));

            yield return null;

            SetTest(TurnEnum.PlayTurn, 0, 2, 2);
            Assert.AreEqual(effectManager.BlockedSlotCount, 4, "Wrong Slot Blokked");

            yield return null;

            SetTest(TurnEnum.TurnDamage, 0, 2, 1);
            Assert.AreEqual(effectManager.BlockedSlotCount, 4, "Wrong Slot Blokked");

            yield return null;

            SetTest(TurnEnum.PlayTurn, 0, 2, 1);
            Assert.AreEqual(effectManager.BlockedSlotCount, 4, "Wrong Slot Blokked");

            yield return null;

            SetTest(TurnEnum.TurnDamage, 0, 0, 0);
            Assert.AreEqual(effectManager.BlockedSlotCount, 0, "Wrong Slot Blokked");
        }

        [UnityTest]
        public IEnumerator AddEffect_ANewElementEffectAdd()
        {
            effectManager.TurnManagerNewTurn(TurnEnum.BattleStart, 0, effectContext);
            Assert.AreEqual(effectManager.Effects.Count, 0, "More Effect Added");

            yield return null;

            effectManager.Add(new ElementEffect(2, 5, Element.Fire, null));

            yield return null;

            SetTest(TurnEnum.PlayTurn, 0, 1, 2);
            Assert.AreEqual(hpManager.hp, 95, $"More attacked this turn : TurnEnum.PlayTurn, 0, 1, 2");

            yield return null;

            SetTest(TurnEnum.TurnDamage, 0, 1, 1);
            Assert.AreEqual(hpManager.hp, 95, $"More attacked this turn: TurnEnum.TurnDamage, 0, 1, 1");

            yield return null;

            SetTest(TurnEnum.PlayTurn, 0, 1, 1);
            Assert.AreEqual(hpManager.hp, 90, $"More attacked this turn: TurnEnum.PlayTurn, 0, 1, 1");

            yield return null;

            SetTest(TurnEnum.TurnDamage, 0, 0, 0);
            Assert.AreEqual(hpManager.hp, 90, $"More attacked this turn: TurnEnum.TurnDamage, 0, 0, 0");
        }

        [UnityTest]
        public IEnumerator AddEffect_TwoNewElementEffectAdd()
        {
            effectManager.TurnManagerNewTurn(TurnEnum.BattleStart, 0, effectContext);
            Assert.AreEqual(effectManager.Effects.Count, 0, "More Effect Added");

            yield return null;

            effectManager.Add(new ElementEffect(2, 5, Element.Fire, null));
            effectManager.Add(new ElementEffect(2, 5, Element.Fire, null));

            yield return null;

            SetTest(TurnEnum.PlayTurn, 0, 2, 2);
            Assert.AreEqual(hpManager.hp, 90, $"More attacked this turn : TurnEnum.PlayTurn, 0, 2, 2");

            yield return null;

            SetTest(TurnEnum.TurnDamage, 0, 2, 1);
            Assert.AreEqual(hpManager.hp, 90, $"More attacked this turn: TurnEnum.TurnDamage, 0, 2, 1");

            yield return null;

            SetTest(TurnEnum.PlayTurn, 0, 2, 1);
            Assert.AreEqual(hpManager.hp, 80, $"More attacked this turn: TurnEnum.PlayTurn, 0, 2, 1");

            yield return null;

            SetTest(TurnEnum.TurnDamage, 0, 0, 0);
            Assert.AreEqual(hpManager.hp, 80, $"More attacked this turn: TurnEnum.TurnDamage, 0, 0, 0");
        }

        [UnityTest]
        public IEnumerator AddEffect_ANewElementEffectAddAndAStunEffectAdd()
        {
            effectManager.TurnManagerNewTurn(TurnEnum.BattleStart, 0, effectContext);
            Assert.AreEqual(effectManager.Effects.Count, 0, "More Effect Added");

            yield return null;

            effectManager.Add(new ElementEffect(2, 5, Element.Fire, null));
            effectManager.Add(new StunEffect(2, null, 4));

            yield return null;

            SetTest(TurnEnum.PlayTurn, 0, 2, 2);
            Assert.AreEqual(hpManager.hp, 95, $"More attacked this turn : TurnEnum.PlayTurn, 0, 2, 2");
            Assert.AreEqual(effectManager.BlockedSlotCount, 4, "Wrong Slot Blokked");


            yield return null;

            SetTest(TurnEnum.TurnDamage, 0, 2, 1);
            Assert.AreEqual(hpManager.hp, 95, $"More attacked this turn: TurnEnum.TurnDamage, 0, 2, 1");
            Assert.AreEqual(effectManager.BlockedSlotCount, 4, "Wrong Slot Blokked");

            yield return null;

            SetTest(TurnEnum.PlayTurn, 0, 2, 1);
            Assert.AreEqual(hpManager.hp, 90, $"More attacked this turn: TurnEnum.PlayTurn, 0, 2, 1");
            Assert.AreEqual(effectManager.BlockedSlotCount, 4, "Wrong Slot Blokked");

            yield return null;

            SetTest(TurnEnum.TurnDamage, 0, 0, 0);
            Assert.AreEqual(hpManager.hp, 90, $"More attacked this turn: TurnEnum.TurnDamage, 0, 0, 0");
            Assert.AreEqual(effectManager.BlockedSlotCount, 0, "Wrong Slot Blokked");

        }
    }
}


