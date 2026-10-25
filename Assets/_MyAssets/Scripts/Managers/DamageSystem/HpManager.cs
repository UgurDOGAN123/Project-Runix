using System.Collections.Generic;
using Runix.Managers;
using Runix.UI;
using UnityEngine;

namespace Runix.Damage
{
    public class HpManager : MonoBehaviour
    {

        [SerializeField] int hp;
        [SerializeField] HpSlider hpSlider;
        [SerializeField] int playerID;

        Dictionary<Element, int> defensValues = new();

        public event System.Action OnDeath;

        Character.CharacterSO Character
        { get; set; }



        public float HpPercent
        {
            get { return (float)hp / MaxHp; }
        }

        int MaxHp { get => Character.maxHp; }
        public int MaxDefens { get => Character.maxDefans; }

        private void Awake()
        {
            Character = DeskManager.instance.GetCharacterSO(playerID);
        }
        private void OnEnable()
        {
            hp = MaxHp;
            hpSlider.ChaneSlider(HpPercent);
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
            for (int i = 0; i < Character.healable.Length; i++)
            {
                healValue.TryGetValue(Character.healable[i], out int addHp);
                hp += addHp;

            }
            if (hp > MaxHp)
            {
                hp = MaxHp;
            }
            healValue.Clear();
        }

        void AddDefens(Dictionary<Element, int> defansValue)
        {
            Dictionary<Element, int> defensValusInFonk = new();
            foreach (var item in defansValue.Keys)
            {
                int defans = defansValue[item];
                defensValues.TryGetValue(item, out int addDefans);
                defans += addDefans;

                if (defans > MaxDefens)
                {
                    defans = MaxDefens;
                }
                defensValusInFonk.Add(item, defans);
            }

            defansValue.Clear();
            defensValues = defensValusInFonk;
        }

        void TakeDamage(Dictionary<Element, int> attackValue, Dictionary<Element, int> multiplerValue)
        {
            foreach (var item in attackValue.Keys)
            {
                float damageValue;
                defensValues.TryGetValue(item, out int defens);
                multiplerValue.TryGetValue(item, out int multipler);
                damageValue = attackValue[item] * (multipler * multipler) * (1 - defens / 100f);
                hp -= Mathf.RoundToInt(damageValue);
            }
            if (hp <= 0)
            {
                OnDeath?.Invoke();
            }
            attackValue.Clear();
        }


    }
}