using Runix.Character;
using Runix.Damage;
using Runix.Effect;
using Runix.Managers;
using UnityEngine;

namespace Runix.Card
{
    [CreateAssetMenu(fileName = "VampiricBite", menuName = "SO/Card/VampiricBite")]
    public class VampiricBite : CardSO
    {
        [SerializeField] int turns = 2;
        [Range(0f, 1f)][SerializeField] float healRatio = 1f;

        public override void UseCard(TurnDataHolder turnDataHolder, DeckManager deckManagers)
        {
            base.UseCard(turnDataHolder, deckManagers);
            var element = deckManagers.Character.GetPrimaryElement();
            turnDataHolder.AddEffect(new VampireEffect(turns, Value, element, deckManagers.GetComponent<HpManager>(), healRatio, ElementImg));
        }
    }
}
