using Runix.Ability;
using UnityEngine;

namespace Runix.UI.CharacterBuild
{
    public class AbilitySelectButton : SelectButtonBase<AbilitySO>
    {
        protected override Sprite GetIcon(AbilitySO d) => d.icon;
        protected override string GetDescription(AbilitySO d) => d.description;
    }
}
