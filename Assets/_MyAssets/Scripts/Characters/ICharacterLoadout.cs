using System.Collections.Generic;
using System.Linq;
using Runix.Ability;
using Runix.Card;

namespace Runix.Character
{
    public interface ICharacterLoadout
    {
        public string Name { get; }
        public string Description { get; }
        public IReadOnlyList<DeckSO> DeckSO { get; }
        public AbilitySO Ability { get; }
        public HpValues HpValues { get; }
        public DeckLimits DeckLimits { get; }
    }
    public static class LoadoutExtensions
    {
        static IEnumerable<DeckSO> ValidDecks(ICharacterLoadout l) =>
            l?.DeckSO == null ? Enumerable.Empty<DeckSO>() : l.DeckSO.Where(d => d != null);

        public static List<CardSO> BuildCards(this ICharacterLoadout l) =>
            ValidDecks(l).Where(d => d.cards != null).SelectMany(d => d.cards).Where(c => c != null).ToList();

        public static Element[] GetHealable(this ICharacterLoadout l) =>
            ValidDecks(l).Where(d => d.healable != null).SelectMany(d => d.healable).Distinct().ToArray();

        public static Dictionary<Element, int> GetPassiveDefense(this ICharacterLoadout l) =>
            ValidDecks(l).Where(d => d.passiveDefense != null).SelectMany(d => d.passiveDefense)
                .GroupBy(p => p.element).ToDictionary(g => g.Key, g => g.Sum(p => p.Value));

        /// <summary>Yeteneðin kullanacaðý ana element: ilk Element türündeki destenin elementi.</summary>
        public static Element GetPrimaryElement(this ICharacterLoadout l) =>
            ValidDecks(l).FirstOrDefault(d => d.kind == DeckKind.Element)?.element ?? Element.None;
    }


    [System.Serializable]
    public struct HpValues
    {
        public int maxHp;
        public int maxDefense;
    }

    [System.Serializable]
    public struct DeckLimits
    {
        public int MaxHandCardCount;
        public int StartCardCount;
        public int EveryTurnAddCardCount;
    }
}
