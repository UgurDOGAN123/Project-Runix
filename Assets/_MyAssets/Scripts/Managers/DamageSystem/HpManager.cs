using System.Collections.Generic;
using Runix.Character;
using Runix.Managers;
using Runix.UI;
using UnityEngine;


namespace Runix.Damage
{
    public class HpManager : MonoBehaviour, IDirectDamageable, IHealable
    {

        [SerializeField] int hp;
        [SerializeField] HpSlider hpSlider;
        [SerializeField] int playerID;
        [SerializeField] private bool isDead;

        Dictionary<Element, int> defenseValues = new();
        Dictionary<Element, int> passiveDefenseValues = new();

        public event System.Action OnDeath;
        ICharacterLoadout Character => DeskManager.instance.GetCharacterSO(playerID);

        readonly DamageCalculator damageCalculator = new();





        public float HpPercent
        {
            get { return (float)hp / MaxHp; }
        }

        int MaxHp { get => Character.HpValues.maxHp; }
        public int MaxDefense { get => Character.HpValues.maxDefense; }

        private void Awake()
        {
            if (playerID == PlayerIds.Player) hp = MaxHp;   // run başında bir kez
        }

        private void OnEnable()
        {
            if (playerID == PlayerIds.Enemy)
            {
                defenseValues.Clear();
                hp = MaxHp;
            }
            isDead = false;
            hpSlider.ChangeSlider(HpPercent);
            passiveDefenseValues = Character.GetPassiveDefense();

        }

        public void AddData(TurnDataHolder Heal_DefData, TurnDataHolder Attack_Mulitpler)
        {


            //heal yapılacak
            hp = damageCalculator.DoHeal(Heal_DefData.HealValues, hp, MaxHp, Character.GetHealable());
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
        public void Heal(Element element, int amount)
        {
            if (isDead || amount <= 0) return;
            Dictionary<Element, int> heal = new()
            {
                { element, amount }
            };
            hp = damageCalculator.DoHeal(heal, hp, MaxHp, Character.GetHealable());
            hpSlider.ChangeSlider(HpPercent);
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