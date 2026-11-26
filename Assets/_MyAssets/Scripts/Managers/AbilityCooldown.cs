using System;

namespace Runix.Character
{
    /// <summary>
    /// Yetenek bekleme süresi (saf mantık, Unity'den bağımsız, birim test edilebilir).
    /// Sahibinin her PlayTurn'ünde StartTurn çağrılır. Trigger(3): bu turdan 3 tur sonra tekrar kullanılabilir.
    /// </summary>
    public class AbilityCooldown
    {
        int turn;
        int readyAtTurn;

        public bool IsReady => turn >= readyAtTurn;
        public int Remaining => Math.Max(0, readyAtTurn - turn);

        public void Reset()
        {
            turn = 0;
            readyAtTurn = 0;
        }

        public void StartTurn() => turn++;

        public void Trigger(int cooldownTurns) => readyAtTurn = turn + Math.Max(0, cooldownTurns);
    }
}
