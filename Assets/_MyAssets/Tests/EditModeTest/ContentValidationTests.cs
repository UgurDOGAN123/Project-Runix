using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Runix.Ability;
using Runix.Card;
using Runix.Desk;
using Runix.Enemy;
using UnityEditor;
using UnityEngine;

namespace Runix.Tests
{
    public class ContentValidationTests
    {
        static List<T> LoadAll<T>() where T : Object =>
            AssetDatabase.FindAssets($"t:{typeof(T).Name}")
                .Select(g => AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(g)))
                .Where(a => a != null).ToList();

        static void AssertNoProblems(List<string> problems) =>
            Assert.IsEmpty(problems, "\n" + string.Join("\n", problems));

        [Test]
        public void Kartlar_ZorunluAlanlarDoluOlmali()
        {
            var problems = new List<string>();
            foreach (var card in LoadAll<CardSO>())
            {
                var so = new SerializedObject(card);   // private alanlarý okumak için
                /*if (so.FindProperty("animPrefab").objectReferenceValue == null)
                     problems.Add($"{card.name}: animPrefab boþ");
                if (so.FindProperty("animator").objectReferenceValue == null)
                    problems.Add($"{card.name}: animator boþ");*/
                if (so.FindProperty("DestroyTime").intValue <= 0)
                    problems.Add($"{card.name}: DestroyTime 0 ya da negatif");
                if (card.Value <= 0) problems.Add($"{card.name}: Value 0 ya da negatif");
                // if (card.CardImage == null) problems.Add($"{card.name}: CardImage boþ");
            }
            AssertNoProblems(problems);
        }

        [Test]
        public void Kartlar_CardTypeSinifiylaUyumluOlmali()
        {
            var problems = new List<string>();
            foreach (var card in LoadAll<CardSO>())
            {
                CardType? expected = card switch
                {
                    Attack => CardType.Attack,
                    Defense => CardType.Defense,
                    Heal => CardType.Heal,
                    Stun => CardType.Special,
                    _ => null
                };
                if (expected != null && card.cardType != expected)
                    problems.Add($"{card.name}: {card.GetType().Name} ama cardType {card.cardType}");
            }
            AssertNoProblems(problems);
        }

        [Test]
        public void Deste_DeðerleriKontrol()
        {
            var problems = new List<string>();
            foreach (var deck in LoadAll<DeckSO>())
            {
                if (string.IsNullOrWhiteSpace(deck.displayName))
                    problems.Add($"[{deck.name}] displayName boþ, seçim ekranýnda isim görünmez.");

                if (deck.cards == null || deck.cards.Count == 0)
                {
                    problems.Add($"[{deck.name}] kart listesi boþ.");
                }

                if (deck.cards.Contains(null))
                    problems.Add($"[{deck.name}] kart listesinde boþ (None) eleman var.");

                if (deck.kind == DeckKind.Element && deck.element == Element.None)
                    problems.Add($"[{deck.name}] Element türünde ama element seçilmemiþ.");
            }

            AssertNoProblems(problems);
        }

        [Test]
        public void Ability_DeðerleriKontrol()
        {
            var problems = new List<string>();
            foreach (var ability in LoadAll<AbilitySO>())
            {
                if (string.IsNullOrWhiteSpace(ability.displayName))
                    problems.Add($"{ability.name} isim yok.");
                if (string.IsNullOrWhiteSpace(ability.description))
                    problems.Add($"{ability.name} açýklama yok.");
            }

            AssertNoProblems(problems);
        }

        [Test]
        public void Dusmanlar_PatternVeIkonAtanmisOlmali()
        {
            var problems = new List<string>();
            foreach (var e in LoadAll<EnemySO>())
            {
                if (e.Pattern == null) problems.Add($"{e.name}: Pattern boþ");
                if (e.DeckLimits.StartCardCount == 0) problems.Add($"{e.name}: Start count boþ");
                if (e.DeckLimits.MaxHandCardCount == 0) problems.Add($"{e.name}: MaxHandCardCount boþ");
                if (e.DeckLimits.EveryTurnAddCardCount == 0) problems.Add($"{e.name}: EveryTurnAddCardCount boþ");
                if (e.HpValues.maxDefense == 0) problems.Add($"{e.name}: maxDefense boþ");
                if (e.HpValues.maxHp == 0) problems.Add($"{e.name}: maxHp boþ");
                if (e.DeckSO.Count == 0) problems.Add($"{e.name}: DeckSO boþ");
                if (e.Ability == null) problems.Add($"{e.name}: ability boþ");
                // if (e.EnemyIcon == null) problems.Add($"{e.name}: EnemyIcon boþ");
            }
            AssertNoProblems(problems);
        }

        [Test]
        public void DeskObject_ListeSays()
        {
            var problems = new List<string>();
            foreach (var e in LoadAll<DeskEnemiesSO>())
            {
                if (e.EnemyLists == null || e.EnemyLists.Count != 4)
                {
                    problems.Add($"{e.name}: tam 4 liste olmalý (þu an {e.EnemyLists?.Count ?? 0})");
                    continue;
                }

                for (int i = 0; i < e.EnemyLists.Count; i++)
                {
                    var list = e.EnemyLists[i];
                    if (list.minDesk >= list.maxDesk)
                        problems.Add($"{e.name} liste {i}: minDesk ({list.minDesk}) maxDesk'ten ({list.maxDesk}) küçük olmalý");

                    var expected = i == 3 ? EnemyType.Boss : EnemyType.Enemy;
                    if (list.Enemies == null || list.Enemies.Count == 0)
                    {
                        problems.Add($"{e.name} liste {i}: düþman listesi boþ");
                        continue;
                    }
                    foreach (var enemy in list.Enemies.Where(a => a == null || a.EnemyType != expected))
                        problems.Add($"{e.name} liste {i}: {(enemy == null ? "boþ eleman" : enemy.name)} türü {expected} olmalý");
                }
            }
            AssertNoProblems(problems);
        }
    }
}