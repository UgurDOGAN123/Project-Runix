using UnityEngine;

namespace Runix
{
    public abstract class MonoNewTurnEventConneter : MonoBehaviour
    {
        private void OnEnable()
        {
            if (TurnControlSystem.TurnControlManager.instance != null)
                TurnControlSystem.TurnControlManager.instance.NewTurn += TurnManagerNewTurnStart;
        }
        private void OnDisable()
        {
            if (TurnControlSystem.TurnControlManager.instance != null)
                TurnControlSystem.TurnControlManager.instance.NewTurn -= TurnManagerNewTurnStart;
        }
        protected abstract void TurnManagerNewTurnStart(TurnEnum arg1, int arg2);


    }
}