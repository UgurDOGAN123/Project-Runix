using System.Collections.Generic;
using Runix.Card;
using Runix.Effect;
using UnityEngine;


namespace Runix.Character
{
    public abstract class CharacterSO : ScriptableObject
    {
        [Header("General Info")]
        public string characterName;
        public string description;

        [Header("Deck Limits")]
        public int MaxHandCardCount;
        public int StartCardCount;
        public int EveryTurnAddCardCount;

        [Header("Hp Values")]
        public int maxHp;
        public int maxDefense;
        public Element[] healable;
        public PassiveDefense[] passiveDefense;

        [Header("Deck")]
        public List<CardSO> Decks;

        public virtual void UseAbility(AbilityContext abilityContext) { }

        //CharacterData eklenecek
        public virtual void SetData() { }
    }
    [System.Serializable]
    public struct PassiveDefense
    {
        public Element element;
        public int Value;
    }

    // Karakter yeteneðinin sahneye eriþim yolu
    public class AbilityContext
    {
        public int OwnerId;
        public EffectManager Own;       // yeteneði kullananýn etkileri
        public EffectManager Opponent;  // rakibin etkileri
    }
}
