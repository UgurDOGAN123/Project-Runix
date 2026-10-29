using Runix.Card;
using UnityEngine;

namespace Runix.Managers.Anim
{
    public class DeckAnimManager : MonoBehaviour
    {
        [SerializeField] DeckAnimationController DeckAnimPrefab;
        [SerializeField] Vector3[] AnimStartPos;

        public async Awaitable AnimPlayer(string trigger, byte playerID)
        {

            GameObject _deck = Instantiate(DeckAnimPrefab.gameObject, SetPos(trigger), Quaternion.identity);
            _deck.GetComponent<DeckAnimationController>().PlayAnim(trigger, playerID);
            await Awaitable.WaitForSecondsAsync(1.0f, destroyCancellationToken);

        }


        Vector2 SetPos(string trigger)
        {
            Vector2 vector2 = new(0, 0);

            switch (trigger)
            {

                case "MoveHand":
                    vector2 = AnimStartPos[0];
                    break;

                case "MoveTrash":
                    vector2 = AnimStartPos[1];
                    break;

                case "MoveDeck":
                    vector2 = AnimStartPos[2];
                    break;
            }
            return vector2;

        }
    }
}