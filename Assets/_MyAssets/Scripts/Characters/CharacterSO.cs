using System.Collections.Generic;
using Runix.Card;
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
        public int maxDefans;
        public Element[] healable;
        public PasiveDefans[] pasiveDefans;

        [Header("Deck")]
        public List<CardSO> Decks;

        public virtual void UseAbility() { }

        //CharacterData eklenecek
        public virtual void SetData() { }
    }
    [System.Serializable]
    public struct PasiveDefans
    {
        public Element element;
        public int Value;
    }
}
