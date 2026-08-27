using System.Collections.Generic;
using Runix.Card;
using UnityEngine;

namespace Runix.Character
{
    public abstract class CharacterSO : ScriptableObject
    {
        public string characterName;
        public string description;
        public int MaxHandCardCount;
        public int StartCardCount;

        public List<CardSO> Decks;

        public abstract void UseAbility();

        //CharacterData eklenecek
        public abstract void SetData();
    }
}
