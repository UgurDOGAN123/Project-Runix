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

        Dictionary<Element, int> defenseValues = new();
        Dictionary<Element, int> passiveDefenseValues = new();

        public event System.Action OnDeath;
        Character.CharacterSO Character => DeskManager.instance.GetCharacterSO(playerID);




        public float HpPercent
        {
            get { return (float)hp / MaxHp; }
        }

        int MaxHp { get => Character.maxHp; }
        public int MaxDefense { get => Character.maxDefense; }

        private void OnEnable()
        {
            if (playerID == PlayerIds.Enemy)
            {
                defenseValues.Clear();
                hp = MaxHp;
            }
            isDead = false;
            hpSlider.ChangeSlider(HpPercent);
            passiveDefenseValues = Character.passiveDefense
    .GroupBy(x => x.element)
    .ToDictionary(g => g.Key, g => g.Sum(x => x.Value));
        }
        public void AddData(TurnDataHolder Heal_DefData, TurnDataHolder Attack_Mulitpler)
        {


            //heal yapılacak
            DoHeal(Heal_DefData.HealValues);
            //defens eklenecek
            AddDefense(Heal_DefData.DefenseValues);
            //attack yapılacak
            TakeDamage(Attack_Mulitpler.AttackValues, Attack_Mulitpler.MultiplierValues);
            hpSlider.ChangeSlider(HpPercent);


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

        void AddDefense(Dictionary<Element, int> defansValue)
        {
            foreach (var item in defansValue)
            {
                defenseValues.TryGetValue(item.Key, out int current);
                defenseValues[item.Key] = Mathf.Min(current + item.Value, MaxDefense);
            }

        }

        void TakeDamage(Dictionary<Element, int> attackValue, Dictionary<Element, int> multiplerValue)
        {
            foreach (var item in attackValue.Keys)
            {
                float damageValue;
                defenseValues.TryGetValue(item, out int defens);
                multiplerValue.TryGetValue(item, out int multipler);
                passiveDefenseValues.TryGetValue(item, out int pasiveDefens);
                float reduction = Mathf.Clamp01((defens + pasiveDefens) / 100f);
                damageValue = attackValue[item] * (multipler * multipler) * (1f - reduction);
                hp -= Mathf.RoundToInt(damageValue);
                defenseValues.Remove(item);
            }
            if (hp <= 0 && !isDead)
            {
                isDead = true;
                OnDeath?.Invoke();
            }
        }

        public void TakeDirectDamage(int attackValue, Element element = Element.None)
        {
            if (isDead) return;
            float damageValue;
            passiveDefenseValues.TryGetValue(element, out int pasiveDefens);
            float reduction = Mathf.Clamp01((pasiveDefens) / 100f);
            damageValue = attackValue * (1f - reduction);
            hp -= Mathf.RoundToInt(damageValue);
            hpSlider.ChangeSlider(HpPercent);
            if (hp <= 0 && !isDead)
            {
                isDead = true;
                OnDeath?.Invoke();
            }
        }


    }
}