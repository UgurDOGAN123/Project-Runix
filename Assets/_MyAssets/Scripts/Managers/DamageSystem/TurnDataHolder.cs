using System.Collections.Generic;
using Runix.Effect;

namespace Runix
{
    public class TurnDataHolder
    {
        public Dictionary<Element, int> AttackValues = new();
        public Dictionary<Element, int> DefenseValues = new();
        public Dictionary<Element, int> HealValues = new();
        public Dictionary<Element, int> MultiplierValues = new();
        public List<IEffect> PendingEffects = new();

        public void AddEffect(IEffect effect)
        {
            if (effect != null) PendingEffects.Add(effect);
        }

        public void AddAttack(Element element, int attackValue)
        {
            AddElementValue(AttackValues, element, attackValue);
        }
        public void AddDefense(Element element, int defensvalue)
        {
            AddElementValue(DefenseValues, element, defensvalue);
        }
        public void AddHeal(Element element, int healValue)
        {
            AddElementValue(HealValues, element, healValue);
        }

        public void AllClear()
        {
            MultiplierValues.Clear();
            DefenseValues.Clear();
            HealValues.Clear();
            AttackValues.Clear();
            PendingEffects.Clear();
        }

        void AddElementValue(Dictionary<Element, int> fonkValues, Element element, int value)
        {
            fonkValues.TryGetValue(element, out int current);
            fonkValues[element] = current + value;
            AddMultiplier(element);
        }


        void AddMultiplier(Element element)
        {
            MultiplierValues.TryGetValue(element, out int a);
            a += 1;
            MultiplierValues[element] = a;
        }
    }
}