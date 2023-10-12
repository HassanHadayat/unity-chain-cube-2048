using System;
using UnityEngine;



[Serializable]
public struct CubeProperty
{
    public int number;
    public Material colorMaterial;

}

public class CubePropertyGenerator : MonoBehaviour
{
    public static CubePropertyGenerator Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        Instance = this;
    }
    public CubeProperty[] cubeProperties;

    public CubeProperty CreateCubeProperty()
    {
        int numberPower = UnityEngine.Random.Range(1, GameManager.Instance.currMaxPower);

        if ((cubeProperties.Length > 0) && numberPower < cubeProperties.Length - 1)
            return cubeProperties[numberPower - 1];
        else
            return cubeProperties[0];
    }
    public CubeProperty CreateCubeProperty(int numberPower)
    {
        if (numberPower > GameManager.Instance.currMaxPower)
            GameManager.Instance.currMaxPower = numberPower;

        if ((cubeProperties.Length > 0) && numberPower < cubeProperties.Length - 1)
        {
            Debug.Log("Testing => " + (numberPower - 1)); ;
            return cubeProperties[numberPower - 1];
        }
        else
            return cubeProperties[0];
    }
}
