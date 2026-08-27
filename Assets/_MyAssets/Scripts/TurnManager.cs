using System.Collections;
using UnityEngine;

namespace Runix.Turn
{
    public class TurnManager : MonoBehaviour
    {
        public static TurnManager instance;

        public int playerCount, playerNo;
        [SerializeField] TurnEnum turnEnum = TurnEnum.None;




        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this);
            }
        }

        public void NextTurn()
        {
            if ( turnEnum == TurnEnum.None )
            {
                turnEnum = TurnEnum.BattleStart;
                // savaş başladı
            }
            else if (  turnEnum == TurnEnum.BattleStart )
            {
                turnEnum = TurnEnum.PlayTurn;
                // oyunandı
            }
            else if (turnEnum == TurnEnum.PlayTurn && playerCount <= playerNo)
            {
                turnEnum = TurnEnum.TurnDamage;
                // hasar verildi
            }
            else if (playerCount <= playerNo && turnEnum == TurnEnum.TurnDamage)
            {
                if(playerCount <= playerNo)
                {
                    turnEnum = TurnEnum.PlayTurn;
                    playerNo++;
                    // sıradaki oyuncuya geçti
                }
                else
                {
                    turnEnum = TurnEnum.TurnEnd;
                    // tur bitti yeni kart ekleniyor
                }
            }
            else if (turnEnum == TurnEnum.TurnEnd)
            {
                playerNo = 0;
                turnEnum = TurnEnum.PlayTurn;
            }
        }



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