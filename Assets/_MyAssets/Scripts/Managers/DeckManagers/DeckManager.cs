using System;
using System.Collections.Generic;
using Runix.Card;
using Runix.Character;
using Runix.Managers.Anim;
using UnityEngine;

namespace Runix.Managers
{
    public class DeckManager : MonoNewTurnEventConnector
    {
        [Header("Decks")]
        [SerializeField] private List<CardSO> onPickableDeck;
        [SerializeField] private List<CardSO> trashDeck;
        [SerializeField] private List<CardSO> onHandDeck;
        public List<CardSO> deckClone;

        [Header("References")]
        [SerializeField] DeckAnimManager animManager;

        [Header("Variables")]
        [SerializeField] byte playerID;

        DeckModel<CardSO> model;   // deste mantığı (Unity'siz, test edilebilir)

        Func<bool> busyCheck;

        public event Action<List<CardSO>> UpdateUI;  // Her işlemden Sonra tetikle

        int drawingCount;                           // devam eden çekme işlemi sayısı
        public bool IsDrawing => drawingCount > 0;  // animasyon sürüyor mu?

        #region // Fields
        public ICharacterLoadout Character
        {
            get
            { return DeskManager.instance.GetCharacterSO(playerID); }
        }

        List<CardSO> MainDeck => Character.BuildCards();

        public List<CardSO> OnHandDeck { get => onHandDeck; private set => onHandDeck = value; }

        public int TrashDeckCount
        {
            get => trashDeck.Count;
        }

        public int OnPickableDeckCount
        {
            get => onPickableDeck.Count;
        }

        #endregion

        void Awake()
        {
            if (Character == null)
            {
                Debug.LogError("Charhter is empty pleas select a character");
            }
            deckClone = new(MainDeck);
            model = new DeckModel<CardSO>(onPickableDeck, trashDeck, onHandDeck);
        }
        void Start()
        {
            busyCheck = () => IsDrawing;
            TurnControlSystem.TurnControlManager.instance?.RegisterBusyCheck(busyCheck);
        }

        void OnDestroy()
        {
            TurnControlSystem.TurnControlManager.instance?.UnregisterBusyCheck(busyCheck);
        }

        #region // Turn System Event Connect

        protected override async void TurnManagerNewTurnStart(TurnEnum arg1, int arg2)
        {
            // Olay void olmak zorunda: asıl iş HandleTurnAsync'te, hatalar burada yakalanır
            try { await HandleTurnAsync(arg1); }
            catch (System.OperationCanceledException) { } // nesne yok edildi
            catch (System.Exception e) { Debug.LogException(e, this); }
        }
        #endregion

        async Awaitable HandleTurnAsync(TurnEnum stage)
        {
            if (stage == TurnEnum.BattleStart)
            {
                model.StartBattle(deckClone);
                UpdateUI?.Invoke(null);
                UpdateUI?.Invoke(onHandDeck);
                await PickTheHand(Character.DeckLimits.StartCardCount);
            }
            else if (stage == TurnEnum.TurnEnd)
            {
                await PickTheHand(Character.DeckLimits.EveryTurnAddCardCount);
            }
        }



        public async Awaitable PickTheHand(int number)
        {
            drawingCount++;
            try
            {
                for (int i = 0; i < number; i++)
                {
                    if (model.IsHandFull(Character.DeckLimits.MaxHandCardCount)) break;

                    if (model.NeedsReshuffle) await ReplaceToTrashcan();
                    if (!model.TryDraw(out _)) break;                     // önce veri
                    await animManager.AnimPlayer("MoveHand", playerID);   // sonra animasyon
                    UpdateUI?.Invoke(onHandDeck);
                }
            }
            finally
            {
                drawingCount--;   // hata ya da iptal olsa bile sayaç düşer
            }
        }

        public async Awaitable ReplaceToTrashcan()
        {
            model.Reshuffle();
            UpdateUI?.Invoke(null);
            await animManager.AnimPlayer("MoveDeck", playerID);
            UpdateUI?.Invoke(onHandDeck);
        }

        public bool ReturnToHand(CardSO card)
        {
            if (!model.ReturnToHand(card, Character.DeckLimits.MaxHandCardCount)) return false;
            UpdateUI?.Invoke(onHandDeck);
            return true;
        }


        public void MoveToTrash(CardSO card)
        {
            model.Discard(card);   // veri hemen güncellenir
            PlayTrashAnimation();  // animasyon arkada oynar, beklenmez
        }

        async void PlayTrashAnimation()
        {
            try
            {
                await animManager.AnimPlayer("MoveTrash", playerID);
                UpdateUI?.Invoke(onHandDeck);
            }
            catch (System.OperationCanceledException) { }
            catch (System.Exception e) { Debug.LogException(e, this); }
        }

        public void RemoveFromHandDeck(CardSO card)
        {
            model.RemoveFromHand(card);
        }

    }

}
