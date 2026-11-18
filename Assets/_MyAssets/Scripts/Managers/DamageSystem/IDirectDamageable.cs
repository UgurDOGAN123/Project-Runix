namespace Runix.Damage
{
    public interface IDirectDamageable
    {
        void TakeDirectDamage(int attackValue, Element element = Element.None);
    }
}