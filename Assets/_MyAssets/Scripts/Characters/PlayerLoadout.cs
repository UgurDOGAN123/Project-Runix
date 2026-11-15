using Runix.Ability;
using Runix.Card;
using Runix.Character;
using UnityEngine;

public class PlayerLoadout : MonoBehaviour, ICharaterLoadout
{
    string _name, description;
    DeckSO deckSO;
    AbilitySO abilitySO;
    HpValues hpValues;
    DeckLimits deckLimits;

    public PlayerLoadout(string name, string description, DeckSO deckSO, AbilitySO abilitySO, HpValues hpValues, DeckLimits deckLimits)
    {
        _name = name;
        this.description = description;
        this.deckSO = deckSO;
        this.abilitySO = abilitySO;
        this.hpValues = hpValues;
        this.deckLimits = deckLimits;
    }

    public string Name => _name;

    public string Description => description;

    public DeckSO DeckSO => deckSO;

    public AbilitySO Ability => abilitySO;

    public HpValues HpValues => hpValues;

    public DeckLimits DeckLimits => deckLimits;
}
