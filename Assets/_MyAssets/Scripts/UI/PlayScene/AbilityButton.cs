using Runix.Character;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runix.UI
{
    /// <summary>Oyuncunun yetenek butonu: PlayTurn'de tıklanınca yeteneği kullanır, bekleme süresini gösterir.</summary>
    public class AbilityButton : MonoBehaviour
    {
        [SerializeField] AbilityRunner runner;     // oyuncunun (ownerId 0) AbilityRunner'ı
        [SerializeField] Button button;
        [SerializeField] Image iconImage;
        [SerializeField] GameObject textPanel;
        [SerializeField] TMP_Text cooldownText;    // kalan bekleme turu (boşsa hazır)

        void OnEnable()
        {
            button.onClick.AddListener(OnClick);
            if (runner != null) runner.Changed += Refresh;
            Refresh();
        }

        void OnDisable()
        {
            button.onClick.RemoveListener(OnClick);
            if (runner != null) runner.Changed -= Refresh;
        }

        void OnClick() => runner.TryUse();

        void Refresh()
        {
            if (runner == null) return;

            var ability = runner.Ability;
            if (iconImage != null && ability != null) iconImage.sprite = ability.icon;

            button.interactable = runner.CanUseNow;

            if (cooldownText != null)
                cooldownText.text = runner.CooldownRemaining > 0 ? runner.CooldownRemaining.ToString() : "";
            if (textPanel != null)
            {
                textPanel.SetActive(runner.CooldownRemaining > 0);
            }
        }
    }
}
