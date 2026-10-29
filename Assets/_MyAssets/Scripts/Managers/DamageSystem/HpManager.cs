using System.Collections.Generic;
using System.Linq;
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
        [SerializeField] private bool isDead;

        Dictionary<Element, int> defensValues = new();
        Dictionary<Element, int> pasiveDefensValues = new();

        public event System.Action OnDeath;
        Character.CharacterSO Character => DeskManager.instance.GetCharacterSO(playerID);




        public float HpPercent
        {
            get { return (float)hp / MaxHp; }
        }

        int MaxHp { get => Character.maxHp; }
        public int MaxDefens { get => Character.maxDefans; }

        private void OnEnable()
        {
            if (playerID == PlayerIds.Enemy)
            {
                defensValues.Clear();
                hp = MaxHp;
            }
            isDead = false;
            hpSlider.ChaneSlider(HpPercent);
            pasiveDefensValues = Character.pasiveDefans
    .GroupBy(x => x.element)
    .ToDictionary(g => g.Key, g => g.Sum(x => x.Value));
        }
        public void AddData(TurnDataHolder Heal_DefData, TurnDataHolder Attack_Mulitpler)
        {


            //heal yapılacak
            DoHeal(Heal_DefData.HealValues);
            //defens eklenecek
            AddDefens(Heal_DefData.DefensValues);
            //attack yapılacak
            TakeDamage(Attack_Mulitpler.AttackValues, Attack_Mulitpler.MultiplerValues);
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
        }

        void AddDefens(Dictionary<Element, int> defansValue)
        {
            foreach (var item in defansValue)
            {
                defensValues.TryGetValue(item.Key, out int current);
                defensValues[item.Key] = Mathf.Min(current + item.Value, MaxDefens);
            }

        }

        void TakeDamage(Dictionary<Element, int> attackValue, Dictionary<Element, int> multiplerValue)
        {
            foreach (var item in attackValue.Keys)
            {
                float damageValue;
                defensValues.TryGetValue(item, out int defens);
                multiplerValue.TryGetValue(item, out int multipler);
                pasiveDefensValues.TryGetValue(item, out int pasiveDefens);
                float reduction = Mathf.Clamp01((defens + pasiveDefens) / 100f);
                damageValue = attackValue[item] * (multipler * multipler) * (1f - reduction);
                hp -= Mathf.RoundToInt(damageValue);
                defensValues.Remove(item);
            }
            if (hp <= 0 && !isDead)
            {
                isDead = true;
                OnDeath?.Invoke();
            }
        }


    }
}