using System.Collections.Generic;
using Runix.UI;
using UnityEngine;

namespace Runix.Damage
{
    public class HpManager : MonoBehaviour
    {
        [SerializeField] int hp, maxDefens, maxHp;
        [SerializeField] Element[] healable;
        [SerializeField] HpSlider hpSlider;
        Dictionary<Element, int> defensValues = new();
        bool isDead;
        public event System.Action OnDeath;

        public float HpPercent
        {
            get { return (float)hp / maxHp; }
        }

        public void AddData(Dictionary<Element, int> attackValue, Dictionary<Element, int> multiplerValue, Dictionary<Element, int> defansValue, Dictionary<Element, int> healValue)
        {
            //heal yapılacak
            DoHeal(healValue);
            //defens eklenecek
            AddDefens(defansValue);
            //attack yapılacak
            TakeDamage(attackValue, multiplerValue);
            hpSlider.ChaneSlider(HpPercent);


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
                damageValue = (attackValue[item] * (multipler * multipler)) * (1 - defens / 100f);
                hp -= Mathf.RoundToInt(damageValue);
            }
            if (hp <= 0)
            {
                isDead = true;
                OnDeath?.Invoke();
            }
        }


    }
}