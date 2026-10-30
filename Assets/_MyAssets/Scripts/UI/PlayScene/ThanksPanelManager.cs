using UnityEngine;
using UnityEngine.SceneManagement;

namespace Runix
{
    public class ThanksPanelManager : MonoBehaviour
    {
        public void PlayAgain()
        {
            SceneManager.LoadScene("SampleScene");
        }

        public void Exit()
        {
            Application.Quit();
        }
    }
}
