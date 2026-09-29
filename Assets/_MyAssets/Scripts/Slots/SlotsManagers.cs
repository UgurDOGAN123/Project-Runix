using Runix.Enviorment;
using Runix.Managers;
using Runix.Turn;
using UnityEngine;

namespace Runix.Slots
{
    public class SlotsManagers : MonoBehaviour
    {
        [SerializeField] Slot[] slots;
        [SerializeField] TurnDataManager turnData;
        [SerializeField] DeckManagers deckManager;


        #region // Turn System Event Connect
        private void OnEnable()
        {
            TurnControlSystem.TurnControlManager.instance.NewTurn += TurnManagerNewTurnStart;
        }
        private void OnDisable()
        {
            TurnControlSystem.TurnControlManager.instance.NewTurn -= TurnManagerNewTurnStart;
        }

        private void TurnManagerNewTurnStart(Runix.TurnEnum arg1, int arg2)
        {

            if (arg1 == Runix.TurnEnum.TurnDamage)
            {

                CalculateData(arg2);
            }
        }


        #endregion

        private void CalculateData(int a)
        {
            TurnDataHolder turnDataHolder;

            if (a == 0)
            {
                turnDataHolder = turnData.playerTurnDataHolder;
            }
            else
            {
                turnDataHolder = turnData.enemyTurnDataHolder;
            }
            foreach (Slot slot in slots)
            {
                if (slot.cardSO != null)
                {
                    slot.cardSO.UseCard(turnDataHolder, deckManager);

                }
            }
        }
    }
}