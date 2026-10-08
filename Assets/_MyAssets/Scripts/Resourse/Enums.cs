namespace Runix
{
    public enum Element
    {
        None,
        Physically,
        Fire,
        Water,
        Wind,
        Earth,
        Nature,
        Lightening,
        Ice,
    }
    public enum CardType
    {
        Attack,
        Defans,
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