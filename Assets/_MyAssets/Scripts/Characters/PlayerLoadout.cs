using System.Collections.Generic;
using Runix.Ability;
using Runix.Card;
using UnityEngine;

namespace Runix.Character
{
    [System.Serializable]
    public class PlayerLoadout : ICharaterLoadout
    {
        [SerializeField] string _name, description;
        [SerializeField] DeckSO elementDeck;
        [SerializeField] DeckSO specialDeck;
        [SerializeField] AbilitySO abilitySO;
        [SerializeField] HpValues hpValues;
        [SerializeField] DeckLimits deckLimits;

        public PlayerLoadout(string name, string description, DeckSO elementDeck, DeckSO specialDeck, AbilitySO abilitySO, HpValues hpValues, DeckLimits deckLimits)
        {
            _name = name;
            this.description = description;
            this.elementDeck = elementDeck;
            this.specialDeck = specialDeck;
            this.abilitySO = abilitySO;
            this.hpValues = hpValues;
            this.deckLimits = deckLimits;
        }

        public string Name => _name;

        public string Description => description;

        public IReadOnlyList<DeckSO> DeckSO
        {
            get
            {
                var list = new List<DeckSO>();
                if (elementDeck != null) list.Add(elementDeck);
                if (specialDeck != null) list.Add(specialDeck);
                return list;
            }
        }

        public AbilitySO Ability => abilitySO;

        public HpValues HpValues => hpValues;

        public DeckLimits DeckLimits => deckLimits;
    }
}