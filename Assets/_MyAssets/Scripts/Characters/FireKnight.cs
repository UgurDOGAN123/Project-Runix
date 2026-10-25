
using UnityEngine;

namespace Runix.Character
{
    [CreateAssetMenu(fileName = "FireKnight", menuName = "SO/Character/FireKnight")]
    public class FireKnight : CharacterSO
    {
        [Header("Fire Knight Info")]
        public Element element;
        public TurnEnum turnEnum;


        public override void SetData()
        {
            throw new System.NotImplementedException();
        }

        public override void UseAbility()
        {
            throw new System.NotImplementedException();
        }
    }
}