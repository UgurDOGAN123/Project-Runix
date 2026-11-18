using Runix.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Runix
{
    public class ThanksPanelManager : MonoBehaviour
    {
        public void PlayAgain()
        {
            SceneManager.LoadScene(SceneNameStruct.CharacterBuild);
        }

        public void Exit()
        {
            Application.Quit();
        }
    }
}
