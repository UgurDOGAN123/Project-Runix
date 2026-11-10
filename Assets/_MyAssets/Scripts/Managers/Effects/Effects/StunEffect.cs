namespace Runix.Effect
{
    public class StunEffect : Effect
    {
        public const int AllSlots = 99; // "hepsi" anlamında, slot sayısıyla sınırlanacak

        public int BlockedSlots { get; }

        public StunEffect(int turns, int blockedSlots = AllSlots) : base(turns)
        {
            BlockedSlots = blockedSlots;
        }
    }
}