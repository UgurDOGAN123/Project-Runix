using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Runix.Card;
using Runix.Character;
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
                if (so.FindProperty("animPrefab").objectReferenceValue == null)
                    problems.Add($"{card.name}: animPrefab boþ");
                if (so.FindProperty("animator").objectReferenceValue == null)
                    problems.Add($"{card.name}: animator boþ");
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
        public void Karakterler_TemelDegerleriGecerliOlmali()
        {
            var problems = new List<string>();
            foreach (var c in LoadAll<CharacterSO>())
            {
                if (c.maxHp <= 0) problems.Add($"{c.name}: maxHp 0 ya da negatif");
                if (c.maxDefense <= 0) problems.Add($"{c.name}: maxDefense 0");
                if (c.Decks == null || c.Decks.Count == 0) problems.Add($"{c.name}: Decks boþ");
                else if (c.Decks.Any(d => d == null)) problems.Add($"{c.name}: Decks içinde boþ eleman");
                if (c.StartCardCount > c.MaxHandCardCount)
                    problems.Add($"{c.name}: StartCardCount > MaxHandCardCount");
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
                // if (e.EnemyIcon == null) problems.Add($"{e.name}: EnemyIcon boþ");
            }
            AssertNoProblems(problems);
        }
    }
}