using System.Collections.Generic;

namespace Runix.Damage
{
    public class DamageCalculator
    {
        public int DoHeal(Dictionary<Element, int> healValue, int hp, int maxhp, Element[] healables)
        {
            for (int i = 0; i < healables.Length; i++)
            {
                healValue.TryGetValue(healables[i], out int addHp);
                hp += addHp;

            }
            if (hp > maxhp)
            {
                hp = maxhp;
            }
            return hp;
        }

        public Dictionary<Element, int> AddDefense(Dictionary<Element, int> currentDefans, Dictionary<Element, int> newDefans, int maxDefense)
        {
            foreach (var item in newDefans)
            {
                currentDefans.TryGetValue(item.Key, out int current);
                currentDefans[item.Key] = UnityEngine.Mathf.Min(current + item.Value, maxDefense);
            }
            return currentDefans;
        }

        public int TakeDamage(Dictionary<Element, int> attackValue, Dictionary<Element, int> multiplerValue, Dictionary<Element, int> defenseValues, Dictionary<Element, int> passiveDefenseValues, int hp)
        {
            foreach (var item in attackValue.Keys)
            {
                float damageValue;
                defenseValues.TryGetValue(item, out int defens);
                multiplerValue.TryGetValue(item, out int multipler);
                passiveDefenseValues.TryGetValue(item, out int pasiveDefens);
                float reduction = UnityEngine.Mathf.Clamp01((defens + pasiveDefens) / 100f);
                damageValue = attackValue[item] * (multipler * multipler) * (1f - reduction);
                hp -= UnityEngine.Mathf.RoundToInt(damageValue);
                defenseValues.Remove(item);
            }
            return hp;
        }
        public int TakeDirectDamage(int attackValue, Element element, Dictionary<Element, int> passiveDefenseValues, int hp)
        {

            float damageValue;
            passiveDefenseValues.TryGetValue(element, out int pasiveDefens);
            float reduction = UnityEngine.Mathf.Clamp01((pasiveDefens) / 100f);
            damageValue = attackValue * (1f - reduction);
            hp -= UnityEngine.Mathf.RoundToInt(damageValue);

            return hp;

        }





    }
}
