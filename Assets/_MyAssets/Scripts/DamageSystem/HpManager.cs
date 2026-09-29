using System.Collections.Generic;
using UnityEngine;

namespace Runix.Damage
{
    public class HpManager : MonoBehaviour
    {
        [SerializeField] int hp, maxDefens, maxHp;
        [SerializeField] Element[] healable;
        Dictionary<Element, int> defensValues = new();

        public float HpPercent
        {
            get { return hp / maxHp; }
        }

        public void AddData(Dictionary<Element, int> attackValue, Dictionary<Element, int> multiplerValue, Dictionary<Element, int> defansValue, Dictionary<Element, int> healValue)
        {
            //heal yapılacak
            DoHeal(healValue);
            //defens eklenecek
            AddDefens(defansValue);
            //attack yapılacak
            TakeDamage(attackValue, multiplerValue);


        }

        void DoHeal(Dictionary<Element, int> healValue)
        {
            for (int i = 0; i < healable.Length; i++)
            {
                healValue.TryGetValue(healable[i], out int addHp);
                hp += addHp;

            }
            if (hp > maxHp)
            {
                hp = maxHp;
            }
        }

        void AddDefens(Dictionary<Element, int> defansValue)
        {
            Dictionary<Element, int> defensValusInFonk = new();
            foreach (var item in defansValue.Keys)
            {
                int defans = defansValue[item];
                defensValues.TryGetValue(item, out int addDefans);
                defans += addDefans;

                if (defans > maxDefens)
                {
                    defans = maxDefens;
                }
                defensValusInFonk.Add(item, defans);
            }

            defansValue.Clear();
            defansValue = defensValusInFonk;
        }

        void TakeDamage(Dictionary<Element, int> attackValue, Dictionary<Element, int> multiplerValue)
        {


            foreach (var item in attackValue.Keys)
            {
                float damageValue;
                defensValues.TryGetValue(item, out int defens);
                multiplerValue.TryGetValue(item, out int multipler);
                damageValue = (attackValue[item] * (multipler * multipler)) * (1 - defens / 100);
                hp -= Mathf.RoundToInt(damageValue);
            }
            if (hp < 0)
            {
                Debug.Log(gameObject.name + " öldü");
            }
        }


    }
}