namespace Runix
{
    public enum Element
    {
        None,
        Physical,
        Fire,
        Water,
        Wind,
        Earth,
        Nature,
        Lightning,
        Ice,
    }
    public enum CardType
    {
        Attack,
        Defense,
        Heal,
        Special,
    }

    public enum TurnEnum
    {
        None,
        BattleStart, // en başda 1 kere çalışacak
        PlayTurn, // kartın oynadğı sıra
        TurnDamage, //hasarın hesaplandığı ve vurulduğu sıra 
        TurnEnd, // tur bitti başa
        BattleEnd // savaş bitti en son bir kere çaılşacak

    }


}