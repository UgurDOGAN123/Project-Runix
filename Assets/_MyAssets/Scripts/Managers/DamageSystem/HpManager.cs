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

        DamageCalculator damageCalculator = new DamageCalculator();



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
            hp = damageCalculator.DoHeal(Heal_DefData.HealValues, hp, MaxHp, Character.healable);
            //defens eklenecek
            defenseValues = damageCalculator.AddDefense(defenseValues, Heal_DefData.DefenseValues, MaxDefense);
            //attack yapılacak
            TakeDamage(Attack_Mulitpler.AttackValues, Attack_Mulitpler.MultiplierValues);
            hpSlider.ChangeSlider(HpPercent);


        }

        void TakeDamage(Dictionary<Element, int> attackValue, Dictionary<Element, int> multiplerValue)
        {
            hp = damageCalculator.TakeDamage(attackValue, multiplerValue, defenseValues, passiveDefenseValues, hp);
            Dead();
        }

        public void TakeDirectDamage(int attackValue, Element element = Element.None)
        {
            if (isDead) return;
            hp = damageCalculator.TakeDirectDamage(attackValue, element, passiveDefenseValues, hp);
            hpSlider.ChangeSlider(HpPercent);
            Dead();
        }

        void Dead()
        {
            if (hp <= 0 && !isDead)
            {
                isDead = true;
                OnDeath?.Invoke();
            }
        }


    }
}