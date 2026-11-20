using Runix.Card;
using UnityEngine;

namespace Runix.UI.CharacterBuild
{
    public class DeckSelectButton : SelectButtonBase<DeckSO>
    {
        protected override Sprite GetIcon(DeckSO d) => d.icon;
        protected override string GetDescription(DeckSO d) => d.description;
    }
}
