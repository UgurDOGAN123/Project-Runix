using System;
using System.Collections.Generic;
using UnityEngine;

namespace Runix
{
    [Serializable]
    public class TurnDataHolder 
    {
        public Dictionary<Element, int> AttackValues = new();
        public Dictionary<Element, int> DefensValues = new();
        public Dictionary<Element, int> HealValues = new();

        public void AddAttack(Element element,int attackValue)
        {
            AttackValues.TryGetValue(element, out int a);
            a += attackValue;
            AttackValues.Add(element, a);
        }
        public void AddDefens(Element element, int defensvalue)
        {
            DefensValues.TryGetValue(element, out int a);
            a += defensvalue;
            DefensValues.Add(element, a);
        }
        public void AddHeal(Element element, int healValue)
        {
            
            HealValues.TryGetValue(element, out int a);
            a += healValue;
            HealValues.Add(element, a);
        }
    }
}