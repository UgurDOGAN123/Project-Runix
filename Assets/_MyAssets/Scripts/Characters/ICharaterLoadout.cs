using Runix.Ability;
using Runix.Card;
using Runix.Character;

public interface ICharaterLoadout
{
    public string Name { get; }
    public string Description { get; }
    public DeckSO DeckSO { get; }
    public AbilitySO Ability { get; }
    public HpValues HpValues { get; }
    public DeckLimits DeckLimits { get; }
}

namespace Runix.Character
{
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
