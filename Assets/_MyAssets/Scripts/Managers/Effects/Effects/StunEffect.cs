using UnityEngine;

namespace Runix.Effect
{
    public class StunEffect : Effect
    {
        public const int AllSlots = 99; // "hepsi" anlamında, slot sayısıyla sınırlanacak

        public int BlockedSlots { get; }

        public StunEffect(int turns, Sprite icon, int blockedSlots = AllSlots) : base(turns, icon)
        {
            BlockedSlots = blockedSlots;
        }
    }
}