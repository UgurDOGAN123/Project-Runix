using System.Collections.Generic;
using Runix.Card;
using UnityEngine;

namespace Runix.Damage
{
    public class HpManager : MonoBehaviour
    {
        [SerializeField] int hp,maxDefens,maxHp;
        [SerializeField] Element[] healable;
        Dictionary<Element, int> defensValues = new();

        public void AddData(Dictionary<Element,int> attackValue , Dictionary<Element, int> defansValue, Dictionary<Element, int> healValue)
        {
            //heal yapılacak
            DoHeal(healValue);
            //defens eklenecek
            AddDefens(defansValue);
            //attack yapılacak
            TakeDamage(attackValue);

        }

        void DoHeal(Dictionary<Element, int> healValue)
        {
            for(int i = 0; i < healable.Length; i++) 
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
            foreach (var item in defansValue.Keys)
            {
                int defans = defansValue[item];
                defensValues.TryGetValue(item, out int addDefans);
                defans += addDefans;
                
                if (defans > maxDefens)
                {
                    defans = maxDefens;
                }
                defensValues.Add(item, defans);
            }
        }

        void TakeDamage(Dictionary<Element, int> attackValue)
        {
            
            foreach (var item in attackValue.Keys)
            {
                float damageValue = 0;
                defensValues.TryGetValue(item, out int defens);
                damageValue = attackValue[item] * (1-defens / 100);
                hp -= Mathf.RoundToInt(damageValue);

                Debug.Log(gameObject.name + " " + item + " " + damageValue);
            }
            if (hp < 0)
            {
                Debug.Log(gameObject.name + " öldü");
            }
        }


    }
}