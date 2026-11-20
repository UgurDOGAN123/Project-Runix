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
            DontSelect(ElementDontSelected, false);
            AnimPlay();
        }
        public void GetSpacialSO(DeckSO spacialDeck)
        {
            this.spacialDeck = spacialDeck;
            DontSelect(SpacialDontSelected, false);
            AnimPlay();
        }
        public void GetAbilitySO(AbilitySO abilitySO)
        {
            this.abilitySO = abilitySO;
            DontSelect(AbilityDontSelected, false);
            AnimPlay();
        }
        public void GetDeskEnemiesSO(DeskEnemiesSO deskEnemiesSO)
        {
            this.deskEnemiesSO = deskEnemiesSO;
            DontSelect(DeskDontSelected, false);
            AnimPlay();
        }
        public void StartGame()
        {
            if (elementDeck == null) { DontSelect(ElementDontSelected, true); return; }
            if (spacialDeck == null) { DontSelect(SpacialDontSelected, true); return; }
            if (abilitySO == null) { DontSelect(AbilityDontSelected, true); return; }
            if (deskEnemiesSO == null) { DontSelect(DeskDontSelected, true); return; }
            if (HpValues.maxDefense <= 0) { return; }
            if (HpValues.maxHp <= 0) { return; }
            if (deckLimits.StartCardCount <= 0) { return; }
            if (deckLimits.MaxHandCardCount <= 0) { return; }
            if (deckLimits.EveryTurnAddCardCount <= 0) { return; }


            DeskManager.instance.Player = new(name: "player", description: "player", elementDeck: elementDeck, specialDeck: spacialDeck, abilitySO: abilitySO, hpValues: HpValues, deckLimits: deckLimits);
            DeskManager.instance.deskEnemies = deskEnemiesSO;

            DeskManager.instance.ResetRun();

            SceneManager.LoadScene(SceneNameStruct.PlayScene);
            // burada Sahne geçiþi async olaiblir belki bakýlacak
        }

        void DontSelect(GameObject gameObject, bool isActive)
        {
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
