using Runix.Desk;
using UnityEngine;

namespace Runix.UI.CharacterBuild
{
    public class MapSelectButton : SelectButtonBase<DeskEnemiesSO>
    {
        protected override Sprite GetIcon(DeskEnemiesSO d) => d.Icon;
        protected override string GetDescription(DeskEnemiesSO d) => d.Description;
    }
}
