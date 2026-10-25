using System;
using System.Collections.Generic;

namespace Runix
{
    [Serializable]
    public class TurnDataHolder
    {
        public Dictionary<Element, int> AttackValues = new();
        public Dictionary<Element, int> DefensValues = new();
        public Dictionary<Element, int> HealValues = new();
        public Dictionary<Element, int> MultiplerValues = new();

        public void AddAttack(Element element, int attackValue)
        {
            AddFonk(AttackValues, element, attackValue);
        }
        public void AddDefens(Element element, int defensvalue)
        {
            AddFonk(DefensValues, element, defensvalue);
        }
        public void AddHeal(Element element, int healValue)
        {
            AddFonk(HealValues, element, healValue);
        }

        public void ClearMulipler()
        {
            MultiplerValues.Clear();
        }

        void AddFonk(Dictionary<Element, int> fonkValues, Element element, int value)
        {
            fonkValues.TryGetValue(element, out int current);
            fonkValues[element] = current + value;
            AddMultipler(element);
        }


        void AddMultipler(Element element)
        {
            MultiplerValues.TryGetValue(element, out int a);
            a += 1;
            MultiplerValues[element] = a;
        }
    }
}