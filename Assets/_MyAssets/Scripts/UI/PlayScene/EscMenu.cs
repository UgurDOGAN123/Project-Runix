using UnityEngine;

public class EscMenu : MonoBehaviour
{
    [SerializeField] GameObject EscPanel;
    private void Start()
    {
        if (!EscPanel.activeInHierarchy)
            Time.timeScale = 1;
    }
    private void Update()
    {
        if (EscPanel != null)
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                EscPanel.SetActive(!EscPanel.activeInHierarchy);
                if (EscPanel.activeInHierarchy)
                    Time.timeScale = 0;
                else if (!EscPanel.activeInHierarchy)
                    Time.timeScale = 1;
            }
        }
    }
}
