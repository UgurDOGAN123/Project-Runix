using Runix.Ability;
using Runix.Card;
using Runix.Character;
using Runix.Core;
using Runix.Desk;
using Runix.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Runix.UI.CharacterBuild
{
    public class StartButton : MonoBehaviour
    {
        [SerializeField] DeckSO elementDeck;
        [SerializeField] DeckSO spacialDeck;
        [SerializeField] AbilitySO abilitySO;
        [SerializeField] DeskEnemiesSO deskEnemiesSO;
        [SerializeField] GameObject ElementDontSelected;
        [SerializeField] GameObject SpacialDontSelected;
        [SerializeField] GameObject AbilityDontSelected;
        [SerializeField] GameObject DeskDontSelected;
        [SerializeField] HpValues HpValues;
        [SerializeField] DeckLimits deckLimits;

        public void GetElementSO(DeckSO elementSO)
        {
            elementDeck = elementSO;
            SetWarning(ElementDontSelected, false);
            AnimPlay();
        }
        public void GetSpacialSO(DeckSO spacialDeck)
        {
            this.spacialDeck = spacialDeck;
            SetWarning(SpacialDontSelected, false);
            AnimPlay();
        }
        public void GetAbilitySO(AbilitySO abilitySO)
        {
            this.abilitySO = abilitySO;
            SetWarning(AbilityDontSelected, false);
            AnimPlay();
        }
        public void GetDeskEnemiesSO(DeskEnemiesSO deskEnemiesSO)
        {
            this.deskEnemiesSO = deskEnemiesSO;
            SetWarning(DeskDontSelected, false);
            AnimPlay();
        }
        public void StartGame()
        {

            if (!ValueCheck())
            {
                return;
            }

            DeskManager.instance.Player = new(name: "player", description: "player", elementDeck: elementDeck, specialDeck: spacialDeck, abilitySO: abilitySO, hpValues: HpValues, deckLimits: deckLimits);
            DeskManager.instance.deskEnemies = deskEnemiesSO;

            DeskManager.instance.ResetRun();

            SceneManager.LoadScene(SceneNameStruct.PlayScene);
            // burada Sahne geçiþi async olaiblir belki bakýlacak
        }

        bool ValueCheck()
        {
            bool isRight = true;
            if (elementDeck == null) { SetWarning(ElementDontSelected, true); isRight = false; }
            if (spacialDeck == null) { SetWarning(SpacialDontSelected, true); isRight = false; }
            if (abilitySO == null) { SetWarning(AbilityDontSelected, true); isRight = false; }
            if (deskEnemiesSO == null) { SetWarning(DeskDontSelected, true); isRight = false; }
            if (HpValues.maxDefense <= 0) { isRight = false; }
            if (HpValues.maxHp <= 0) { isRight = false; }
            if (deckLimits.StartCardCount <= 0) { isRight = false; }
            if (deckLimits.MaxHandCardCount <= 0) { isRight = false; }
            if (deckLimits.EveryTurnAddCardCount <= 0) { isRight = false; }


            return isRight;
        }

        void SetWarning(GameObject gameObject, bool isActive)
        {
            if (gameObject == null)
            {
                Debug.LogError($"{gameObject} null", this);
                return;
            }
            gameObject.SetActive(isActive);
        }
        void AnimPlay()
        {
            if (elementDeck == null) { return; }
            if (spacialDeck == null) { return; }
            if (abilitySO == null) { return; }
            if (deskEnemiesSO == null) { return; }

            GetComponent<UnityEngine.Animation>().Play();
        }
    }
}
