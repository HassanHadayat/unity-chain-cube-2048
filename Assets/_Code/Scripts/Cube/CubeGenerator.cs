using System;
using UnityEngine;



[Serializable]
public struct CubeProperty
{
    public int number;
    public Material colorMaterial;
}



public class CubeGenerator : MonoBehaviour
{
    public static CubeGenerator Instance;

    [SerializeField] private GameObject cubePrefab;
    [SerializeField] private CubeProperty[] cubeProperties;

    [SerializeField] private float instantiateEffectUpForce = 1.25f;
    [SerializeField] private float instantiateEffectRotateForce = 0.25f;
    [SerializeField] private Transform dynamicTrans;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        Instance = this;
    }


    //xxxxxxxxxxxxxxxxxxxxxx Cube Generators xxxxxxxxxxxxxxxxxxxxxxx
    public GameObject CreatePlayerCube(Transform instantiatePos)
    {
        // Get Player Cube Property
        CubeProperty cubeProperty = GetCubeProperty();
        GameObject cubeGO = Instantiate(cubePrefab, instantiatePos);
        Cube cube = cubeGO.GetComponent<Cube>();

        // Apply Player Cube Tag
        cubeGO.tag = "PlayerCube";

        // Gravity OFF & Freeze Rotation ON
        cube.m_Rigidbody.useGravity = false;
        cube.m_Rigidbody.freezeRotation = true;

        // Trigger ON
        cube.m_Collider.isTrigger = true;

        // Setup the Cube Properties
        cube.Setup(cubeProperty);

        return cubeGO;
    }

    public GameObject CreateCube(Vector3 instantiatePos, int numberPower)
    {
        // Get Cube Property
        CubeProperty cubeProperty = GetCubeProperty(numberPower);

        // Instantiate the Cube
        GameObject cubeGO = Instantiate(cubePrefab, instantiatePos, Quaternion.identity, dynamicTrans);
        Cube cube = cubeGO.GetComponent<Cube>();

        // Apply Cube Tag
        cubeGO.tag = "Cube";

        // Gravity ON & Freeze Rotation OFF
        cube.m_Rigidbody.useGravity = true;
        cube.m_Rigidbody.freezeRotation = false;

        // Trigger OFF
        cube.m_Collider.isTrigger = false;

        // Setup the Cube Properties
        cube.Setup(cubeProperty);

        
        // Apply Instantiate Effect (Upward Force & Rotate)
        Vector3 upForce = Vector3.up * (instantiateEffectUpForce * numberPower);
        Vector3 randomTorque = UnityEngine.Random.insideUnitSphere * instantiateEffectRotateForce;

        cube.m_Rigidbody.AddForce(upForce, ForceMode.Impulse);
        cube.m_Rigidbody.AddTorque(randomTorque, ForceMode.Impulse);


        return cubeGO;
    }



    //xxxxxxxxxxxxxxxxxxxxxx Cube Property Getters xxxxxxxxxxxxxxxxxxxxxxx

    // Get Cube Property For Player Cube
    public CubeProperty GetCubeProperty()
    {
        int numberPower = UnityEngine.Random.Range(1, GameManager.Instance.currMaxPower);
        Debug.Log("Number Power = " + numberPower);

        if ((cubeProperties.Length > 0) && numberPower < cubeProperties.Length - 1)
        {
            Debug.Log("Teting - 1");
            return cubeProperties[numberPower - 1];
        }
        else
        {
            Debug.Log("Teting - 2");

            return cubeProperties[0];
        }
    }

    // Get Cube Property For Cube
    public CubeProperty GetCubeProperty(int numberPower)
    {
        if (numberPower > GameManager.Instance.currMaxPower)
            GameManager.Instance.currMaxPower = numberPower;

        if ((cubeProperties.Length > 0) && numberPower < cubeProperties.Length - 1)
        {
            return cubeProperties[numberPower - 1];
        }
        else
            return cubeProperties[0];
    }

}
