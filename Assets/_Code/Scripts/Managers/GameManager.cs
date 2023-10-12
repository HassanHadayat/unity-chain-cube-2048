using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int currMaxPower = 1;
    
    private void Awake()
    {
        Application.targetFrameRate = Screen.currentResolution.refreshRate;
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        Instance = this;
        currMaxPower = 1;
    }

}
