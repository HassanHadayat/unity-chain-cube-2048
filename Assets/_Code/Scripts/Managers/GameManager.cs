using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int currMaxPower = 1;
    
    private void Awake()
    {
        Time.timeScale = 1f;


        Application.targetFrameRate = Screen.currentResolution.refreshRate;
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        Instance = this;

        // Set the initial cube number power
        currMaxPower = 1;
    }


    public void OnClick_RestartBtn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
