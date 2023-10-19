using System;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    private  Dictionary<int, List<GameObject>> numberCubes = new Dictionary<int, List<GameObject>>();
    public float minUpForce = 5f;
    public float maxUpForce = 7f;

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
        cube.Setup(cubeProperty, true);

        // Play Cube Animation (Initialize Player Cube)
        cube.m_Animator.Play("InitializePlayerCube");

        return cubeGO;
    }

    public void CreateCube(Vector3 instantiatePos, int numberPower)
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


        Vector3 forceDir = Vector3.up * Mathf.Clamp((instantiateEffectUpForce * numberPower), minUpForce, maxUpForce);
        cube.m_Rigidbody.AddForce(forceDir, ForceMode.Impulse);
        // IF THIS CUBE NUMBERS CUBE ALREADY IN LIST
        // -> ADD FORCE IN THAT CUBE DIRECTION
        if (numberCubes.ContainsKey(cubeProperty.number) && numberCubes[cubeProperty.number].Count > 0)
        {
            Vector3 dir = GetClosestCubeDirection(cubeGO, cubeProperty.number);
            dir.y = 0.5f;
            forceDir = dir;
            //forceDir = dir * (instantiateEffectUpForce * numberPower);
            cube.m_Rigidbody.AddForce(forceDir, ForceMode.Impulse);
        }


        // Apply Instantiate Effect (Upward Force & Rotate)
        //Vector3 forceDir = Vector3.up * (instantiateEffectUpForce * numberPower);
        Vector3 randomTorque = UnityEngine.Random.insideUnitSphere * instantiateEffectRotateForce;

        //cube.m_Rigidbody.AddForce(forceDir, ForceMode.Impulse);
        cube.m_Rigidbody.AddTorque(randomTorque, ForceMode.Impulse);

        // Play Cube Animation (Initialize Player Cube)
        cube.m_Animator.Play("InitializeCube");

        AddCube(cubeProperty.number, cubeGO);

    }



    //xxxxxxxxxxxxxxxxxxxxxx Cube Property Getters xxxxxxxxxxxxxxxxxxxxxxx

    // Get Cube Property For Player Cube
    public CubeProperty GetCubeProperty()
    {
        int numberPower = UnityEngine.Random.Range(1, GameManager.Instance.currMaxPower);

        if ((cubeProperties.Length > 0) && numberPower < cubeProperties.Length - 1)
        {
            return cubeProperties[numberPower - 1];
        }
        else
        {
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

    //xxxxxxxxxxxxxxxxxxxxxx Number Cubes Functions xxxxxxxxxxxxxxxxxxxxxxx
    public void AddCube(int number, GameObject cube)
    {
        Debug.Log("Cube Added !");
        if (numberCubes.ContainsKey(number))
        {
            // Key exists in the dictionary.
            numberCubes[number].Add(cube);
        }
        else
        {
            numberCubes.Add(number, new List<GameObject>() { cube });
        }
    }
    public void RemoveCube(int number, GameObject cube)
    {
        Debug.Log("Cube Removed !");
        if (numberCubes.ContainsKey(number))
        {
            // Key exists in the dictionary.
            numberCubes[number].Remove(cube);
            Destroy(cube);
        }
    }
    public Vector3 GetClosestCubeDirection(GameObject thisCube, int number)
    {
        Debug.Log("Finding GetClosestCubeDirection");
        (float dist, GameObject cube) minData = new(int.MaxValue, thisCube);

        if (numberCubes.ContainsKey(number))
        {
            foreach (GameObject cube in numberCubes[number])
            {
                if (Vector3.Distance(thisCube.transform.position, cube.transform.position) < minData.dist)
                {
                    minData = (Vector3.Distance(thisCube.transform.position, cube.transform.position), cube);
                }
            }
        }
        return (minData.cube.transform.position - thisCube.transform.position);
    }
}
